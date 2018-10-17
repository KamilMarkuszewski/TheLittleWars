using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(UnitMoveScript))]
    [RequireComponent(typeof(UnitControllerBase))]
    public class UnitModelScript : MonoBehaviour
    {
        #region Services

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        private SoundService SoundService
        {
            get { return ServiceLocator.GetService<SoundService>(); }
        }

        #endregion

        private SpriteRenderer _spriteRenderer;
        private SpriteRenderer SpriteRenderer
        {
            get { return _spriteRenderer ?? (_spriteRenderer = GetComponentInChildren<SpriteRenderer>()); }
        }

        private UnitMoveScript _unitMoveScript;
        private UnitMoveScript UnitMoveScript
        {
            get { return _unitMoveScript ?? (_unitMoveScript = GetComponent<UnitMoveScript>()); }
        }

        private UnitControllerBase _unitControllerBase;
        private UnitControllerBase UnitControllerBase
        {
            get
            {
                if (_unitControllerBase == null)
                {
                    _unitControllerBase = GetComponent<UnitControllerBase>();
                }
                return _unitControllerBase;
            }
        }

        public int Id { set; get; }
        public string Name { private set; get; }
        public Color Color { private set; get; }
        public bool AllowControll { private set; get; }
        private int _hp = 100;
        public int Hp
        {
            private set { _hp = value; }
            get { return _hp; }
        }

        private void Kill()
        {
            SoundService.PlayClip(AudioClipsEnum.UnitKilled);
            GameObjectsProviderService.MainGameController.MatchController.RemoveUnit(this);

            gameObject.SetActive(false);

            GameObjectsProviderService.MainGameController.EndMatchIfNeeded();
            if (AllowControll)
            {
                GameObjectsProviderService.MainGameController.NewRound();
            }
        }


        public void ChangeHp(int amount)
        {
            var newHp = Hp + amount;
            if (newHp > 100)
            {
                newHp = 100;
            }
            if (newHp < 0)
            {
                newHp = 0;
            }
            else if (amount < 0)
            {
                SoundService.PlayClip(AudioClipsEnum.UnitDamaged);
            }
            Hp = newHp;

            if (Hp < 1)
            {
                Kill();
            }
        }

        public bool IsAlive()
        {
            return Hp > 0;
        }

        public void SetColor(Color color)
        {
            SpriteRenderer.color = color;
            Color = color;
        }

        public void SetAllowControll(bool allowControll)
        {
            AllowControll = allowControll;
            UnitControllerBase.AllowControll = allowControll;
        }

        public void SetScopeVisibility(bool visibility)
        {
            UnitMoveScript.SetScopeVisibility(visibility);
        }


    }
}
