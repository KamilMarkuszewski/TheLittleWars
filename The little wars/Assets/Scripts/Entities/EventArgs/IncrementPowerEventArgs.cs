using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Entities.EventArgs
{
    public class IncrementPowerEventArgs : System.EventArgs
    {
        public int CurrentPower;

        public IncrementPowerEventArgs(int currentPower)
        {
            CurrentPower = currentPower;
        }
    }
}
