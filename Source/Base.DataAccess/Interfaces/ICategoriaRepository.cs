using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DataAccess.Interfaces
{
   public interface ICategoriaRepository<T,Q> where T: class
    {
        Q Add(T entity);
    }
}
