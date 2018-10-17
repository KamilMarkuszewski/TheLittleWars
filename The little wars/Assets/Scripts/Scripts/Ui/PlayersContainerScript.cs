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
        public PlayerType Type;

        public PlayerCreationEntity GetPlayerCreationEntity()
        {
            Color color = Color.gray;
            PlayerType playerType = PlayerType.None;
            int unitsNumber = 0;
            int team = 0;
            string playerName = String.Empty;

            foreach (Transform child in transform)
            {
                TryGetColor(child.gameObject, ref color);
                TryGetType(child.gameObject, ref playerType);
                TryGetUnitsNumber(child.gameObject, ref unitsNumber);
                TryGetTeam(child.gameObject, ref team);
                TryGetPlayerName(out playerName);
            }

            return new PlayerCreationEntity(color, playerType, unitsNumber, team, playerName);
        }

        private void TryGetType(GameObject child, ref PlayerType playerType)
        {
            if (Type == PlayerType.None)
            {
                int chosenItem = 0;
                if (TryGetValueFromDropdown(child, "TypeDropdown", ref chosenItem))
                {
                    playerType = (PlayerType)(chosenItem + 1);
                }
            }
            else
            {
                playerType = Type;
            }
        }

        private void TryGetTeam(GameObject child, ref int team)
        {
            int chosenItem = 0;
            if (TryGetValueFromDropdown(child, "TeamDropdown", ref chosenItem))
            {
                team = chosenItem + 1;
            }
        }


        private void TryGetUnitsNumber(GameObject child, ref int unitsNumber)
        {
            int chosenItem = 0;
            if (TryGetValueFromDropdown(child, "UnitsNumberDropdown", ref chosenItem))
            {
                unitsNumber = chosenItem + 1;
            }
        }

        private bool TryGetValueFromDropdown(GameObject child, string gameObjectName, ref int chosenItem)
        {
            if (child.name.Equals(gameObjectName))
            {
                var dropdownComponent = child.GetComponent<Dropdown>();
                chosenItem = dropdownComponent.value;
                return true;
            }
            return false;
        }

        private void TryGetColor(GameObject child, ref Color color)
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

        public bool TryGetColor(out Color color)
        {
            foreach (Transform child in transform)
            {
                if (child.name.Equals("ColorImage"))
                {
                    var img = child.GetComponent<Image>();
                    color = img.color;
                    return true;
                }
            }
            color = Color.black;
            return false;
        }



        public bool TryGetPlayerName(out string playerName)
        {
            foreach (Transform child in transform)
            {
                if (child.name.Equals("NameText"))
                {
                    var text = child.GetComponent<Text>();
                    playerName = text.text;
                    return true;
                }
            }
            playerName = String.Empty;
            return false;
        }

        public void SetPlayerName(string playerName)
        {
            foreach (Transform child in transform)
            {
                if (child.name.Equals("NameText"))
                {
                    var text = child.GetComponent<Text>();
                    text.text = playerName;
                }
            }
        }

        public void SetTeam(int team)
        {
            SetDropdownValue("TeamDropdown", team);
        }

        public void SetUnitsNumber(int unitsNumber)
        {
            SetDropdownValue("UnitsNumberDropdown", unitsNumber);
        }

        private void SetDropdownValue(string gameObjectName, int val)
        {
            foreach (Transform child in transform)
            {
                if (child.name.Equals(gameObjectName))
                {
                    var dropdownComponent = child.GetComponent<Dropdown>();
                    dropdownComponent.value = val;
                }
            }
        }
    }
}
