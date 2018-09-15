using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Scripts;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Entities
{

    public class Unit
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

        public string Name { private set; get; }
        public Color Color { private set; get; }
        public int Hp { private set; get; }
        public Transform UnitTransform;
        public bool AllowControll { private set; get; }

        private CharacterMoveScript _characterMoveScript;
        private CharacterMoveScript CharacterMoveScript
        {
            get
            {
                if (_characterMoveScript == null)
                {
                    _characterMoveScript = UnitTransform.GetComponent<CharacterMoveScript>();
                }
                return _characterMoveScript;
            }
        }

        private SpriteRenderer _spriteRenderer;
        private SpriteRenderer SpriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                {
                    _spriteRenderer = UnitTransform.GetComponentInChildren<SpriteRenderer>();
                }
                return _spriteRenderer;
            }
        }

        public Unit()
        {
            Hp = 100;
            Name = "Some name from pool";
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
            CharacterMoveScript.AllowControll = allowControll;
        }

        public void SetScopeVisibility(bool visibility)
        {
            CharacterMoveScript.SetScopeVisibility(visibility);
        }

        public void Instantiate(Transform characterPrefab, Vector3 spawnPosition, Color color)
        {
            var createdCharacter = UnityEngine.Object.Instantiate(characterPrefab, GameObjectsProviderService.CharactersParentObject.transform);
            createdCharacter.transform.position = spawnPosition;
            UnitTransform = createdCharacter;
            CharacterMoveScript.Unit = this;

            SetColor(color);
            SetAllowControll(false);
            SetScopeVisibility(false);
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
        }
    }
}
