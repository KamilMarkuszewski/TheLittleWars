using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Scripts.Ui
{
    public class WeaponButtonScript : MonoBehaviour
    {
        #region Services

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        #endregion

        public WeaponDefinition WeaponDefinition;

        public void SetWeaponAsCurrent()
        {
            GameObjectsProviderService.CurrentWeaponController.SetCurrentWeapon(WeaponDefinition);
        }
    }
}
