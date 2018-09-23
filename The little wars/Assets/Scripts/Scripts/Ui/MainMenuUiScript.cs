using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using Assets.Scripts.ScriptableObjects.GameModel;
using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Scripts.Ui
{
    public class MainMenuUiScript : MonoBehaviour
    {
        private int _maxPlayersNumber = 8;

        private PlayerColorsHelper _playerColorsHelper;

        public GameObject MainMenuPanel;
        public GameObject SettingsPanel;
        public GameObject CreateLocalGamePanel;
        public GameObject CreateWebGamePanel;
        public Transform LocalGameNewPlayerPrefab;
        public Transform LocalGameNewPlayerContainer;
        public GameObject LocalGamePlayersContainer;
        public ApplicationModel ApplicationModel;

        public GameObject WebGamePanelRoomsList;
        public GameObject WebGamePanelMatchDetails;

        private void Start()
        {
            MainMenuPanel.SetActive(true);
            SettingsPanel.SetActive(false);
            CreateLocalGamePanel.SetActive(false);
            CreateWebGamePanel.SetActive(false);
        }

        public void LocalGameAddNewPlayer()
        {
            if (LocalGameNewPlayerContainer.childCount < _maxPlayersNumber)
            {
                var createdObject = Instantiate(LocalGameNewPlayerPrefab, LocalGameNewPlayerContainer);
                var playerContainer = createdObject.GetComponent<PlayersContainerScript>();
                playerContainer.SetColor(_playerColorsHelper.GetNextColor());

            }
        }


        public void WebGameCreateRoom()
        {

        }

        public void WebGameJoinRoom()
        {

        }

        public void CreateLocalGame()
        {
            MainMenuPanel.SetActive(false);
            CreateLocalGamePanel.SetActive(true);
            _playerColorsHelper = new PlayerColorsHelper();
        }

        public void CreateWebGame()
        {
            PhotonNetwork.ConnectUsingSettings("v1");
            MainMenuPanel.SetActive(false);
            CreateWebGamePanel.SetActive(true);
            WebGamePanelRoomsList.SetActive(true);
            WebGamePanelMatchDetails.SetActive(false);
            _playerColorsHelper = new PlayerColorsHelper();
        }

        public void StartLocalGame()
        {
            ApplicationModel.ResetMatch();
            ApplicationModel.PlayersToCreate.Clear();
            foreach (Transform child in LocalGameNewPlayerContainer)
            {
                PlayersContainerScript container = child.GetComponent<PlayersContainerScript>();
                if (container != null)
                {
                    ApplicationModel.PlayersToCreate.Add(container.GetPlayerCreationEntity());
                }
            }
            SceneManager.LoadScene(SceneNames.PlayScene);
        }

        public void OpenSettings()
        {
            MainMenuPanel.SetActive(false);
            SettingsPanel.SetActive(true);
        }

        public void BackToMainMenu()
        {
            MainMenuPanel.SetActive(true);
            SettingsPanel.SetActive(false);
            CreateLocalGamePanel.SetActive(false);
            CreateWebGamePanel.SetActive(false);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
