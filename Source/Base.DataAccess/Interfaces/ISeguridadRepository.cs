using System.Collections.Generic;

namespace Base.DataAccess.Interfaces
{
    public interface ISeguridadRepository<T> where T :class
    {
        IList<T> GetFormulario();
    }
}
