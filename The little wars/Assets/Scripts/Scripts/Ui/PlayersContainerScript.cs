using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scripts.Ui
{
    public class PlayersContainerScript : MonoBehaviour
    {
        public PlayerCreationEntity GetPlayerCreationEntity()
        {
            Color color = Color.gray;
            PlayerType playerType = PlayerType.None;
            int unitsNumber = 0;
            int team = 0;

            foreach (Transform child in transform)
            {
                TryGetColor(child.gameObject, ref color);
                TryGetType(child.gameObject, ref playerType);
                TryGetUnitsNumber(child.gameObject, ref unitsNumber);
                TryGetTeam(child.gameObject, ref team);
            }

            return new PlayerCreationEntity(color, playerType, unitsNumber, team);
        }

        private static void TryGetType(GameObject child, ref PlayerType playerType)
        {
            int chosenItem = 0;
            if (TryGetValueFromDropdown(child, "TypeDropdown", ref chosenItem))
            {
                playerType = (PlayerType)(chosenItem + 1);
            }
        }

        private static void TryGetTeam(GameObject child, ref int team)
        {
            int chosenItem = 0;
            if (TryGetValueFromDropdown(child, "TeamDropdown", ref chosenItem))
            {
                team = chosenItem + 1;
            }
        }


        private static void TryGetUnitsNumber(GameObject child, ref int unitsNumber)
        {
            int chosenItem = 0;
            if (TryGetValueFromDropdown(child, "UnitsNumberDropdown", ref chosenItem))
            {
                unitsNumber = chosenItem + 1;
            }
        }

        private static bool TryGetValueFromDropdown(GameObject child, string gameObjectName, ref int chosenItem)
        {
            if (child.name.Equals(gameObjectName))
            {
                var dropdownComponent = child.GetComponent<Dropdown>();
                chosenItem = dropdownComponent.value;
                return true;
            }
            return false;
        }

        private static void TryGetColor(GameObject child, ref Color color)
        {
            if (child.name.Equals("ColorImage"))
            {
                var image = child.GetComponent<Image>();
                if (image != null)
                {
                    color = image.color;
                }
            }
        }

        public void SetColor(Color color)
        {
            foreach (Transform child in transform)
            {
                if (child.name.Equals("ColorImage"))
                {
                    var img = child.GetComponent<Image>();
                    img.color = color;
                }
            }
        }
    }
}
