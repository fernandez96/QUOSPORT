using Base.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DataAccess.Core
{
  public  interface IRepository<T,Q> where T:class
    {
        Q Add(T entity);
        Q Delete(T entity);
        IList<T> GetAll(string whereFilters);
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
        T GetByIdGetById(T entity);
        Q Update(T entity);
    }
}
