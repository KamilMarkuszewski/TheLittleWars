using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    public class WeaponButtonScript : MonoBehaviour
    {
        #region Services

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        private SoundService SoundService
        {
            get { return ServiceLocator.GetService<SoundService>(); }
        }

        #endregion

        public WeaponDefinition WeaponDefinition;

        public void SetWeaponAsCurrent()
        {
            GameObjectsProviderService.WeaponController.SetCurrentWeapon(WeaponDefinition);
            SoundService.PlayClip(WeaponDefinition.ClipOnButtonClicked);
        }
    }
}
