using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Scripts;
using UnityEngine;

namespace Assets.Scripts.Entities
{

    public class Unit
    {
        public string Name { private set; get; }
        public Color Color { private set; get; }
        public int Hp { private set; get; }
        public Transform UnitTransform;
        public bool AllowControll { private set; get; }

        private CharacterMoveScript _characterMoveScript;
        private CharacterMoveScript characterMoveScript
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
        private SpriteRenderer spriteRenderer
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

        public void SetColor(Color color)
        {
            spriteRenderer.color = color;
            Color = color;
        }

        public void SetAllowControll(bool allowControll)
        {
            AllowControll = allowControll;
            characterMoveScript.AllowControll = allowControll;
        }

        public void SetScopeVisibility(bool visibility)
        {
            characterMoveScript.SetScopeVisibility(visibility);
        }

        public void Instantiate(Transform characterPrefab, Transform charactersParentObject, Vector3 spawnPosition, Color color)
        {
            var createdCharacter = UnityEngine.Object.Instantiate(characterPrefab, charactersParentObject);
            createdCharacter.transform.position = spawnPosition;
            UnitTransform = createdCharacter;
            characterMoveScript.Unit = this;

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
            Hp = newHp;
            Debug.Log(newHp);
        }
    }
}
