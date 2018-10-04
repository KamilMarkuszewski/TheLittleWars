using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Contollers.Models;
using Assets.Scripts.Entities;
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

        public List<PlayerCreationEntity> PlayersToCreate;
        public MatchModel MatchModel;
        public CurrentWeaponModel CurrentWeaponModel;

    }
}
