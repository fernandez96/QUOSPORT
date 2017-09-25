using Base.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessLogic.Interfaces
{
  public  interface ITipoDocumentoBL <T,Q> where T: class 
    {
        Q AddModulo(T entity);
        Q Add(T entity);
        Q Update(T entity);
        Q Delete(T entity);
        T GetById(T entity);
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
        IList<T> GetIdModulo(T entity);
    }
}
