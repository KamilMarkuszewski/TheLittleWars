using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Contollers;
using Assets.Scripts.Entities;
using Assets.Scripts.ScriptableObjects.GameModel;
using Assets.Scripts.Scripts;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Utility;
using Photon.Pun;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Services
{
    public class UnitCreatorService : IService
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

        public void CreateSinglePlayerUnit(Vector3 position, Player player)
        {
            var playersCount = MainGameController.ApplicationModel.PlayersToCreate.Count;
            var createdCharacter = Object.Instantiate(UnitPrefabsHelper.GetNextFreeUnitPrefab(playersCount, player.PlayerType), position, Quaternion.identity);
            createdCharacter.transform.parent = GameObjectsProviderService.CharactersParentObject.transform;
            var unitModelScript = createdCharacter.GetComponent<UnitModelScript>();

            unitModelScript.SetColor(player.Color);
            unitModelScript.SetAllowControll(false);
            unitModelScript.SetScopeVisibility(false);
            player.Units.Add(unitModelScript);
        }

        public void CreateMultiplayerUnit(Vector3 position, string playerName, int viewId)
        {
            var players = MainGameController.ApplicationModel.MatchModel.Players;
            var player = players.First(p => p.Name.Equals(playerName));
            //var prefabPath = UnitPrefabsHelper.GetPrefabPath(player.PlayerType);
            //var createdCharacter = PhotonNetwork.Instantiate(prefabPath, position, Quaternion.identity);

            var createdCharacter = Object.Instantiate(UnitPrefabsHelper.GetNextFreeUnitPrefab(players.Count, player.PlayerType), position, Quaternion.identity);

            createdCharacter.transform.parent = GameObjectsProviderService.CharactersParentObject.transform;
            createdCharacter.GetComponent<PhotonView>().ViewID = viewId;
            createdCharacter.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.PlayerList.First(p => p.NickName == playerName));

            var unitModelScript = createdCharacter.GetComponent<UnitModelScript>();

            unitModelScript.SetColor(player.Color);
            Debug.Log(player.Color);
            unitModelScript.SetAllowControll(false);
            unitModelScript.SetScopeVisibility(false);
            unitModelScript.Id = viewId;

            player.Units.Add(unitModelScript);
        }



        #region IService

        public void Initialize()
        {
            Status = ServiceStatus.Initializing;

            Status = ServiceStatus.Ready;
        }

        public ServiceStatus Status
        {
            get; private set;
        }

        #endregion
    }
}
