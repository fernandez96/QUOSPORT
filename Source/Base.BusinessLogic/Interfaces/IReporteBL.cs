using Base.Common;
using System.Collections.Generic;

namespace Base.BusinessLogic.Interfaces
{
    public interface IReporteBL<T,Q> where T:class
    {
        IList<T> VentaGetAllFilter(PaginationParameter<Q> paginationParameter);
        IList<T> VentaGetAllReport(PaginationParameter<Q> paginationParameter);
    }
}
