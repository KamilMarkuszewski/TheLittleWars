using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Scripts.Ui
{
    public class GameUiScript : MonoBehaviour
    {
        #region Services

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        #endregion

        public GameObject WeaponsPanel;
        public GameObject ShowWeaponsPanelButton;

        public UnityEngine.UI.Text TimerTextScript;


        void OnEnable()
        {
            Assert.IsNotNull(WeaponsPanel, "Component value is null");
            Assert.IsNotNull(ShowWeaponsPanelButton, "Component value is null");
            Assert.IsNotNull(TimerTextScript, "Component value is null");
        }

        void Start()
        {
            GameObjectsProviderService.MainGameController.RoundChangedEvent += (sender, eventArgs) =>
            {
                TimerTextScript.color = GameObjectsProviderService.MainGameController.GetCurrentPlayer().Color;
            };
        }

        void FixedUpdate()
        {
            string time = GameObjectsProviderService.MainGameController.GetTime();
            TimerTextScript.text = time;
        }

        public void ShowWeaponsPanel()
        {
            WeaponsPanel.SetActive(true);
            ShowWeaponsPanelButton.SetActive(false);
        }

        public void HideWeaponsPanel()
        {
            WeaponsPanel.SetActive(false);
            ShowWeaponsPanelButton.SetActive(true);
        }
    }
}
