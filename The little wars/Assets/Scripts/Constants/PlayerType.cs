using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Constants
{
    [Serializable]
    public enum PlayerType
    {
        None = 0,
        LocalPlayer = 1,
        RemotePlayer = 2,
        Ai = 3
    }
}
