using Base.Common;
using System.Collections.Generic;

namespace Base.BusinessLogic.Interfaces
{
    public interface ICargoBL<T,Q> where T:class
    {
        IList<T> GetAll(PaginationParameter<Q> paginationParameter);
        IList<T> GetAllPaging(Q Id);
    }
}
