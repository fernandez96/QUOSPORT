using Base.Common;
using System.Collections.Generic;

namespace Base.DataAccess.Interfaces
{
    public interface ITablaRegistroRepository<T,Q> where T:class
    {
        Q Add(T entity);
        Q Update(T entity);
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
        T GetById(T entity);
        T GetCorrelativaCab();

        Q AddDetalle(T entity);
        Q UpdateDetalle(T entity);
        IList<T> GetAllPagingDetalle(PaginationParameter<Q> paginationParameters);
        T GetByIdDetalle(T entity);
        Q DeleteDetalle(T entity);
        T GetCorrelativaDet(T entity);

        IList<T> GetAll(Q idtable);


    }
}
