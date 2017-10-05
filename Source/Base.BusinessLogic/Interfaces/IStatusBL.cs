using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessLogic.Interfaces
{
    public interface IStatusBL<T,Q> where T:class
    {
        Q status(T entity);
    }
}
