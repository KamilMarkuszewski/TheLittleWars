using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class ServiceLocator
    {
        public static ServiceLocator Instance;

        private readonly Dictionary<object, IService> _services = new Dictionary<object, IService>();

        public void Initialize()
        {
            RegisterServices();
            InitializeServices();
        }

        private void RegisterServices()
        {
            Register<CameraService>();
            Register<GameObjectsProviderService>();
            Register<ResourcesService>();
            Register<ShootService>();
            Register<SoundService>();
            Register<SpawnsService>();
            Register<DestructibleTerrainService>();
            Register<UnitCreatorService>();
            Register<ObjectPoolingService>();
        }

        private void Register<T>() where T : IService, new()
        {
            _services.Add(typeof(T), new T());
        }

        private void InitializeServices()
        {
            Debug.Log("Loading");
            foreach (var serviceItem in _services)
            {
                var service = serviceItem.Value;
                service.Initialize();
            }

            int readyCount = 0;

            while (readyCount < _services.Count)
            {
                int readyLastLoop = readyCount;
                readyCount = _services.Count(si => si.Value.Status == ServiceStatus.Ready);

                if (readyCount > readyLastLoop)
                {
                    Debug.Log("Loaded: " + readyCount * 100 / _services.Count + "%");
                }
            }

            Debug.Log("Ready");
        }

        public static T GetService<T>() where T : IService
        {
            try
            {
                var service = (T)Instance._services[typeof(T)];
                if (service.Status != ServiceStatus.Ready)
                {
                    throw new UnityException(String.Format("The requested service {0} is not ready", typeof(T)));
                }
                return service;
            }
            catch (KeyNotFoundException)
            {
                throw new UnityException(String.Format("The requested service {0} is not registered", typeof(T)));
            }
        }
    }
}
