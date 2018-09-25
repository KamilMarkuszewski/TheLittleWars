using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using Assets.Scripts.ScriptableObjects.GameModel;
using Assets.Scripts.Utility;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Player = Photon.Realtime.Player;

namespace Assets.Scripts.Scripts.Ui
{
    public class MainMenuUiScript : MonoBehaviourPunCallbacks
    {
        private byte _maxPlayersNumber = 8;
        private bool _inLobby;
        private string _playerName;

        private PlayerColorsHelper _playerColorsHelper;

        [Header("Main Panels")]
        public GameObject MainMenuPanel;
        public GameObject SettingsPanel;
        public GameObject CreateLocalGamePanel;
        public GameObject CreateWebGamePanel;

        [Header("Models")]
        public ApplicationModel ApplicationModel;

        [Header("Prefabs")]
        public Transform LocalGameNewPlayerPrefab;
        public Transform WebGameNewPlayerAiPrefab;
        public Transform WebGameNewPlayerHumanPrefab;
        public Transform WebGameRoomPrefab;

        [Header("LocalGame")]
        public GameObject LocalGamePlayersContainer;

        [Header("WebGame")]
        public GameObject WebGamePanelRoomsList;
        public GameObject WebGamePanelMatchDetails;
        public GameObject WebGamePanelLogin;
        public GameObject WebGameRoomsContainer;
        public GameObject WebGamePlayersContainer;
        public InputField NickInputField;
        public InputField RoomNameInputField;
        public Text RoomNameText;

        private void Start()
        {
            ActivatePanel(MainMenuPanel);
        }

        private void ActivatePanel(params GameObject[] panelToActivate)
        {
            MainMenuPanel.SetActive(false);
            SettingsPanel.SetActive(false);
            CreateLocalGamePanel.SetActive(false);
            CreateWebGamePanel.SetActive(false);
            WebGamePanelRoomsList.SetActive(false);
            WebGamePanelMatchDetails.SetActive(false);
            WebGamePanelLogin.SetActive(false);

            panelToActivate.ToList().ForEach(t => t.SetActive(true));
        }

        public void LocalGameAddNewPlayer()
        {
            if (LocalGamePlayersContainer.transform.childCount < _maxPlayersNumber)
            {
                LocalGameCreatePlayerContainer();
            }
        }

        private void LocalGameCreatePlayerContainer()
        {
            var createdObject = Instantiate(LocalGameNewPlayerPrefab, LocalGamePlayersContainer.transform);
            var playerContainer = createdObject.GetComponent<PlayersContainerScript>();
            var color = _playerColorsHelper.GetNextColor();
            playerContainer.SetColor(color);
        }

        public void CreateLocalGame()
        {
            ActivatePanel(CreateLocalGamePanel);
            _playerColorsHelper = new PlayerColorsHelper();
        }

        public void CreateWebGame()
        {
            ActivatePanel(CreateWebGamePanel, WebGamePanelLogin);
            _playerColorsHelper = new PlayerColorsHelper();
            _inLobby = true;
        }

        public void StartLocalGame()
        {
            ApplicationModel.ResetMatch();
            ApplicationModel.PlayersToCreate.Clear();
            foreach (Transform child in LocalGamePlayersContainer.transform)
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
            ActivatePanel(SettingsPanel);
        }

        public void BackToMainMenu()
        {
            ActivatePanel(MainMenuPanel);
            _inLobby = false;
            PhotonNetwork.Disconnect();
        }

        public void WebGameBackRoomList()
        {
            WebGameCleanPlayersList();
            ActivatePanel(CreateWebGamePanel, WebGamePanelRoomsList);
            PhotonNetwork.LeaveRoom();
            Invoke("WebGameRefreshRoomsList", 1.0f);
        }

        public void Exit()
        {
            Application.Quit();
        }


        public void WebGameCreateRoom()
        {
            string roomName = RoomNameInputField.text;
            if (!String.IsNullOrEmpty(roomName))
            {
                var options = new RoomOptions { MaxPlayers = _maxPlayersNumber };
                PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
            }
        }

        public void WebGameLoginButtonClicked()
        {
            _playerName = NickInputField.text;
            if (_playerName.Equals(""))
            {
                Debug.LogError("Player Name is invalid.");
                return;
            }
            ConnectIntoPhoton();
        }

        public void WebGameRefreshRoomsList()
        {
            PhotonNetwork.Disconnect();
        }

        private void AddBotToRoomHashtable(PlayerCreationEntity player)
        {
            var aiPlayers = CurrentRoomGetAiPlayers();
            aiPlayers.Add(player.GetColor().ToString(), player.AsDistionary());
            var hash = new Hashtable
            {
                {PhotonPropertiesNames.AiPlayers, aiPlayers}
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        private static Dictionary<string, object> CurrentRoomGetAiPlayers()
        {
            return (Dictionary<string, object>)PhotonNetwork.CurrentRoom.CustomProperties[PhotonPropertiesNames.AiPlayers] ?? new Dictionary<string, object>();
        }

        public void WebGameAddNewAiPlayer()
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (WebGamePlayersContainer.transform.childCount < _maxPlayersNumber)
                {
                    WebGameCreateAiPlayerContainer();
                }
            }
        }

        private void WebGameCreateAiPlayerContainer(PlayerCreationEntity playerCreationEntity)
        {
            var createdObject = Instantiate(WebGameNewPlayerAiPrefab, WebGamePlayersContainer.transform);
            var playerContainer = createdObject.GetComponent<PlayersContainerScript>();
            var color = playerCreationEntity.GetColor();
            playerContainer.SetColor(color);
            playerContainer.SetTeam(playerCreationEntity.Team);
            playerContainer.SetUnitsNumber(playerCreationEntity.UnitsNumber);
        }

        private void WebGameCreateAiPlayerContainer()
        {
            var createdObject = Instantiate(WebGameNewPlayerAiPrefab, WebGamePlayersContainer.transform);
            var playerContainer = createdObject.GetComponent<PlayersContainerScript>();
            var color = _playerColorsHelper.GetNextColor();
            playerContainer.SetColor(color);
            AddBotToRoomHashtable(playerContainer.GetPlayerCreationEntity());
        }

        private void WebGameCreateHumanPlayerContainer(string playerName)
        {
            var createdObject = Instantiate(WebGameNewPlayerHumanPrefab, WebGamePlayersContainer.transform);
            var playerContainer = createdObject.GetComponent<PlayersContainerScript>();
            var color = _playerColorsHelper.GetNextColor();
            playerContainer.SetColor(color);
            playerContainer.SetPlayerName(playerName);
        }

        private void RefreshRoomsList(List<RoomInfo> roomList)
        {
            foreach (Transform child in WebGameRoomsContainer.transform)
            {
                Destroy(child.gameObject);
            }
            roomList.ForEach(CreateRoomContainer);
        }

        private void CreateRoomContainer(RoomInfo room)
        {
            var createdObject = Instantiate(WebGameRoomPrefab, WebGameRoomsContainer.transform);
            var roomContainer = createdObject.GetComponent<RoomPanelScript>();
            roomContainer.Fill(room);
        }

        private void WebGameCleanPlayersList()
        {
            foreach (Transform child in WebGamePlayersContainer.transform)
            {
                PlayersContainerScript playersContainerScript = child.GetComponent<PlayersContainerScript>();
                Color color;
                if (playersContainerScript.TryGetColor(out color))
                {
                    _playerColorsHelper.PutColorBack(color);
                }
                Destroy(child.gameObject);
            }
        }

        private void WebGameRefreshPlayersList()
        {
            foreach (Transform child in WebGamePlayersContainer.transform)
            {
                PlayersContainerScript playersContainerScript = child.GetComponent<PlayersContainerScript>();
                Color color;
                if (playersContainerScript.TryGetColor(out color))
                {
                    _playerColorsHelper.PutColorBack(color);
                }
                Destroy(child.gameObject);
            }
            var aiPlayers = CurrentRoomGetAiPlayers();

            foreach (var el in aiPlayers)
            {
                var dict = (Dictionary<string, object>)el.Value;
                WebGameCreateAiPlayerContainer(PlayerCreationEntity.FromDictionary(dict));
            };
            PhotonNetwork.PlayerList.ToList().ForEach(player => WebGameCreateHumanPlayerContainer(player.NickName));
        }

        #region Photon

        private void ConnectIntoPhoton()
        {
            PhotonNetwork.LocalPlayer.NickName = _playerName;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.LeaveRoom();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            base.OnConnectedToMaster();
            if (_inLobby)
            {
                PhotonNetwork.JoinLobby(TypedLobby.Default);
                ActivatePanel(CreateWebGamePanel, WebGamePanelRoomsList);
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected");
            base.OnDisconnected(cause);
            if (_inLobby)
            {
                Invoke("ConnectIntoPhoton", 1.0f);
            }
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            base.OnJoinedRoom();
            if (_inLobby)
            {
                RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
                ActivatePanel(CreateWebGamePanel, WebGamePanelMatchDetails);
            }
            Invoke("WebGameRefreshPlayersList", 1.0f);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("OnRoomListUpdate");
            base.OnRoomListUpdate(roomList);
            if (_inLobby)
            {
                RefreshRoomsList(roomList);
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("OnPlayerEnteredRoom");
            base.OnPlayerEnteredRoom(newPlayer);
            WebGameRefreshPlayersList();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("OnPlayerLeftRoom");
            base.OnPlayerLeftRoom(otherPlayer);
            WebGameRefreshPlayersList();
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            WebGameRefreshPlayersList();
        }

        #endregion

    }
}
