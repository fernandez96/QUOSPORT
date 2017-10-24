using Base.Common;
using System.Collections.Generic;

namespace Base.BusinessLogic.Interfaces
{
    public  interface IStockXAlmacenBL<T,Q> where T:class
    {
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
    }
}
