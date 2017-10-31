using Base.Common;
using System.Collections.Generic;

namespace Base.DataAccess.Interfaces
{
    public interface IProductoRepository<T,Q> where T:class
    {
        Q Add(T entity);
        Q Update(T entity);
        Q Delete(T entity);
        T GetById(T entity);
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
        IList<T> GetAllPagingStock(PaginationParameter<Q> paginationParameters);
        decimal GetStockProducto(Q idproducto);


    }
}
