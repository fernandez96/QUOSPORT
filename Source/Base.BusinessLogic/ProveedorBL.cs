using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System.Collections.Generic;

namespace Base.BusinessLogic
{
    public class ProveedorBL:Singleton<ProveedorBL>, IProveedorBL<Proveedor, int>
    {
        public int Add(Proveedor entity)
        {
            return ProveedorRepository.Instancia.Add(entity);
        }

        public int Update(Proveedor entity)
        {
            return ProveedorRepository.Instancia.Update(entity);
        }

        public int Delete(Proveedor entity)
        {
            return ProveedorRepository.Instancia.Delete(entity);
        }

        public int GetExisteProveedor(int tipo, string dni , string ruc, string nombre)
        {
            return ProveedorRepository.Instancia.GetExisteProveedor(tipo,dni,ruc,nombre);
        }


        public IList<Proveedor> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return ProveedorRepository.Instancia.GetAllPaging(paginationParameters);
        }

        public Proveedor GetById(Proveedor entity)
        {
            return ProveedorRepository.Instancia.GetById(entity);
        }

        public int GetCorrelativo()
        {
            return ProveedorRepository.Instancia.GetCorrelativo();
        }

      
    }
}
