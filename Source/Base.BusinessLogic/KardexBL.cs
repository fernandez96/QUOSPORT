using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System;
using System.Collections.Generic;


namespace Base.BusinessLogic
{
   public class KardexBL: Singleton<KardexBL>, IKardexBL<Kardex, int>
    {
        public IList<Kardex> GetAll(Kardex entity)
        {
            return KardexRepository.Instancia.GetAll(entity);
        }
    }
}
