using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Scripts;
using Assets.Scripts.Services;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.ExtensionMethods;

namespace Assets.Scripts.Entities
{
    public class Player
    {
        public Color Color;
        public int Team;
        public PlayerType PlayerType;
        public List<UnitModelScript> Units = new List<UnitModelScript>();
        public string Name;
        public int UnitsCount;

        public Player(PlayerCreationEntity playerCreationEntity)
        {
            Team = playerCreationEntity.Team;
            PlayerType = playerCreationEntity.PlayerType;
            Color = playerCreationEntity.GetColor();
            Name = playerCreationEntity.PlayerName;
            UnitsCount = playerCreationEntity.UnitsNumber;
        }

        public bool HasAliveUnits()
        {
            return Units.Any(u => u.IsAlive());
        }
    }
}
