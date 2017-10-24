using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess;
using System.Collections.Generic;
namespace Base.BusinessLogic
{
    public class StockXAlmacenBL: Singleton<StockXAlmacenBL>, IStockXAlmacenBL<StockXAlmacen, int>
    {
        public IList<StockXAlmacen> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            return StockXAlmacenRepository.Instancia.GetAllPaging(paginationParameters);
        }
    }
}
