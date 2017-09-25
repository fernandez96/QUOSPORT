namespace Base.DataAccess.Interfaces
{
    public interface IParametroRepository<T,Q> where T : class
    {
        Q Add(T entity);
        Q Update(T entity);
        T GetAll();
    }
}
