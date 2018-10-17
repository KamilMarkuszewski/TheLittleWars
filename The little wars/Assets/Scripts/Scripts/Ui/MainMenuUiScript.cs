using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.ScriptableObjects.GameModel;
using Assets.Scripts.Utility;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Assertions;
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

        //void OnEnable()
        //{
        //    Assert.IsNotNull(MainMenuPanel, "Component value is null");
        //    Assert.IsNotNull(SettingsPanel, "Component value is null");
        //    Assert.IsNotNull(CreateLocalGamePanel, "Component value is null");
        //    Assert.IsNotNull(CreateWebGamePanel, "Component value is null");
        //    Assert.IsNotNull(ApplicationModel, "Component value is null");
        //    Assert.IsNotNull(LocalGameNewPlayerPrefab, "Component value is null");
        //    Assert.IsNotNull(WebGameNewPlayerAiPrefab, "Component value is null");
        //    Assert.IsNotNull(WebGameNewPlayerHumanPrefab, "Component value is null");
        //    Assert.IsNotNull(WebGameRoomPrefab, "Component value is null");
        //    Assert.IsNotNull(LocalGamePlayersContainer, "Component value is null");
        //    Assert.IsNotNull(WebGamePanelRoomsList, "Component value is null");
        //    Assert.IsNotNull(WebGamePanelMatchDetails, "Component value is null");
        //    Assert.IsNotNull(WebGamePanelLogin, "Component value is null");
        //    Assert.IsNotNull(WebGameRoomsContainer, "Component value is null");
        //    Assert.IsNotNull(WebGamePlayersContainer, "Component value is null");
        //    Assert.IsNotNull(NickInputField, "Component value is null");
        //    Assert.IsNotNull(RoomNameInputField, "Component value is null");
        //    Assert.IsNotNull(RoomNameText, "Component value is null");
        //}

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
            PhotonNetwork.OfflineMode = true;
            StartGame(LocalGamePlayersContainer);
            SceneManager.LoadScene(SceneNames.PlayScene);
        }

        private void StartGame(GameObject playersContainer)
        {
            ApplicationModel.ResetMatch();
            ApplicationModel.PlayersToCreate.Clear();
            foreach (Transform child in playersContainer.transform)
            {
                PlayersContainerScript container = child.GetComponent<PlayersContainerScript>();
                if (container != null)
                {
                    ApplicationModel.PlayersToCreate.Add(container.GetPlayerCreationEntity());
                }
            }
        }

        public void StartWebGame()
        {
            StartGame(WebGamePlayersContainer);
            if (IsCurrentPlayerRoomOwner())
            {
                SceneManager.LoadScene(SceneNames.PlayScene);
            }
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


        public void WebGameAddNewAiPlayer()
        {
            if (IsCurrentPlayerRoomOwner())
            {
                if (WebGamePlayersContainer.transform.childCount < _maxPlayersNumber)
                {
                    WebGameCreateAiPlayerContainerHost();
                }
            }
        }

        private PlayerCreationEntity GetPlayerCreationEntityByName(string playerName)
        {
            foreach (Transform child in WebGamePlayersContainer.transform)
            {
                PlayersContainerScript playersContainerScript = child.GetComponent<PlayersContainerScript>();
                var ent = playersContainerScript.GetPlayerCreationEntity();
                if (ent.PlayerName.Equals(playerName))
                {
                    return ent;
                }
            }
            return null;
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

        private void WebGameCreateHumanPlayerContainer(PlayerCreationEntity playerCreationEntity)
        {
            var createdObject = Instantiate(WebGameNewPlayerHumanPrefab, WebGamePlayersContainer.transform);
            var playerContainer = createdObject.GetComponent<PlayersContainerScript>();
            var color = playerCreationEntity.GetColor();
            playerContainer.SetColor(color);
            playerContainer.SetTeam(playerCreationEntity.Team);
            playerContainer.SetUnitsNumber(playerCreationEntity.UnitsNumber);
            playerContainer.SetPlayerName(playerCreationEntity.PlayerName);
        }

        private void WebGameCreateAiPlayerContainerHost()
        {
            var createdObject = Instantiate(WebGameNewPlayerAiPrefab, WebGamePlayersContainer.transform);
            var playerContainer = createdObject.GetComponent<PlayersContainerScript>();
            var color = _playerColorsHelper.GetNextColor();
            playerContainer.SetColor(color);
            CustomPropertiesHelper.AddBotToRoomHashtable(playerContainer.GetPlayerCreationEntity());
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
            WebGameRefreshHostGuestList();
        }

        private void WebGameRefreshHostGuestList()
        {
            foreach (Transform child in WebGamePlayersContainer.transform)
            {
                Destroy(child.gameObject);
            }
            var aiPlayers = CustomPropertiesHelper.CurrentRoomGetCustomPropertyPlayers(PhotonPropertiesNames.AiPlayers);
            var humanPlayers = CustomPropertiesHelper.CurrentRoomGetCustomPropertyPlayers(PhotonPropertiesNames.HumanPlayers);

            foreach (var el in aiPlayers)
            {
                var dict = (Dictionary<string, object>)el.Value;
                WebGameCreateAiPlayerContainer(PlayerCreationEntity.FromDictionary(dict));
            }

            foreach (var el in humanPlayers)
            {
                var dict = (Dictionary<string, object>)el.Value;
                WebGameCreateHumanPlayerContainer(PlayerCreationEntity.FromDictionary(dict));
            }
        }

        private List<Color> GetPlayersColors()
        {
            var colors = new List<Color>();
            foreach (Transform child in WebGamePlayersContainer.transform)
            {
                PlayersContainerScript playersContainerScript = child.GetComponent<PlayersContainerScript>();
                if (playersContainerScript.Type.In(PlayerType.LocalPlayer, PlayerType.RemotePlayer))
                {
                    Color color;
                    if (playersContainerScript.TryGetColor(out color))
                    {
                        colors.Add(color);
                    }
                }
            }
            return colors;
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

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            WebGameCreateHumanPlayerContainer(PhotonNetwork.NickName);
            var entity = GetPlayerCreationEntityByName(PhotonNetwork.NickName);
            if (entity != null)
            {
                CustomPropertiesHelper.AddHumanToRoomHashtable(entity);
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("OnPlayerEnteredRoom");
            base.OnPlayerEnteredRoom(newPlayer);
            if (IsCurrentPlayerRoomOwner())
            {
                WebGameCreateHumanPlayerContainer(newPlayer.NickName);
                var entity = GetPlayerCreationEntityByName(newPlayer.NickName);
                CustomPropertiesHelper.AddHumanToRoomHashtable(entity);
            }
            WebGameRefreshPlayersList();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("OnPlayerLeftRoom");
            base.OnPlayerLeftRoom(otherPlayer);
            if (IsCurrentPlayerRoomOwner())
            {
                var entity = GetPlayerCreationEntityByName(otherPlayer.NickName);
                _playerColorsHelper.PutColorBack(entity.GetColor());
                CustomPropertiesHelper.RemoveHumanFromRoomHashtable(entity);
            }
            WebGameRefreshPlayersList();
        }

        private static bool IsCurrentPlayerRoomOwner()
        {
            return PhotonNetwork.LocalPlayer.IsMasterClient;
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            WebGameRefreshPlayersList();
        }


        #endregion

    }
}
