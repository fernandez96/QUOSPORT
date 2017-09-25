using Base.Common;
using System.Collections.Generic;

namespace Base.BusinessLogic.Core
{
    public interface IReadOnlyLogic<T,Q> where T:class
    {
        IList<T> GetAll(string whereFilters);
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
        T GetById(T entity);
    }
}
