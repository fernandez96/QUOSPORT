namespace Base.DataAccess.Interfaces
{
    public interface ILogRepository<T,Q> where T :class
    {
        Q Add(T entity);
    }
}
