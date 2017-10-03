﻿using Base.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessLogic.Interfaces
{
   public interface ICategoriaBL<T, L, S, Q> where T: class
    {
        Q Add(T entity);
        Q Update(T entity);
        //Q Delete(T entity);
        //T GetByIdLinea(T entity);
        //T GetByIdSubLinea(T entity);
        T GetById(T entity);
        IList<L> GetAllLinea(L entity);
        IList<S> GetAllSubLinea(S entity);
        IList<T> GetAllPaging(PaginationParameter<Q> paginationParameters);
    }
}
