using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Entities;
using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    public class CharacterMoveScript : MonoBehaviour
    {
        public Unit Unit;
        private bool lookRight = true;
        public bool AllowControll;

        private Rigidbody2D _rigidbody;

        private float _t;
        private float _xToJump;
        private Vector2 _moveDir;
        public float MoveSpeed = 0.1f;
        private Vector2 _moveAmount;
        private Vector2 _smoothMoveVelocity;

        public Transform Scope;
        public Transform CharacterSprite;

        // Use this for initialization
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Update is called once per frame
        void Update()
        {
            ControllCharacter();
        }

        public void SetScopeVisibility(bool visibility)
        {
            var sr = Scope.GetComponent<SpriteRenderer>();
            sr.enabled = visibility;
        }


        private float fireTimer = 0;
        private void ControllFire()
        {
            if (Input.GetButton("Fire2"))
            {
                if (fireTimer < Time.time)
                {
                    fireTimer = Time.time + 0.06f;
                    GameObjectsProviderHelper.GameModel.IncrementPower();
                }
            }
            else
            {
                int power = GameObjectsProviderHelper.GameModel.GetPower();
                if (power > 0)
                {
                    var direction = lookRight ? Scope.right : Scope.right * -1;
                    ShootHelper.Shoot(GameObjectsProviderHelper.GameModel.CurrentWeapon, transform.position, direction.normalized, power);
                    GameObjectsProviderHelper.GameModel.ResetPower();
                    if (ShootHelper.ShouldEndRound(GameObjectsProviderHelper.GameModel.CurrentWeapon))
                    {
                        GameObjectsProviderHelper.GameModel.NewRound();
                    }
                }
            }

        }

        private void ControllScope()
        {
            var moveScope = Input.GetAxis("Vertical");
            var x = Input.GetAxis("Horizontal");
            if ((lookRight && x < 0) || (!lookRight && x > 0))
            {
                lookRight = !lookRight;
                CharacterSprite.transform.localScale = new Vector3(-CharacterSprite.transform.localScale.x, CharacterSprite.transform.localScale.y, CharacterSprite.transform.localScale.z);

            }
            else
            {
                if (!lookRight)
                {
                    moveScope = -moveScope;
                }

                float localEulerAnglesZ = Scope.transform.localEulerAngles.z + moveScope;
                if ((localEulerAnglesZ <= 90 && localEulerAnglesZ >= -10) || (localEulerAnglesZ <= 370 && localEulerAnglesZ >= 270))
                {
                    Scope.transform.Rotate(new Vector3(0, 0, 1), moveScope);
                }
            }
        }

        private void ControllCharacter()
        {
            if (!AllowControll)
            {
                return;
            }

            ControllScope();
            ControllFire();

            var y = Input.GetButton("Jump") ? 1.0f : 0.0f;
            var x = Input.GetAxis("Horizontal");

            if (!_isGroundedForWalk)
            {
                x = 0.0f;
            }
            if (y < 0)
            {
                y = 0;
            }
            else
            {
                if (!_isGroundedForJump)
                {
                    if (_t < Time.time)
                    {
                        y = 0;
                        _xToJump = 0;
                    }
                    else
                    {
                        x = _xToJump;
                        y = 1.0f;
                    }
                }
                else if (y > 0 && _t < Time.time)
                {
                    if (x > 0.1f)
                    {
                        _xToJump = 2.0f;
                    }
                    else if (x < -0.1f)
                    {
                        _xToJump = -2.0f;
                    }
                    else
                    {
                        _xToJump = 0;
                    }
                    y = 1.0f;
                    _t = Time.time + 0.4f;
                }
            }

            _moveDir = new Vector2(x, y) * MoveSpeed;
            _moveAmount = Vector2.SmoothDamp(_moveAmount, _moveDir, ref _smoothMoveVelocity, 0.15f);
        }

        void FixedUpdate()
        {
            var a = transform.TransformDirection(new Vector3(_moveAmount.x, _moveAmount.y * 2, 0));
            _rigidbody.MovePosition(_rigidbody.position + new Vector2(a.x, a.y));
        }

        #region Grounded

        GameObject _groundedOn;
        public bool _isGroundedForJump;
        public bool _isGroundedForWalk;

        void OnCollisionEnter2D(Collision2D theCollision)
        {
            foreach (ContactPoint2D contact in theCollision.contacts)
            {
                if (Mathf.Abs(contact.normal.y - transform.up.normalized.y) < 0.99f)
                {
                    _isGroundedForWalk = true;
                    _groundedOn = theCollision.gameObject;
                }
                if (Mathf.Abs(contact.normal.y - transform.up.normalized.y) < 0.99f)
                {
                    _isGroundedForJump = true;
                    _groundedOn = theCollision.gameObject;
                    break;
                }
            }
        }

        void OnCollisionExit2D(Collision2D theCollision)
        {
            if (theCollision.gameObject == _groundedOn)
            {
                _groundedOn = null;
                _isGroundedForWalk = false;
                _isGroundedForJump = false;
            }
        }

        #endregion
    }
}

