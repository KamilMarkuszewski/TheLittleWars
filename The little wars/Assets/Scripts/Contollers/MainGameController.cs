using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using Assets.Scripts.ScriptableObjects.GameModel;
using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Contollers
{
    public class MainGameController : MonoBehaviour
    {
        #region Services

        private SoundService SoundService
        {
            get { return ServiceLocator.GetService<SoundService>(); }
        }

        private ShootService ShootService
        {
            get { return ServiceLocator.GetService<ShootService>(); }
        }

        private CameraService CameraService
        {
            get { return ServiceLocator.GetService<CameraService>(); }
        }

        #endregion

        public MatchController MatchController;
        public CurrentWeaponController CurrentWeaponController;
        public ApplicationModel ApplicationModel;

        public event EventHandler<EventArgs> RoundChangedEvent;
        public bool GameOver;


        private int _roundLength;
        private float _roundStart;
        public bool TimeFrozen = true;

        public Player GetCurrentPlayer()
        {
            return MatchController.GetCurrentPlayer();
        }

        public void ChangeHp(Unit unit, int amount)
        {
            unit.ChangeHp(amount);
            if (unit.Hp < 1)
            {
                Kill(unit);
            }
        }

        private void Kill(Unit unit)
        {
            SoundService.PlayClip(AudioClipsEnum.UnitKilled);
            MatchController.RemoveUnit(unit);

            unit.UnitTransform.gameObject.SetActive(false);

            EndMatchIfNeeded();
            if (unit.AllowControll)
            {
                NewRound();
            }
        }

        private void EndMatchIfNeeded()
        {
            if (MatchController.HasPlayersQueueOnlyOneTeam())
            {
                GameOver = true;
                MatchController.DequeuePlayer();
                SceneManager.LoadScene(SceneNames.GameOverScene);
            }
            else if (MatchController.IsPlayersQueueEmpty())
            {
                GameOver = true;
                SceneManager.LoadScene(SceneNames.GameOverScene);
            }
        }


        // Use this for initialization
        void Awake()
        {
            CurrentWeaponController = new CurrentWeaponController(ApplicationModel.CurrentWeaponModel);
            MatchController = new MatchController(ApplicationModel.MatchModel, ApplicationModel.PlayersToCreate);

            _roundLength = 434435;

            NewRound();
        }



        public string GetTime()
        {
            int seconds = (int)(_roundLength - (Time.time - _roundStart));
            if (TimeFrozen)
            {
                seconds = 0;
            }

            var span = new TimeSpan(0, 0, seconds);
            if (seconds < 0)
            {
                NewRound();
            }
            return string.Format("{0}:{1:00}", span.Minutes, span.Seconds);
        }

        public void NewRound()
        {
            RoundEnd();
            MatchController.EnqueuePlayer();
            StartCoroutine(RoundCoroutine());
        }

        private IEnumerator RoundCoroutine()
        {
            yield return new WaitForSeconds(2);
            RoundStart();
        }

        public Color GetCurrentPlayerColor()
        {
            return MatchController.GetCurrentPlayer().Color;
        }

        private void RoundEnd()
        {
            var unit = MatchController.GetCurrenUnit();
            if (unit != null)
            {
                unit.SetAllowControll(false);
                unit.SetScopeVisibility(false);
            }
            TimeFrozen = true;
            CurrentWeaponController.SetCurrentWeapon(ShootService.GetNoneWeapon());
            EndMatchIfNeeded();
        }

        private void RoundStart()
        {
            TimeFrozen = false;
            _roundStart = Time.time;
            MatchController.DequeuePlayer();
            var unit = MatchController.GetCurrenUnit();
            unit.SetAllowControll(true);
            unit.SetScopeVisibility(true);

            CameraService.SetCameraFollowAndRotationTarget(unit.UnitTransform);

            CurrentWeaponController.ResetPower();
            CurrentWeaponController.SetCurrentWeapon(ShootService.GetNoneWeapon());

            if (RoundChangedEvent != null)
            {
                RoundChangedEvent.Invoke(this, new EventArgs());
            }
            EndMatchIfNeeded();
        }
    }
}



