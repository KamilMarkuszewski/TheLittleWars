using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using ExitGames.Client.Photon;
using Photon.Pun;

namespace Assets.Scripts.Utility
{
    public static class CustomPropertiesHelper
    {
        public static Dictionary<string, object> CurrentRoomGetCustomPropertyPlayers(string propName)
        {
            return (Dictionary<string, object>)PhotonNetwork.CurrentRoom.CustomProperties[propName] ?? new Dictionary<string, object>();
        }

        public static void RemoveHumanFromRoomHashtable(PlayerCreationEntity player)
        {
            var humanPlayers = CurrentRoomGetCustomPropertyPlayers(PhotonPropertiesNames.HumanPlayers);
            humanPlayers.Remove(player.GetColor().ToString());
            var hash = new Hashtable
            {
                {PhotonPropertiesNames.HumanPlayers, humanPlayers}
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        public static void AddHumanToRoomHashtable(PlayerCreationEntity player)
        {
            var humanPlayers = CurrentRoomGetCustomPropertyPlayers(PhotonPropertiesNames.HumanPlayers);
            humanPlayers.Add(player.GetColor().ToString(), player.AsDistionary());
            var hash = new Hashtable
            {
                {PhotonPropertiesNames.HumanPlayers, humanPlayers}
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        public static void AddBotToRoomHashtable(PlayerCreationEntity player)
        {
            var aiPlayers = CurrentRoomGetCustomPropertyPlayers(PhotonPropertiesNames.AiPlayers);
            aiPlayers.Add(player.GetColor().ToString(), player.AsDistionary());
            var hash = new Hashtable
            {
                {PhotonPropertiesNames.AiPlayers, aiPlayers}
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

    }
}
