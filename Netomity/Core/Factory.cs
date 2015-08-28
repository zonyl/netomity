using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Netomity.Core
{
    public class NFactory
    {
        UnityContainer _uc = new UnityContainer();

        public static NFactory Create()
        {
            return new NFactory();
        }

        public T Create<T>(params object[] objs)
        {
            var a = _uc.Resolve<T>();
           return default(T);
        }
    }
}
