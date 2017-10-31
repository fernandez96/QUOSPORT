using Base.BusinessEntity;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using System.Data;

namespace Base.DataAccess
{
    public class StockXAlmacenRepository: Singleton<StockXAlmacenRepository>, IStockXAlmacenRepository<StockXAlmacen, int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

        public IList<StockXAlmacen> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            List<StockXAlmacen> stockXAlmacen = new List<StockXAlmacen>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_STOCKXALMACEN_GetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.WhereFilter) ? string.Empty : paginationParameters.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.OrderBy) ? string.Empty : paginationParameters.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.Int32, paginationParameters.Start);
                _database.AddInParameter(comando, "@Rows", DbType.Int32, paginationParameters.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        stockXAlmacen.Add(new StockXAlmacen
                        {
                            stockactual = lector.IsDBNull(lector.GetOrdinal("stockactual")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("stockactual")),
                            prdc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("prdc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("prdc_vdescripcion")),
                            umec_vdescripcion = lector.IsDBNull(lector.GetOrdinal("umec_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("umec_vdescripcion")),
                            almac_vdescripcion = lector.IsDBNull(lector.GetOrdinal("almac_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vdescripcion")),
                            almac_iid_almacen = lector.IsDBNull(lector.GetOrdinal("almac_iid_almacen")) ? default(int) : lector.GetInt32(lector.GetOrdinal("almac_iid_almacen")),
                            prdc_iid_producto = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return stockXAlmacen;
        }
    }
}
