using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System.Collections.Generic;
namespace Base.BusinessLogic
{
    public class TransferenciaBL : Singleton<TransferenciaBL>, ITransferenciaBL<Transferencia, TransferenciaDetalle, int>
    {
        public int Add(Transferencia entity)
        {
            return TransferenciaRepository.Instancia.Add(entity);
        }

        public int Delete(Transferencia entity)
        {
            return TransferenciaRepository.Instancia.Delete(entity);
        }


        public IList<Transferencia> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return TransferenciaRepository.Instancia.GetAllPaging(paginationParameters);
        }

        public Transferencia GetById(Transferencia entity)
        {
            return TransferenciaRepository.Instancia.GetById(entity);
        }

        public int Update(Transferencia entity)
        {
            return TransferenciaRepository.Instancia.Update(entity);
        }
        public IList<TransferenciaDetalle> GetAll(Transferencia entity)
        {
            return TransferenciaRepository.Instancia.GetAll(entity);
        }
        public int GetCorrelativo()
        {
            return TransferenciaRepository.Instancia.GetCorrelativo();
        }
    }
}
