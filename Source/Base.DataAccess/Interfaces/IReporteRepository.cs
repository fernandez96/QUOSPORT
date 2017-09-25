using Base.Common;
using System.Collections.Generic;

namespace Base.DataAccess.Interfaces
{
    public interface IReporteRepository<T,Q> where T:class
    {
        IList<T> VentaGetAllFilter(PaginationParameter<Q> paginationParameter);
        IList<T> VentaGetAllReport(PaginationParameter<Q> paginationParameter);
    }
}
