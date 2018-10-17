using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Entities.EventArgs;
using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scripts.Ui
{
    [RequireComponent(typeof(Image))]
    public class WeaponImageScript : MonoBehaviour
    {
        #region Services

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        #endregion

        private Image _image;
        private Image Image
        {
            get
            {
                if (_image == null)
                {
                    _image = GetComponent<Image>();
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
