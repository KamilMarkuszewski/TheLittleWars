using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Entities.EventArgs
{
    public class WeaponChangedEventArgs : System.EventArgs
    {
        public WeaponEnum WeaponEnum;
        public Sprite Sprite;

        public WeaponChangedEventArgs(WeaponEnum weaponEnum, Sprite sprite)
        {
            WeaponEnum = weaponEnum;
            Sprite = sprite;
        }
    }
}
