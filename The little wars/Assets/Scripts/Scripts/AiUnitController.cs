using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Scripts
{
    public class AiUnitController : UnitControllerBase
    {
        #region Services

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        #endregion

        public WeaponDefinition UsedWeapon;
        private bool _isShooting;

        // Use this for initialization
        void Start()
        {

        }

        void FixedUpdate()
        {
            if (UnityEngine.Random.Range(0, 100) == 0)
            {
                _isShooting = !_isShooting;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!AllowControll)
            {
                return;
            }

            if (GameObjectsProviderService.CurrentWeaponController.GetCurrentWeapon() == WeaponEnum.None)
            {
                GameObjectsProviderService.CurrentWeaponController.SetCurrentWeapon(UsedWeapon);
            }

            const float xUnitMovement = 0.0f;
            const float yScopeMovement = 0.0f;
            var isShooting = _isShooting;
            const bool isJumping = false;

            UnitMoveScript.ControllCharacter(xUnitMovement, yScopeMovement, isShooting, isJumping);
        }
    }
}
