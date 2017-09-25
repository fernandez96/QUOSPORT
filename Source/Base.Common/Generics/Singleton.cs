using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Base.Common.Generics
{
   public abstract class Singleton<T> where T: class, new()
    {
        private static T _Instancia;
        private static readonly Mutex Mutex = new Mutex();

        public static T Instancia
        {
            get
            {
                Mutex.WaitOne();
                if (_Instancia == null) _Instancia = new T();
                Mutex.ReleaseMutex();
                return _Instancia;
              
            }
        }  
    }
}
