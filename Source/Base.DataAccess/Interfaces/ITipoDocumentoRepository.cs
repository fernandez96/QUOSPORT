using Base.Common;
using System.Collections.Generic;

namespace Base.DataAccess.Interfaces
{
    public interface ITipoDocumentoRepository<T,Q> where T: class
    {
        Q AddModulo(T entity);
        Q Add(T entity);
        Q Update(T entity);
        Q Delete(T entity);
        T GetById(T entity);
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
        IList<T> GetIdModulo(T entity);
    }
}
