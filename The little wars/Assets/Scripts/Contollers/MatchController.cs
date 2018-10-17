using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Contollers.Models;
using Assets.Scripts.Entities;
using Assets.Scripts.Scripts;
using Assets.Scripts.Services;
using Assets.Scripts.Utility;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Contollers
{
    public class MatchController
    {
        #region Services

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        private SpawnsService SpawnsService
        {
            get { return ServiceLocator.GetService<SpawnsService>(); }
        }

        #endregion

        private readonly MatchModel _model;

        public MatchController(MatchModel model, ICollection<PlayerCreationEntity> playerCreationEntities)
        {
            _model = model;
            foreach (var initValues in playerCreationEntities)
            {
                var player = new Player(initValues);
                _model.Players.Add(player);
            }
            Debug.Log("_model.Players " + _model.Players.Count);
        }

        public void CreatePlayersUnits()
        {
            if (!PhotonNetwork.OfflineMode)
            {
                foreach (var player in _model.Players)
                {
                    if (player.PlayerType != PlayerType.Ai)
                    {
                        player.PlayerType = player.Name.Equals(PhotonNetwork.NickName) ? PlayerType.LocalPlayer : PlayerType.RemotePlayer;
                    }
                }
            }
            var spawns = SpawnsService.GetSpawns();
            foreach (var player in _model.Players)
            {
                CreatePlayerUnits(player, player.UnitsCount, spawns);
                _model.PlayersQueue.Enqueue(new PlayerQueue(player));
            }
        }

        private void CreatePlayerUnits(Player player, int unitsCount, List<Vector3> spawns)
        {
            if (PhotonNetwork.OfflineMode || PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                for (int i = 0; i < unitsCount; i++)
                {
                    GameObjectsProviderService.MainGameController.CreateUnit(SpawnsService.GetNextSpawn(spawns), player);
                }
            }
        }

        public void EnqueuePlayer()
        {
            if (PhotonNetwork.OfflineMode || PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (_model.CurrentPlayerQueue != null && _model.CurrentPlayerQueue.Player.HasAliveUnits())
                {
                    if (_model.CurrentUnit != null && _model.CurrentUnit.IsAlive())
                    {
                        _model.CurrentPlayerQueue.UnitsQueue.Enqueue(_model.CurrentUnit);
                    }
                    _model.PlayersQueue.Enqueue(_model.CurrentPlayerQueue);
                }
                _model.CurrentPlayerQueue = null;
                _model.CurrentUnit = null;
            }
        }

        public void RemoveUnit(UnitModelScript unitToRemove)
        {
            if (_model.CurrentPlayerQueue != null && _model.CurrentPlayerQueue.UnitsQueue.Contains(unitToRemove))
            {
                RemoveUnitFromPlayerQueue(_model.CurrentPlayerQueue, unitToRemove);
            }
            else
            {
                foreach (var playerQueue in _model.PlayersQueue)
                {
                    RemoveUnitFromPlayerQueue(playerQueue, unitToRemove);
                }
                for (int i = 0; i < _model.PlayersQueue.Count; i++)
                {
                    var p = _model.PlayersQueue.Dequeue();
                    if (p.Player.HasAliveUnits())
                    {
                        _model.PlayersQueue.Enqueue(p);
                    }
                }
            }
        }

        private void RemoveUnitFromPlayerQueue(PlayerQueue player, UnitModelScript unitToRemove)
        {
            for (int i = 0; i < player.UnitsQueue.Count; i++)
            {
                var u = player.UnitsQueue.Dequeue();
                if (u != unitToRemove)
                {
                    player.UnitsQueue.Enqueue(u);
                }
            }
        }

        public void DequeuePlayer()
        {
            if (PhotonNetwork.OfflineMode || PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                _model.CurrentPlayerQueue = _model.PlayersQueue.Dequeue();
                _model.CurrentUnit = _model.CurrentPlayerQueue.UnitsQueue.Dequeue();
            }
        }

        public Player GetCurrentPlayer()
        {
            return _model.CurrentPlayerQueue.Player;
        }

        public bool IsPlayersQueueEmpty()
        {
            return !_model.Players.Any(p => p.HasAliveUnits());
        }

        public bool HasPlayersQueueOnlyOneTeam()
        {
            return _model.Players.GroupBy(players => players.Team).Count(g => g.Any(queue => queue.HasAliveUnits())) == 1;
        }

        public UnitModelScript GetCurrenUnit()
        {
            return _model.CurrentUnit;
        }

        public void SetCurrentUnit(UnitModelScript unit)
        {
            _model.CurrentUnit = unit;
            _model.PlayersQueue.Enqueue(_model.CurrentPlayerQueue);
            _model.CurrentPlayerQueue = _model.PlayersQueue.First(p => p.Player.Color == unit.Color);
        }


    }

}
