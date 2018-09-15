using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using Assets.Scripts.Services;
using UnityEngine;

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
        public WeaponController WeaponController;

        public event EventHandler<EventArgs> RoundChangedEvent;
        public bool GameOver;


        private int _roundLength;
        private float _roundStart;
        private bool _timeFrozen = true;

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
            if (unit.AllowControll)
            {
                if (MatchController.HasPlayersQueueOnlyOneElement())
                {
                    GameOver = true;
                    MatchController.DequeuePlayer();
                    NewRound();
                    Debug.Log("Player " + MatchController.GetCurrentPlayer().Color + " won! ");
                }
                else if (MatchController.IsPlayersQueueEmpty())
                {
                    GameOver = true;
                    NewRound();
                    Debug.Log("Cthulhu ftaghn! ");
                }


            }
        }


        // Use this for initialization
        void Awake()
        {
            WeaponController = new WeaponController();
            MatchController = new MatchController(
                new PlayerInitValues(Color.red, 3),
                new PlayerInitValues(Color.blue, 3)
                );

            _roundLength = 434435;

            NewRound();
        }



        public string GetTime()
        {
            int seconds = (int)(_roundLength - (Time.time - _roundStart));
            if (_timeFrozen)
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
            if (MatchController.HasPlayersQueueOnlyOneElement())
            {
                GameOver = true;
                MatchController.DequeuePlayer();
                Debug.Log("Player " + MatchController.GetCurrentPlayer().Color + " won! ");
            }
            else if (MatchController.IsPlayersQueueEmpty())
            {
                GameOver = true;
                Debug.Log("Cthulhu ftaghn! ");
            }
            else
            {
                StartCoroutine(RoundCoroutine());
            }
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
            _timeFrozen = true;
        }

        private void RoundStart()
        {
            _timeFrozen = false;
            _roundStart = Time.time;
            MatchController.DequeuePlayer();
            var unit = MatchController.GetCurrenUnit();
            unit.SetAllowControll(true);
            unit.SetScopeVisibility(true);

            CameraService.SetCameraFollowAndRotationTarget(unit.UnitTransform);

            WeaponController.ResetPower();
            WeaponController.SetCurrentWeapon(ShootService.GetNoneWeapon());

            if (RoundChangedEvent != null)
            {
                RoundChangedEvent.Invoke(this, new EventArgs());
            }
        }
    }
}



