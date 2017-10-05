using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DataAccess.Interfaces
{
   public interface IStatusRepository<T,Q> where T :class
    {
        Q status(T entity);
    }
}
