﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Contollers.Models;
using Assets.Scripts.Entities.EventArgs;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Contollers
{
    public class WeaponController
    {
        public event EventHandler<IncrementPowerEventArgs> IncrementPowerEvent;
        public event EventHandler<EventArgs> ResetPowerEvent;
        public event EventHandler<WeaponChangedEventArgs> WeaponChangedEvent;

        private readonly WeaponModel _model;

        public WeaponController()
        {
            _model = new WeaponModel();
        }
        
        public void IncrementPower()
        {
            if (_model.CurrentWeapon != WeaponEnum.None)
            {
                if (_model.CurrentPower < _model.MaxPower)
                {
                    if (IncrementPowerEvent != null)
                    {
                        IncrementPowerEvent.Invoke(this, new IncrementPowerEventArgs(_model.CurrentPower));
                    }
                    _model.CurrentPower++;
                }
            }
        }

        public void ResetPower()
        {
            _model.CurrentPower = 0;
            if (ResetPowerEvent != null)
            {
                ResetPowerEvent.Invoke(this, new EventArgs());
            }
        }

        public int GetPower()
        {
            return _model.CurrentPower;
        }

        public WeaponEnum GetCurrentWeapon()
        {
            return _model.CurrentWeapon;
        }

        public void SetCurrentWeapon(WeaponDefinition definition)
        {
            _model.CurrentWeapon = definition.WeaponEnum;
            if (WeaponChangedEvent != null)
            {
                WeaponChangedEvent.Invoke(this, new WeaponChangedEventArgs(definition.WeaponEnum, definition.Sprite));
            }
        }
    }
}
