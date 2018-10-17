using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class IdContainer
    {
        private int _lastId;
        private readonly int _maxId;

        public int GetNext()
        {
            _lastId++;
            if (_lastId < _maxId)
            {
                return _lastId;
            }
            throw new UnityException("Too much Ids");
        }

        public IdContainer(int beginId, int len)
        {
            _lastId = beginId;
            _maxId = len + beginId;
        }
    }
}
