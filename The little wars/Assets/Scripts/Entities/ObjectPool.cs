using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class ObjectPool<T>
    {
        private readonly Queue<T> _objects;
        private readonly Func<T> _objectGenerator;

        public ObjectPool(Func<T> objectGenerator)
        {
            _objects = new Queue<T>();
            _objectGenerator = objectGenerator;
        }

        public T GetObject()
        {
            if (_objects.Any())
            {
                if (_objects.Peek() != null)
                {
                    return _objects.Dequeue();
                }
            }

            return _objectGenerator();
        }

        public void PutObject(T item)
        {
            _objects.Enqueue(item);
        }
    }
}