using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Scripts.Ui
{
    public class PowerBarScript : MonoBehaviour
    {
        #region Services

        private SoundService SoundService
        {
            get { return ServiceLocator.GetService<SoundService>(); }
        }

        #endregion

        public Transform[] Bars;
        public int CurrentPower { private set; get; }
        private AudioSource _audioSource;

        public void Reset()
        {
            foreach (var b in Bars)
            {
                b.gameObject.SetActive(false);
            }
            CurrentPower = 0;
            SoundService.StopPlaying(_audioSource);
        }

        public void IncrementPower()
        {
            if (CurrentPower == 0)
            {
                SoundService.PlayClip(_audioSource, AudioClipsEnum.PowerUp);
            }
            if (CurrentPower < Bars.Length)
            {
                Bars[CurrentPower].gameObject.SetActive(true);
                CurrentPower++;
            }
        }

        // Use this for initialization
        void Start()
        {
            Reset();
            _audioSource = SoundService.GetAudioSourceFromPool();
        }
    }
}
