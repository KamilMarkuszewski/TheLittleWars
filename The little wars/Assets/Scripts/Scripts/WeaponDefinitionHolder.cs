using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Scripts
{
    public class WeaponDefinitionHolder : MonoBehaviour
    {
        public WeaponDefinition WeaponDefinition;

        void Enable()
        {
            Assert.IsNull(WeaponDefinition, "Component value is null");
        }
    }
}
