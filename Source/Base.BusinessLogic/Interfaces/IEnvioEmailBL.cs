using Base.BusinessEntity;

namespace Base.BusinessLogic.Interfaces
{
    public interface IEnvioEmailBL<T> where T:class
    {
        long Add(string envioEmail);
    }
}
