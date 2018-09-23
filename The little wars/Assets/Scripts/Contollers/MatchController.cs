using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Contollers.Models;
using Assets.Scripts.Entities;
using Assets.Scripts.Services;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Contollers
{
    public class MatchController
    {
        #region Services

        private SpawnsService SpawnsService
        {
            get { return ServiceLocator.GetService<SpawnsService>(); }
        }

        #endregion

        private readonly MatchModel _model;

        public MatchController(MatchModel model, ICollection<PlayerCreationEntity> playerCreationEntities)
        {
            _model = model;

            CreatePlayersUnits(playerCreationEntities);
        }

        private void CreatePlayersUnits(ICollection<PlayerCreationEntity> playerCreationEntities)
        {
            var prefabs = new UnitPrefabsContainer();
            var spawns = SpawnsService.GetSpawns();
            foreach (var initValues in playerCreationEntities)
            {
                var p = CreatePlayerUnits(playerCreationEntities.Count, initValues, spawns, prefabs);
                _model.Players.Add(p);
                _model.PlayersQueue.Enqueue(new PlayerQueue(p));
            }
        }

        private static Player CreatePlayerUnits(int playersCount, PlayerCreationEntity playerCreationEntity, List<Vector3> spawns, UnitPrefabsContainer prefabs)
        {
            return new Player(playerCreationEntity, spawns, prefabs.GetNextFreeUnitPrefab(playersCount, playerCreationEntity.PlayerType));
        }

        public void EnqueuePlayer()
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

        public void RemoveUnit(Unit unitToRemove)
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

        private void RemoveUnitFromPlayerQueue(PlayerQueue player, Unit unitToRemove)
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
            _model.CurrentPlayerQueue = _model.PlayersQueue.Dequeue();
            _model.CurrentUnit = _model.CurrentPlayerQueue.UnitsQueue.Dequeue();
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

        public Unit GetCurrenUnit()
        {
            return _model.CurrentUnit;
        }


    }

}
