using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.Scripts.Ui
{
    public class RoomPanelScript : MonoBehaviour
    {
        private RoomInfo _roomInfo;
        public Text RoomNameText;
        public Text RoomPlayersCountText;

        void Enable()
        {
            Assert.IsNull(RoomNameText, "Component value is null");
            Assert.IsNull(RoomPlayersCountText, "Component value is null");
        }

        public void Fill(RoomInfo roomInfo)
        {
            _roomInfo = roomInfo;
            RoomNameText.text = roomInfo.Name;
            RoomPlayersCountText.text = String.Format("{0}/{1}", roomInfo.PlayerCount, roomInfo.MaxPlayers);
        }

        public void OnJoinRoomButtonClicked()
        {
            PhotonNetwork.JoinRoom(_roomInfo.Name);
        }

    }

    
}
