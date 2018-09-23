using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Entities.EventArgs;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Scripts.Ui
{
    public class WeaponImageScript : MonoBehaviour
    {
        #region Services

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        #endregion

        private UnityEngine.UI.Image _image;
        private UnityEngine.UI.Image Image
        {
            get
            {
                if (_image == null)
                {
                    _image = GetComponent<UnityEngine.UI.Image>();
                }
                return _image;
            }
        }

        void Start()
        {
            GameObjectsProviderService.CurrentWeaponController.WeaponChangedEvent += OnWeaponChangedEvent;
        }

        private void OnWeaponChangedEvent(object sender, WeaponChangedEventArgs weaponChangedEventArgs)
        {
            Image.sprite = weaponChangedEventArgs.Sprite;
        }


    }
}
