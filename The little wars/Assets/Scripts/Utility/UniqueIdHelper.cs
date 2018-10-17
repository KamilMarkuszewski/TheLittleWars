using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public static class UniqueIdHelper
    {
        private static int _reservedForHardcoded = 100;
        private static int _mainContainerLen = 1000;
        private static readonly int WeaponsStart = _reservedForHardcoded + _mainContainerLen;
        private static int _weaponLen = 100;

        private static readonly IdContainer MainContainer = new IdContainer(_reservedForHardcoded, _mainContainerLen);
        private static readonly IdContainer[] UsedPerWeapon;

        static UniqueIdHelper()
        {
            UsedPerWeapon = new IdContainer[(int)WeaponEnum.MaxId];
            for (int i = 0; i < (int)WeaponEnum.MaxId; i++)
            {
                UsedPerWeapon[i] = new IdContainer(WeaponsStart + i * _weaponLen, _weaponLen);
            }
        }

        public static int GetNextForWeapon(WeaponEnum weapon)
        {
            return UsedPerWeapon[(int)weapon].GetNext();
        }

        public static int GetNext()
        {
            return MainContainer.GetNext();
        }
    }
}
