using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System.Collections.Generic;

namespace Base.BusinessLogic
{
    public class TransportistaBL : Singleton<TransportistaBL>, ITransportistaBL<Transportista, int>
    {
        public int Add(Transportista entity)
        {
            return TransportistaRepository.Instancia.Add(entity);
        }

        public int Delete(Transportista entity)
        {
            return TransportistaRepository.Instancia.Delete(entity);
        }

        public IList<Transportista> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return TransportistaRepository.Instancia.GetAllPaging(paginationParameters);
        }

        public Transportista GetById(Transportista entity)
        {
            return TransportistaRepository.Instancia.GetById(entity);
        }

        public int Update(Transportista entity)
        {
            return TransportistaRepository.Instancia.Update(entity);
        }

        public int GetCorrelativo()
        {
            return TransportistaRepository.Instancia.GetCorrelativo();
        }
    }
}
