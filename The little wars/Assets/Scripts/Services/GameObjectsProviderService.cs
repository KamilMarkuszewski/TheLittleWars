using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Contollers;
using Assets.Scripts.Scripts.Ui;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Services
{
    public class GameObjectsProviderService : IService
    {
        public CurrentWeaponController CurrentWeaponController
        {
            get { return MainGameController.CurrentWeaponController; }
        }

        private MainGameController _mainGameController;
        public MainGameController MainGameController
        {
            get
            {
                if (_mainGameController == null)
                {
                    _mainGameController = GameObject.Find("MainGameController").GetComponent<MainGameController>();
                }
                return _mainGameController;
            }
        }

        private GameObject _audioSourcesGameObject;
        public GameObject AudioSourcesGameObject
        {
            get
            {
                if (_audioSourcesGameObject == null)
                {
                    _audioSourcesGameObject = GameObject.Find("AudioSources");
                }
                return _audioSourcesGameObject;
            }
        }


        private GameObject _bulletsParentObject;
        public GameObject BulletsParentObject
        {
            get
            {
                if (_bulletsParentObject == null)
                {
                    _bulletsParentObject = GameObject.Find("Bullets");
                }
                return _bulletsParentObject;
            }
        }

        private GameObject _charactersParentObject;
        public GameObject CharactersParentObject
        {
            get
            {
                if (_charactersParentObject == null)
                {
                    _charactersParentObject = GameObject.Find("Characters");
                }
                return _charactersParentObject;
            }
        }

        private Image _currentWeaponUiImage;
        public Image CurrentWeaponUiImage
        {
            get
            {
                if (_currentWeaponUiImage == null)
                {
                    _currentWeaponUiImage = GameObject.Find("WeaponImage").GetComponent<Image>();
                }
                return _currentWeaponUiImage;
            }
        }

        private PowerBarScript _powerBarScript;
        public PowerBarScript PowerBarScript
        {
            get
            {
                if (_powerBarScript == null)
                {
                    _powerBarScript = GameObject.Find("PowerBar").GetComponent<PowerBarScript>();
                }
                return _powerBarScript;
            }
        }

        #region IService

        public void Initialize()
        {
            Status = ServiceStatus.Initializing;

            Status = ServiceStatus.Ready;
        }

        public ServiceStatus Status
        {
            get; private set;
        }

        #endregion
    }
}
