using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class PlayerColorsHelper
    {
        private readonly Stack<Color> _colors;

        private readonly Color _orange = new Color(0.9f, 0.4f, 0f);
        private readonly Color _brown = new Color(0.5f, 0.4f, 0f);

        public PlayerColorsHelper()
        {
            _colors = new Stack<Color>();

            _colors.Push(_brown);
            _colors.Push(_orange);
            _colors.Push(Color.magenta);
            _colors.Push(Color.cyan);
            _colors.Push(Color.yellow);
            _colors.Push(Color.blue);
            _colors.Push(Color.red);
            _colors.Push(Color.green);
        }

        public Color GetNextColor()
        {
            return _colors.Pop();
        }

        public void PutColorBack(Color color)
        {
            _colors.Push(color);
        }
    }
}
