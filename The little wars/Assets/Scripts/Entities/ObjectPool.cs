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
        private readonly Action<T> _objectDeactivator;
        private readonly Action<T> _objectActivator;

        public ObjectPool(Func<T> objectGenerator, Action<T> objectActivator, Action<T> objectDeactivator, int objectsToGenerateCount)
        {
            _objects = new Queue<T>();
            _objectGenerator = objectGenerator;
            for (int i = 0; i < objectsToGenerateCount; i++)
            {
                _objects.Enqueue(Generate());
            }
            _objectActivator = objectActivator;
            _objectDeactivator = objectDeactivator;
        }

        public void PutObject(T item)
        {
            if (_objectDeactivator != null)
            {
                _objectDeactivator(item);
            }
            _objects.Enqueue(item);
        }

        public T GetObject()
        {
            if (_objects.Any())
            {
                if (_objects.Peek() != null)
                {
                    return Dequeue();
                }
            }

            return Generate();
        }

        private T Generate()
        {
            var item2 = _objectGenerator();
            if (_objectDeactivator != null)
            {
                _objectDeactivator(item2);
            }
            if (_objectActivator != null)
            {
                _objectActivator(item2);
            }
            return item2;
        }

        private T Dequeue()
        {
            var item = _objects.Dequeue();
            if (_objectActivator != null)
            {
                _objectActivator(item);
            }
            return item;
        }
    }
}