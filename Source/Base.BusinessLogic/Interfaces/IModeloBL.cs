using System.Collections.Generic;

namespace Base.BusinessLogic.Interfaces
{
    public interface IModeloBL<T> where T:class
    {
        IList<T> GetAllActives();
    }
}
