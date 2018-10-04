﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Contollers;
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

        private MainGameController MainGameController
        {
            get { return GameObjectsProviderService.MainGameController; }
        }

        #endregion

        public WeaponDefinition WeaponDefinition;

        public void SetWeaponAsCurrent()
        {
            if (!MainGameController.IsTimeFrozen() && MainGameController.GetCurrentPlayer().PlayerType == PlayerType.LocalPlayer)
            {
                GameObjectsProviderService.CurrentWeaponController.SetCurrentWeapon(WeaponDefinition);
            }
        }
    }
}
