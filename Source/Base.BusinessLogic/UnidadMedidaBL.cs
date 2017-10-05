using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessLogic
{
   public class UnidadMedidaBL: Singleton<UnidadMedidaBL>, IUnidadMedidaBL<UnidadMedida, int>
    {
        public int Add(UnidadMedida entity)
        {
            return UnidadMedidaRepository.Instancia.Add(entity);
        }

        public int Delete(UnidadMedida entity)
        {
            return UnidadMedidaRepository.Instancia.Delete(entity);
        }

      
        public IList<UnidadMedida> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return UnidadMedidaRepository.Instancia.GetAllPaging(paginationParameters);
        }

        public IList<UnidadMedida> GetAll()
        {
            return UnidadMedidaRepository.Instancia.GetAll();
        }
        public UnidadMedida GetById(UnidadMedida entity)
        {
            return UnidadMedidaRepository.Instancia.GetById(entity);
        }

        public int Update(UnidadMedida entity)
        {
            return UnidadMedidaRepository.Instancia.Update(entity);
        }
  
    }
}
