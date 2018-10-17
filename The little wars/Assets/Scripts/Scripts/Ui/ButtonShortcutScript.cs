using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.Scripts.Ui
{
    [RequireComponent(typeof(Button))]
    public class ButtonShortcutScript : MonoBehaviour
    {
        public KeyCode ShortcutKey;

        private Button _buttonComponent;
        private Button ButtonComponent
        {
            get { return _buttonComponent ?? (_buttonComponent = GetComponent<Button>()); }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(ShortcutKey))
            {
                ButtonComponent.onClick.Invoke();
            }
        }
    }
}
