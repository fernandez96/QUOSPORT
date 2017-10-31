using System.Collections.Generic;

namespace Base.DataAccess.Interfaces
{
    public interface IKardexRepository<T,Q> where T:class
    {
        IList<T> GetAll(T entity);
    }
}
