using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common.Generics;
using Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessLogic
{
   public class RolBL:Singleton<RolBL>,IRolBL<Rol>
    {
        public IList<Rol> GetAllActives()
        {
            return RolRepository.Instancia.GetAllActives();
        }
    }
}
