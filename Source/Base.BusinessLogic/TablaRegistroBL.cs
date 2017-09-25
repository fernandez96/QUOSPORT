using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System.Collections.Generic;

namespace Base.BusinessLogic
{
    public  class TablaRegistroBL: Singleton<TablaRegistroBL>, ITablaRegistroBL<TablaRegistro, int>
    {
        public int Add(TablaRegistro entity)
        {
            return TablaRegistroRepository.Instancia.Add(entity);
        }

        public IList<TablaRegistro> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return TablaRegistroRepository.Instancia.GetAllPaging(paginationParameters);
        }

        public TablaRegistro GetById(TablaRegistro entity)
        {
            return TablaRegistroRepository.Instancia.GetById(entity);
        }

        public TablaRegistro GetCorrelativaCab()
        {
            return TablaRegistroRepository.Instancia.GetCorrelativaCab();
        }
        public int Update(TablaRegistro entity)
        {
            return TablaRegistroRepository.Instancia.Update(entity);
        }

        public int AddDetalle(TablaRegistro entity)
        {
            return TablaRegistroRepository.Instancia.AddDetalle(entity);
        }

        public IList<TablaRegistro> GetAllPagingDetalle(PaginationParameter<int> paginationParameters)
        {
            return TablaRegistroRepository.Instancia.GetAllPagingDetalle(paginationParameters);
        }

        public TablaRegistro GetByIdDetalle(TablaRegistro entity)
        {
            return TablaRegistroRepository.Instancia.GetByIdDetalle(entity);
        }

        public int UpdateDetalle(TablaRegistro entity)
        {
            return TablaRegistroRepository.Instancia.UpdateDetalle(entity);
        }
        public int DeleteDetalle(TablaRegistro entity)
        {
            return TablaRegistroRepository.Instancia.DeleteDetalle(entity);
        }

        public TablaRegistro GetCorrelativaDet(TablaRegistro entity)
        {
            return TablaRegistroRepository.Instancia.GetCorrelativaDet(entity);
        }
        public IList<TablaRegistro> GetAll( int idtable)
        {
            return TablaRegistroRepository.Instancia.GetAll(idtable);
        }

    }
}
