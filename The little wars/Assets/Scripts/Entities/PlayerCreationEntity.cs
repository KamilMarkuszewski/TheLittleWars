using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [Serializable]
    public class PlayerCreationEntity
    {
        private readonly float _red; // Becouse UnityEngine.Color isnt serializable
        private readonly float _green;
        private readonly float _blue;
        public PlayerType PlayerType;
        public int UnitsNumber;
        public int Team;
        public string PlayerName;

        public Dictionary<string, object> AsDistionary()
        {
            return new Dictionary<string, object>
            {
                {"_red", _red},
                {"_green", _green},
                {"_blue", _blue},
                {"PlayerType", PlayerType},
                {"UnitsNumber", UnitsNumber},
                {"Team", Team},
                {"PlayerName", PlayerName}
            };
        }

        public static PlayerCreationEntity FromDictionary(Dictionary<string, object> source)
        {
            return new PlayerCreationEntity(
                (float)source["_red"],
                (float)source["_green"],
                (float)source["_blue"],
                (PlayerType)source["PlayerType"],
                (int)source["UnitsNumber"],
                (int)source["Team"],
                (string)source["PlayerName"]
            );
        }

        public PlayerCreationEntity(float red, float green, float blue, PlayerType playerType, int unitsNumber, int team, string playerName)
        {
            _red = red;
            _green = green;
            _blue = blue;
            PlayerType = playerType;
            UnitsNumber = unitsNumber;
            Team = team;
            PlayerName = playerName;
        }

        public PlayerCreationEntity(Color color, PlayerType playerType, int unitsNumber, int team, string playerName)
        {
            _red = color.r;
            _green = color.g;
            _blue = color.b;
            PlayerType = playerType;
            UnitsNumber = unitsNumber;
            Team = team;
            PlayerName = playerName;
        }

        public Color GetColor()
        {
            return new Color(_red, _green, _blue);
        }

        public override string ToString()
        {
            return "Color: " + GetColor() + "; PlayerType: " + PlayerType + "; UnitsNumber: " + UnitsNumber + "; Team: " + Team;
        }
    }
}
