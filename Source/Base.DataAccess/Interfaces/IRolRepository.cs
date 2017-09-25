using System.Collections.Generic;

namespace Base.DataAccess.Interfaces
{
    public interface IRolRepository<T> where T:class
    {
        IList<T> GetAllActives();
    }
}
