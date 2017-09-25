using Base.BusinessLogic.Core;

namespace Base.BusinessLogic.Interfaces
{
    public interface IUsuarioBL<T,Q> : ILogic<T,Q> where T : class
    {
        bool Exists(T entity);
        T GetByUsername(string username, string pass);
    }
}
