using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    public class HumanUnitController : UnitControllerBase
    {
        // Update is called once per frame
        void Update()
        {
            if (!AllowControll)
            {
                return;
            }

            var xUnitMovement = Input.GetAxis("Horizontal");
            var yScopeMovement = Input.GetAxis("Vertical");
            var isShooting = Input.GetButton("Fire2");
            var isJumping = Input.GetButton("Jump");

            UnitMoveScript.ControllCharacter(xUnitMovement, yScopeMovement, isShooting, isJumping);
        }
    }
}
