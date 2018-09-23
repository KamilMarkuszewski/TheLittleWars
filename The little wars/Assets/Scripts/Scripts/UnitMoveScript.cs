using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Entities;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class UnitMoveScript : MonoBehaviour
    {
        #region Services

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        private ShootService ShootService
        {
            get { return ServiceLocator.GetService<ShootService>(); }
        }

        #endregion

        public Unit Unit;
        private bool _lookRight = true;
        private float _fireTimer;

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

        public void SetScopeVisibility(bool visibility)
        {
            var sr = Scope.GetComponent<SpriteRenderer>();
            sr.enabled = visibility;
        }

        private void ControllFire(bool isShooting)
        {
            if (isShooting)
            {
                StartShooting();
            }
            else
            {
                StopShooting();
            }

        }

        private void StopShooting()
        {
            int power = GameObjectsProviderService.CurrentWeaponController.GetPower();
            if (power > 0)
            {
                var direction = _lookRight ? Scope.right : Scope.right * -1;
                ShootService.Shoot(GameObjectsProviderService.CurrentWeaponController.GetCurrentWeapon(), transform.position,
                    direction.normalized, power);
                GameObjectsProviderService.CurrentWeaponController.ResetPower();
                if (ShootService.ShouldRoundEnd(GameObjectsProviderService.CurrentWeaponController.GetCurrentWeapon()))
                {
                    GameObjectsProviderService.MainGameController.NewRound();
                }
            }
        }

        private void StartShooting()
        {
            if (_fireTimer < Time.time)
            {
                _fireTimer = Time.time + 0.06f;
                GameObjectsProviderService.MainGameController.CurrentWeaponController.IncrementPower();
            }
        }

        private void ControllScope(float yScopeMovement, float xMovement)
        {
            if ((_lookRight && xMovement < 0) || (!_lookRight && xMovement > 0))
            {
                _lookRight = !_lookRight;
                CharacterSprite.transform.localScale = new Vector3(-CharacterSprite.transform.localScale.x, CharacterSprite.transform.localScale.y, CharacterSprite.transform.localScale.z);

            }
            else
            {
                if (!_lookRight)
                {
                    yScopeMovement = -yScopeMovement;
                }

                float localEulerAnglesZ = Scope.transform.localEulerAngles.z + yScopeMovement;
                if ((localEulerAnglesZ <= 90 && localEulerAnglesZ >= -10) || (localEulerAnglesZ <= 370 && localEulerAnglesZ >= 270))
                {
                    Scope.transform.Rotate(new Vector3(0, 0, 1), yScopeMovement);
                }
            }
        }

        public void ControllCharacter(float xUnitMovement, float yScopeMovement, bool isShooting, bool isJumping)
        {
            ControllScope(yScopeMovement, xUnitMovement);
            ControllFire(isShooting);
            ControllMovement(xUnitMovement, isJumping);
        }

        private void ControllMovement(float xUnitMovement, bool isJumping)
        {
            var jumpMovement = isJumping ? 1.0f : 0.0f;

            if (!_isGroundedForWalk)
            {
                xUnitMovement = 0.0f;
            }
            if (jumpMovement < 0)
            {
                jumpMovement = 0;
            }
            else
            {
                if (!_isGroundedForJump)
                {
                    if (_t < Time.time)
                    {
                        jumpMovement = 0;
                        _xToJump = 0;
                    }
                    else
                    {
                        xUnitMovement = _xToJump;
                        jumpMovement = 1.0f;
                    }
                }
                else if (jumpMovement > 0 && _t < Time.time)
                {
                    if (xUnitMovement > 0.1f)
                    {
                        _xToJump = 2.0f;
                    }
                    else if (xUnitMovement < -0.1f)
                    {
                        _xToJump = -2.0f;
                    }
                    else
                    {
                        _xToJump = 0;
                    }
                    jumpMovement = 1.0f;
                    _t = Time.time + 0.4f;
                }
            }

            _moveDir = new Vector2(xUnitMovement, jumpMovement) * MoveSpeed;
            _moveAmount = Vector2.SmoothDamp(_moveAmount, _moveDir, ref _smoothMoveVelocity, 0.15f);
        }

        void FixedUpdate()
        {
            var a = transform.TransformDirection(new Vector3(_moveAmount.x, _moveAmount.y * 2, 0));
            _rigidbody.MovePosition(_rigidbody.position + new Vector2(a.x, a.y));
        }

        #region Grounded

        private GameObject _groundedOn;
        private bool _isGroundedForJump;
        private bool _isGroundedForWalk;

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
                SetNotGrounded();
            }
        }

        private void SetNotGrounded()
        {
            _groundedOn = null;
            _isGroundedForWalk = false;
            _isGroundedForJump = false;
        }

        #endregion
    }
}

