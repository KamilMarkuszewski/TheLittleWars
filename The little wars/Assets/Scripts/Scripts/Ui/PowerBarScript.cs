using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities.EventArgs;
using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Scripts.Ui
{
    public class PowerBarScript : MonoBehaviour
    {
        #region Services

        private SoundService SoundService
        {
            get { return ServiceLocator.GetService<SoundService>(); }
        }

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        #endregion

        public Transform[] Bars;
        private AudioSource _audioSource;

        void Enable()
        {
            Assert.IsNull(Bars, "Component value is null");
        }

        // Use this for initialization
        private void Start()
        {
            Reset();
            _audioSource = SoundService.GetAudioSourceFromPool();
            GameObjectsProviderService.CurrentWeaponController.IncrementPowerEvent += OnIncrementPowerEvent;
            GameObjectsProviderService.CurrentWeaponController.ResetPowerEvent += OnResetPowerEvent;
        }

        private void OnResetPowerEvent(object sender, EventArgs eventArgs)
        {
            Reset();
        }

        private void Reset()
        {
            foreach (var b in Bars)
            {
                b.gameObject.SetActive(false);
            }
            SoundService.StopPlaying(_audioSource);
        }

        private void OnIncrementPowerEvent(object sender, IncrementPowerEventArgs incrementPowerEventArgs)
        {
            if (incrementPowerEventArgs.CurrentPower == 0)
            {
                SoundService.PlayClip(_audioSource, AudioClipsEnum.PowerUp);
            }
            if (incrementPowerEventArgs.CurrentPower < Bars.Length)
            {
                Bars[incrementPowerEventArgs.CurrentPower].gameObject.SetActive(true);
            }
        }
    }
}
