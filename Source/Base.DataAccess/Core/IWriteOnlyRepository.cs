using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DataAccess.Core
{
   public interface IWriteOnlyRepository<T,Q> where T:class
    {
        Q Add(T entity);
        Q Delete(T entity);
        Q Update(T entity);
    }
}
