using Base.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessLogic.Interfaces
{
  public  interface INotaIngresoBL<N,D,Q> where N :class
    {
        Q AddNI(N entity);
        Q UpdateNI(N entity);
        Q DeleteNI(N entity);
        N GetById(N entity);
        IList<N> GetAllPaging(PaginationParameter<Q> paginationParameters);
        IList<D> GetAll(N entity);
        Q GetCorrelativo(N entity);
    }
}
