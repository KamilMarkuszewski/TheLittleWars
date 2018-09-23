using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class PlayerCreationEntity
    {
        public Color Color;
        public PlayerType PlayerType;
        public int UnitsNumber;
        public int Team;

        public PlayerCreationEntity(Color color, PlayerType playerType, int unitsNumber, int team)
        {
            Color = color;
            PlayerType = playerType;
            UnitsNumber = unitsNumber;
            Team = team;
        }

        public override string ToString()
        {
            return "Color: " + Color + "; PlayerType: " + PlayerType + "; UnitsNumber: " + UnitsNumber + "; Team: " + Team;
        }
    }
}
