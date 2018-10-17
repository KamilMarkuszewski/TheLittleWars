using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Contollers.Models;
using Assets.Scripts.Entities;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.GameModel
{
    [CreateAssetMenu]
    public class ApplicationModel : ScriptableObject
    {
        public ApplicationModel()
        {
            PlayersToCreate = new List<PlayerCreationEntity>
            {
                new PlayerCreationEntity(Color.red, PlayerType.LocalPlayer, 3, 1, "player 1"),
                new PlayerCreationEntity(Color.green, PlayerType.LocalPlayer, 3, 2, "player 2")
            };
            MatchModel = new MatchModel();
            CurrentWeaponModel = new CurrentWeaponModel();
        }

        public void ResetMatch()
        {
            MatchModel = new MatchModel();
            CurrentWeaponModel = new CurrentWeaponModel();
        }

        public void LoadFromServer()
        {
            ResetMatch();
            PlayersToCreate.Clear();

            var aiPlayers = CustomPropertiesHelper.CurrentRoomGetCustomPropertyPlayers(PhotonPropertiesNames.AiPlayers);
            var humanPlayers = CustomPropertiesHelper.CurrentRoomGetCustomPropertyPlayers(PhotonPropertiesNames.HumanPlayers);

            foreach (var el in aiPlayers)
            {
                var dict = (Dictionary<string, object>)el.Value;
                PlayersToCreate.Add(PlayerCreationEntity.FromDictionary(dict));
            }

            foreach (var el in humanPlayers)
            {
                var dict = (Dictionary<string, object>)el.Value;
                PlayersToCreate.Add(PlayerCreationEntity.FromDictionary(dict));
            }
        }

        public List<PlayerCreationEntity> PlayersToCreate;

        public MatchModel MatchModel;
        public CurrentWeaponModel CurrentWeaponModel;

    }
}
