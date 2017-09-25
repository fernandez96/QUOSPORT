using Base.Common;
using System.Collections.Generic;

namespace Base.DataAccess.Interfaces
{
    public interface ICargoRepository<T,Q> where T:class
    {
        IList<T> GetAll(PaginationParameter<Q> paginationParameter);
        IList<T> GetAllPaging(Q Id);
    }
}
