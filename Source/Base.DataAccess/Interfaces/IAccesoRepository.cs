using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DataAccess.Interfaces
{
    public interface IAccesoRepository<T> where T:class
    {
        IList<T> GetFormulario();
    }
}
