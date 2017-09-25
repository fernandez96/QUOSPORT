using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System;
using System.Collections.Generic;

namespace Base.BusinessLogic
{
    public  class EnvioEmailBL: Singleton<EnvioEmailBL>, IEnvioEmailBL<EnvioEmail>
    {
        public long Add(string entity)
        {
            return EnvioEmailRepository.Instancia.Add(entity);
        }
    }
}
