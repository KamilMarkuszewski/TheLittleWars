using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class GameObjectsProviderHelper
    {
        private static GameModel _gameModel;
        public static GameModel GameModel
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

    }
}
