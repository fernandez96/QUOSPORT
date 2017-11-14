﻿using Base.Common;
using System.Collections.Generic;

namespace Base.BusinessLogic.Interfaces
{
    public interface INotaSalidaBL<N,D,Q> where N: class
    {
        Q AddNS(N entity);
        Q UpdateNS(N entity);
        Q DeleteNS(N entity);
        N GetById(N entity);
        IList<N> GetAllPaging(PaginationParameter<Q> paginationParameters);
        IList<D> GetAll(N entity);
    }
}
