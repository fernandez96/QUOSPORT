using System.Collections.Generic;

namespace Base.BusinessLogic.Interfaces
{
    public interface IRolBL<T> where T: class
    {
        IList<T> GetAllActives();
    }
}
