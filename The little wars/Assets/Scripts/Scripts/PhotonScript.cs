using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Contollers;
using Assets.Scripts.Services;
using Photon.Pun;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    [RequireComponent(typeof(PhotonView))]
    public class PhotonScript : MonoBehaviour
    {
        #region Services

        private ShootService ShootService
        {
            get { return ServiceLocator.GetService<ShootService>(); }
        }

        private UnitCreatorService UnitCreatorService
        {
            get { return ServiceLocator.GetService<UnitCreatorService>(); }
        }

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        private MainGameController MainGameController
        {
            get { return GameObjectsProviderService.MainGameController; }
        }


        #endregion

        [PunRPC]
        public void RPC_WeaponShoot(WeaponEnum weaponEnum, Vector3 position)
        {
            ShootService.WeaponShoot(weaponEnum, position);
        }

        [PunRPC]
        public void RPC_CreateUnit(Vector3 position, string playerName, int viewId)
        {
            UnitCreatorService.CreateMultiplayerUnit(position, playerName, viewId);
        }

        [PunRPC]
        public void RPC_RoundStart(int unitId)
        {
            MainGameController.RoundStartFun(unitId);
        }

        [PunRPC]
        public void RPC_SetTimeTo3Sec(int unitId)
        {
            GameObjectsProviderService.MainGameController.SetTimeTo3Sec();
        }

    }
}
