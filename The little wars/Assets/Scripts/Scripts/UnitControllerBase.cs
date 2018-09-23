using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    [RequireComponent(typeof(UnitMoveScript))]
    public class UnitControllerBase : MonoBehaviour
    {
        private UnitMoveScript _unitMoveScript;
        protected UnitMoveScript UnitMoveScript
        {
            get { return _unitMoveScript ?? (_unitMoveScript = GetComponent<UnitMoveScript>()); }
        }

        public bool AllowControll;

    }
}
