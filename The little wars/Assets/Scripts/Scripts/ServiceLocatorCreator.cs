using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    public class ServiceLocatorCreator : MonoBehaviour
    {
        public void Awake()
        {
            ServiceLocator.Instance = new ServiceLocator();
            ServiceLocator.Instance.Initialize();
        }
    }
}
