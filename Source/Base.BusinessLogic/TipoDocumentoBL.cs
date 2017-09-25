using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System.Collections.Generic;

namespace Base.BusinessLogic
{
    public class TipoDocumentoBL: Singleton<TipoDocumentoBL>, ITipoDocumentoBL<TipoDocumento, int>
    {
        public int Add(TipoDocumento entity)
        {
            return TipoDocumentoRepository.Instancia.Add(entity);
        }
        public int Update(TipoDocumento entity)
        {
            return TipoDocumentoRepository.Instancia.Update(entity);
        }
        public int Delete(TipoDocumento entity)
        {
            return TipoDocumentoRepository.Instancia.Delete(entity);
        }

        public IList<TipoDocumento> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return TipoDocumentoRepository.Instancia.GetAllPaging(paginationParameters);
        }
        public TipoDocumento GetById(TipoDocumento entity)
        {
            return TipoDocumentoRepository.Instancia.GetById(entity);
        }
        public int AddModulo(TipoDocumento entity)
        {
            return TipoDocumentoRepository.Instancia.AddModulo(entity);
        }
        public IList<TipoDocumento> GetIdModulo(TipoDocumento entity)
        {
            return TipoDocumentoRepository.Instancia.GetIdModulo(entity);
        }
    }
}
