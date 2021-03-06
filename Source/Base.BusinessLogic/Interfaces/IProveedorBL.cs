﻿using Base.Common;
using System.Collections.Generic;

namespace Base.BusinessLogic.Interfaces
{
    public interface IProveedorBL<T,Q> where T: class
    {
        Q Add(T entity);
        Q Update(T entity);
        Q Delete(T entity);
        T GetById(T entity);
        Q GetCorrelativo();
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
        Q GetExisteProveedor(int type, string dni, string ruc, string name);
    }
}
