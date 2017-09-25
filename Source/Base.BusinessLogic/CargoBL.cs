using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System.Collections.Generic;

namespace Base.BusinessLogic
{
    public class CargoBL:Singleton<CargoBL>,ICargoBL<Cargo,int>
    {
        public IList<Cargo> GetAll(PaginationParameter<int> paginationParameter)
        {
            return CargoRepository.Instancia.GetAll(paginationParameter);
        }
        public IList<Cargo> GetAllPaging(int Id)
        {
            return CargoRepository.Instancia.GetAllPaging(Id);
        }
    }
}
