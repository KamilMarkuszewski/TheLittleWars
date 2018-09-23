using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Services;
using UnityEngine;

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
            if (UnityEngine.Random.Range(0, 200) == 0)
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

            var xUnitMovement = 0.0f;
            var yScopeMovement = 0.0f;
            var isShooting = _isShooting;
            var isJumping = false;

            UnitMoveScript.ControllCharacter(xUnitMovement, yScopeMovement, isShooting, isJumping);
        }
    }
}
