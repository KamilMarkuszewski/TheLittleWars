using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Contollers.Models
{
    public class WeaponModel
    {
        public int MaxPower = 24;
        public int CurrentPower;
        public WeaponEnum CurrentWeapon { get; set; }
    }
}
