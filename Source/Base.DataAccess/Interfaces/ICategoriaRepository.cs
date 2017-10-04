using Base.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DataAccess.Interfaces
{
   public interface ICategoriaRepository<T,L,S,Q> where T: class
    {
        Q Add(T entity);
        Q Update(T entity);
        T GetById(T entity);
        IList<T> GetAll();
        IList<L> GetAllLinea(L entity);
        IList<L> AllLinea();
        IList<S> AllSubLinea();
        IList<S> GetAllSubLinea(S entity);
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
    }
}
