using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Services.Interfaces
{
    public interface IService
    {
        ServiceStatus Status { get; }

        void Initialize();
    }
}
