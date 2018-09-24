using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scripts.Ui
{
    public class ConnectionStatusScript : MonoBehaviour
    {
        private const string ConnectionStatusMessage = "Connection status: ";

        private Text _connectionStatusText;
        private Text ConnectionStatusText
        {
            get { return _connectionStatusText ?? (_connectionStatusText = GetComponent<Text>()); }
        }
    
        public void Update()
        {
            ConnectionStatusText.text = ConnectionStatusMessage + PhotonNetwork.NetworkClientState;
        }
    }
}
