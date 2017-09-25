using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common.Generics;
using Base.DataAccess;
using System.Collections.Generic;

namespace Base.BusinessLogic
{
    public class ModeloBL: Singleton<ModeloBL>, IModeloBL<Modulo>
    {
        public IList<Modulo> GetAllActives()
        {
            return ModuloRepository.Instancia.GetAllActives();
        }
    }
}
