using Base.Common;
using System.Collections.Generic;

namespace Base.DataAccess.Interfaces
{
    public interface IStockXAlmacenRepository<T,Q> where T:class
    {
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
    }
}
