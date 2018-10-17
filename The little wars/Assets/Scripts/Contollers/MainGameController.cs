using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using Assets.Scripts.ScriptableObjects.GameModel;
using Assets.Scripts.Scripts;
using Assets.Scripts.Services;
using Assets.Scripts.Utility;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Contollers
{
    public class MainGameController : MonoBehaviour
    {
        #region Services

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        private UnitCreatorService UnitCreatorService
        {
            get { return ServiceLocator.GetService<UnitCreatorService>(); }
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


        internal bool IsTimeFrozen()
        {
            return TimeFrozen;
        }

        public Player GetCurrentPlayer()
        {
            return MatchController.GetCurrentPlayer();
        }

        public void EndMatchIfNeeded()
        {
            if (PhotonHelper.PlayerIsMultiplayerHost() || PhotonNetwork.OfflineMode)
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
        }


        // Use this for initialization
        void Awake()
        {
            if (PhotonHelper.PlayerIsMultiplayerGuest())
            {
                ApplicationModel.LoadFromServer();
            }
            CurrentWeaponController = new CurrentWeaponController(ApplicationModel.CurrentWeaponModel);
            MatchController = new MatchController(ApplicationModel.MatchModel, ApplicationModel.PlayersToCreate);

            _roundLength = 45;
        }



        void Start()
        {
            StartCoroutine(AwakeCoroutine());
        }

        private IEnumerator AwakeCoroutine()
        {
            yield return new WaitForSeconds(1);
            MatchController.CreatePlayersUnits();
            MatchController.EnqueuePlayer();
            StartCoroutine(RoundCoroutine());
        }

        public void CreateUnit(Vector3 position, Player player)
        {
            if (PhotonHelper.PlayerIsMultiplayerHost())
            {
                int id = UniqueIdHelper.GetNext();
                GameObjectsProviderService.MainPhotonView.RPC("RPC_CreateUnit", RpcTarget.All, position, player.Name, id);
            }
            else if(PhotonNetwork.OfflineMode)
            {
                UnitCreatorService.CreateSinglePlayerUnit(position, player);
            }
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

        public void SetTimeTo3Sec()
        {
            _roundStart = Time.time - _roundLength + 3;
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
            if (PhotonHelper.PlayerIsMultiplayerGuest())
            {
                
            }
            else if (PhotonHelper.PlayerIsMultiplayerHost())
            {
                MatchController.DequeuePlayer();
                var unit = MatchController.GetCurrenUnit();
                GameObjectsProviderService.MainPhotonView.RPC("RPC_RoundStart", RpcTarget.All, unit.Id);
            }
            else
            {
                RoundStart();
            }
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

        public void RoundStartFun(int unitId)
        {
            Debug.Log("RPC_RoundStart");
            TimeFrozen = false;
            _roundStart = Time.time;
            var foundUnit = ApplicationModel.MatchModel.Units.First(u => u.Id == unitId);
            if (!PhotonHelper.PlayerIsSinglePlayer())
            {
                MatchController.SetCurrentUnit(foundUnit);
            }
            if (foundUnit.Color == GetCurrentPlayerColor() && GetCurrentPlayer().Name == PhotonNetwork.NickName)
            {
                foundUnit.SetAllowControll(true);
                foundUnit.SetScopeVisibility(true);
            }
            else
            {
                foundUnit.SetAllowControll(false);
                foundUnit.SetScopeVisibility(false);
            }

            CameraService.SetCameraFollowAndRotationTarget(foundUnit.gameObject.transform);

            CurrentWeaponController.ResetPower();
            CurrentWeaponController.SetCurrentWeapon(ShootService.GetNoneWeapon());

            if (RoundChangedEvent != null)
            {
                RoundChangedEvent.Invoke(this, new EventArgs());
            }
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

            CameraService.SetCameraFollowAndRotationTarget(unit.gameObject.transform);

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



