using Base.BusinessEntity;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
namespace Base.DataAccess
{
   public class KardexRepository: Singleton<KardexRepository>, IKardexRepository<Kardex, int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

        public IList<Kardex> GetAll(Kardex entity)
        {
            List<Kardex> kardex = new List<Kardex>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGEA_KARDEX_LISTAR_X_FECHA_ALMACEN_PRODUCTO")))
            {
                _database.AddInParameter(comando, "@intProducto", DbType.Int32, entity.prdc_icod_producto);
                _database.AddInParameter(comando, "@intAlmacen", DbType.String, entity.almac_icod_almacen);
         

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        kardex.Add(new Kardex
                        {
                            kardc_fecha_movimiento = lector.IsDBNull(lector.GetOrdinal("kardc_fecha_movimiento")) ? default(DateTime) : lector.GetDateTime(lector.GetOrdinal("kardc_fecha_movimiento")),
                            almac_icod_almacen = lector.IsDBNull(lector.GetOrdinal("almac_icod_almacen")) ? default(int) : lector.GetInt32(lector.GetOrdinal("almac_icod_almacen")),
                            prdc_icod_producto = lector.IsDBNull(lector.GetOrdinal("prdc_icod_producto")) ? default(int) : lector.GetInt32(lector.GetOrdinal("prdc_icod_producto")),
                            kardc_icantidad_prod = lector.IsDBNull(lector.GetOrdinal("kardc_icantidad_prod")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("kardc_icantidad_prod")),
                            tdocc_icod_tipo_doc = lector.IsDBNull(lector.GetOrdinal("tdocc_icod_tipo_doc")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tdocc_icod_tipo_doc")),
                            kardc_numero_doc = lector.IsDBNull(lector.GetOrdinal("kardc_numero_doc")) ? default(string) : lector.GetString(lector.GetOrdinal("kardc_numero_doc")),
                            kardc_tipo_movimiento = lector.IsDBNull(lector.GetOrdinal("kardc_tipo_movimiento")) ? default(int) : lector.GetInt32(lector.GetOrdinal("kardc_tipo_movimiento")),
                            strMotivo = lector.IsDBNull(lector.GetOrdinal("strMotivo")) ? default(string) : lector.GetString(lector.GetOrdinal("strMotivo")),
                            strAlmacen = lector.IsDBNull(lector.GetOrdinal("strAlmacen")) ? default(string) : lector.GetString(lector.GetOrdinal("strAlmacen")),
                            strCodProducto = lector.IsDBNull(lector.GetOrdinal("strCodProducto")) ? default(string) : lector.GetString(lector.GetOrdinal("strCodProducto")),
                            strProducto = lector.IsDBNull(lector.GetOrdinal("strProducto")) ? default(string) : lector.GetString(lector.GetOrdinal("strProducto")),
                            strDocumento = lector.IsDBNull(lector.GetOrdinal("strDocumento")) ? default(string) : lector.GetString(lector.GetOrdinal("strDocumento")),
                            kardc_observaciones = lector.IsDBNull(lector.GetOrdinal("kardc_observaciones")) ? default(string) : lector.GetString(lector.GetOrdinal("kardc_observaciones")),
                            dblIngreso = lector.IsDBNull(lector.GetOrdinal("dblIngreso")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("dblIngreso")),
                            dblSalida = lector.IsDBNull(lector.GetOrdinal("dblSalida")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("dblSalida")),
                            dblSaldo = lector.IsDBNull(lector.GetOrdinal("dblSaldo")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("dblSaldo")),

                        });
                    }
                }
            }

            return kardex;
        }
    }
}
