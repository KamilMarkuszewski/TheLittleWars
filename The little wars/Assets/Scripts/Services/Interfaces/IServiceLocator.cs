using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Services.Interfaces
{
    public interface IServiceLocator
    {
        T GetService<T>();
    }
}
