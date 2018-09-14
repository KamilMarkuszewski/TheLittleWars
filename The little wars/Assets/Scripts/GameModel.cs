using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Scripts;
using Assets.Scripts.Scripts.Ui;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameModel : MonoBehaviour
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

        private SpawnsService SpawnsService
        {
            get { return ServiceLocator.GetService<SpawnsService>(); }
        }

        #endregion

        public List<Player> Players = new List<Player>();
        public Transform CharacterPrefab;
        public Transform CharactersParentObject;
        public UnityEngine.UI.Image CurrentWeaponUiImage;
        public PowerBarScript PowerBarScript;

        public WeaponEnum CurrentWeapon;

        public void SetCurrentWeapon(WeaponDefinition definition)
        {
            CurrentWeapon = definition.WeaponEnum;
            CurrentWeaponUiImage.sprite = definition.Sprite;
        }

        public void IncrementPower()
        {
            if (CurrentWeapon != WeaponEnum.None)
            {
                PowerBarScript.IncrementPower();
            }
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
            Players.First(p => p.Color == unit.Color).Units.Remove(unit);
            unit.UnitTransform.gameObject.SetActive(false);
            if (unit.AllowControll)
            {
                NewRound();
            }
        }

        public void ResetPower()
        {
            PowerBarScript.Reset();
        }

        public int GetPower()
        {
            return PowerBarScript.CurrentPower;
        }

        private int _roundLength;
        private float _roundStart;
        private bool _timeFrozen = true;
        private int _currentPlayer = -1;
        private int _currentUnit = -1;

        public event EventHandler<EventArgs> RoundChangedEvent;




        // Use this for initialization
        void Start()
        {
            var spawns = SpawnsService.GetSpawns();

            Players.Add(new Player(Color.red, 3, spawns, CharacterPrefab, CharactersParentObject));
            Players.Add(new Player(Color.blue, 3, spawns, CharacterPrefab, CharactersParentObject));


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

        public Player GetCurrentPlayer()
        {
            return Players[_currentPlayer];
        }

        public Unit GetCurrenUnit()
        {
            return GetCurrentPlayer().Units[_currentUnit];
        }

        public Color GetCurrentPlayerColor()
        {
            return GetCurrentPlayer().Color;
        }

        public void NewRound()
        {
            Players.Remove(Players.FirstOrDefault(p => p.Units.Count == 0));
            if (Players.Count == 1)
            {
                Debug.Log("Player " + Players[0].Color + " won! ");
            }
            else if (Players.Count == 0)
            {
                Debug.Log("Cthulhu ftaghn! ");
            }
            else
            {
                StartCoroutine(RoundCoroutine());
            }
        }

        private IEnumerator RoundCoroutine()
        {
            RoundEnd();
            yield return new WaitForSeconds(2);
            RoundStart();
        }

        private void RoundEnd()
        {
            if (_currentUnit >= 0 && _currentPlayer >= 0 && GetCurrenUnit() != null)
            {
                GetCurrenUnit().SetAllowControll(false);
                GetCurrenUnit().SetScopeVisibility(false);
            }
            _timeFrozen = true;
        }

        private void RoundStart()
        {
            _timeFrozen = false;
            _roundStart = Time.time;
            _currentPlayer = (_currentPlayer + 1) % Players.Count;
            _currentUnit = (_currentUnit + 1) % GetCurrentPlayer().Units.Count;
            GetCurrenUnit().SetAllowControll(true);
            GetCurrenUnit().SetScopeVisibility(true);

            CameraService.SetCameraFollowAndRotationTarget(GetCurrenUnit().UnitTransform);

            PowerBarScript.Reset();
            SetCurrentWeapon(ShootService.GetNoneWeapon());
            CurrentWeapon = WeaponEnum.None;

            if (RoundChangedEvent != null)
            {
                RoundChangedEvent.Invoke(this, null);
            }
        }

    }
}



