using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class PlayerColorsHelper
    {
        private readonly Queue<Color> _colors;

        private readonly Color _orange = new Color(0.9f, 0.4f, 0f);
        private readonly Color _brown = new Color(0.5f, 0.4f, 0f);

        public PlayerColorsHelper()
        {
            _colors = new Queue<Color>();

            _colors.Enqueue(Color.green);
            _colors.Enqueue(Color.red);
            _colors.Enqueue(Color.blue);
            _colors.Enqueue(Color.yellow);
            _colors.Enqueue(Color.cyan);
            _colors.Enqueue(Color.magenta);
            _colors.Enqueue(_orange);
            _colors.Enqueue(_brown);
        }

        public Color GetNextColor()
        {
            return _colors.Dequeue();
        }
    }
}
