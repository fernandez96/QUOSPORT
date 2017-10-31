using System.Collections.Generic;

namespace Base.BusinessLogic.Interfaces
{
    public interface IKardexBL<T,Q> where T:class
    {
        IList<T> GetAll(T entity);
    }
}
