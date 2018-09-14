using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class GameObjectsProviderService : IService
    {
        private GameModel _gameModel;
        public GameModel GameModel
        {
            get
            {
                if (_gameModel == null)
                {
                    _gameModel = GameObject.Find("GameModel").GetComponent<GameModel>();
                }
                return _gameModel;
            }
        }

        private GameObject _audioSourcesGameObject;
        public GameObject AudioSourcesGameObjectGameObject
        {
            get
            {
                if (_audioSourcesGameObject == null)
                {
                    _audioSourcesGameObject = GameObject.Find("AudioSources");
                }
                return _audioSourcesGameObject;
            }
        }


        #region IService

        public void Initialize()
        {
            Status = ServiceStatus.Initializing;

            Status = ServiceStatus.Ready;
        }

        public ServiceStatus Status
        {
            get; private set;
        }

        #endregion
    }
}
