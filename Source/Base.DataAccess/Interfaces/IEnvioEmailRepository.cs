using Base.BusinessEntity;

namespace Base.DataAccess.Interfaces
{
    public interface IEnvioEmailRepository<T> where T:class
    {
        long Add(string url);
    }
}
