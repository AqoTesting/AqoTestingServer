using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Services
{
    public abstract class ServiceBase : IDisposable
    {
        public ServiceBase()
        {
        
        }

        public virtual void Dispose()
        {

        }
    }
}
