namespace Base.BusinessLogic.Interfaces
{
    public  interface IParametroBL<T,Q> where T :class
    {
        Q Add(T entity);
        Q Update(T entity);
        T GetAll();
    }
}
