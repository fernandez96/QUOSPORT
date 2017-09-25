using System.Collections.Generic;

namespace Base.BusinessLogic.Interfaces
{
    public interface ISeguridadBL<T> where T:class
    {
        IList<T> GetFormulario();
    }
}
