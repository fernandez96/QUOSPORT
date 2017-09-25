using Base.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessLogic.Core
{
   public interface ILogic<T,Q> where T:class
    {
        Q Add(T entity);
        Q Delete(T entity);
        IList<T> GetAll(string whereFilters);
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
        T GetById(T entity);
        Q Update(T entity);
    }
}
