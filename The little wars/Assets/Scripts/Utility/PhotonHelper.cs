using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Pun;

namespace Assets.Scripts.Utility
{
    public static class PhotonHelper
    {
        public static bool PlayerIsMultiplayerGuest()
        {
            return !PhotonNetwork.OfflineMode && !PhotonNetwork.LocalPlayer.IsMasterClient;
        }

        public static bool PlayerIsMultiplayerHost()
        {
            return !PhotonNetwork.OfflineMode && PhotonNetwork.LocalPlayer.IsMasterClient;
        }

        public static bool PlayerIsSinglePlayer()
        {
            return PhotonNetwork.OfflineMode;
        }


    }
}
