﻿using Base.Common;
using System.Collections.Generic;

namespace Base.BusinessLogic.Interfaces
{
    public interface IUnidadMedidaBL<T,Q> where T:class
    {
        Q Add(T entity);
        Q Update(T entity);
        Q Delete(T entity);
        T GetById(T entity);
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
        IList<T> GetAll();
    }
}
