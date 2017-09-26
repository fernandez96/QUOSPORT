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
    public class AlmacenBL: Singleton<AlmacenBL>, IAlmacenBL<Almacen, int>
    {
        public int Add(Almacen entity)
        {
            return AlmacenRepository.Instancia.Add(entity);
        }

        public int Delete(Almacen entity)
        {
            return AlmacenRepository.Instancia.Delete(entity);
        }

      
        public IList<Almacen> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return AlmacenRepository.Instancia.GetAllPaging(paginationParameters);
        }

        public Almacen GetById(Almacen entity)
        {
            return AlmacenRepository.Instancia.GetById(entity);
        }

        public int Update(Almacen entity)
        {
            return AlmacenRepository.Instancia.Update(entity);
        }

        public int CambioEstado(Almacen entity)
        {
            return AlmacenRepository.Instancia.CambioEstado(entity);
        }

    }
}
