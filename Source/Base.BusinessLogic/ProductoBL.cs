using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System;
using System.Collections.Generic;

namespace Base.BusinessLogic
{
    public class ProductoBL : Singleton<ProductoBL>,IProductoBL<Producto, int>
    {
        public int Add(Producto entity)
        {
            return ProductoRepository.Instancia.Add(entity);
        }
        public int Delete(Producto entity)
        {
            return ProductoRepository.Instancia.Delete(entity);
        }
        public IList<Producto> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return ProductoRepository.Instancia.GetAllPaging(paginationParameters);
        }
        public Producto GetById(Producto entity)
        {
            return ProductoRepository.Instancia.GetById(entity);
        }
        public int Update(Producto entity)
        {
            return ProductoRepository.Instancia.Update(entity);
        }

        public IList<Producto> GetAllPagingStock(PaginationParameter<int> paginationParameters)
        {
            return ProductoRepository.Instancia.GetAllPagingStock(paginationParameters);
        }

        public decimal GetStockProducto(int idproducto)
        {
            return ProductoRepository.Instancia.GetStockProducto(idproducto);
        }
    }
}
