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
    public class ReporteRepository:Singleton<ReporteRepository>, IReporteRepository<Reporte,int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion
        public IList<Reporte> VentaGetAllFilter(PaginationParameter<int> paginationParameter)
         {
            List<Reporte> reporte = new List<Reporte>();
            using (var comando=_database.GetStoredProcCommand(string.Format("{0}{1}",ConectionStringRepository.EsquemaName, "VentaGetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, paginationParameter.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, paginationParameter.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.String, paginationParameter.Start);
                _database.AddInParameter(comando, "@Rows", DbType.String, paginationParameter.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector!=null)
                    {
                        while (lector.Read())
                        {
                            reporte.Add(new Reporte
                            {
                                 Id=lector.IsDBNull(lector.GetOrdinal("Id"))?default(int):lector.GetInt32(lector.GetOrdinal("Id")),
                                Anio = lector.IsDBNull(lector.GetOrdinal("Anio")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Anio")),
                                Mes = lector.IsDBNull(lector.GetOrdinal("Mes")) ? default(string) : lector.GetString(lector.GetOrdinal("Mes")),
                                Cliente = lector.IsDBNull(lector.GetOrdinal("Cliente")) ? default(string) : lector.GetString(lector.GetOrdinal("Cliente")),
                                Monto = lector.IsDBNull(lector.GetOrdinal("Monto")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("Monto")),
                                Cantidad = lector.IsDBNull(lector.GetOrdinal("CantidadVenta")) ? default(int) : lector.GetInt32(lector.GetOrdinal("CantidadVenta")),
                                CantidadRows = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad")),
                            });
                        }
                    }
                }
            }

            return reporte;
         }
        public IList<Reporte> VentaGetAllReport(PaginationParameter<int> paginationParameter)
        {
            List<Reporte> reporte = new List<Reporte>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "VentaGetAllReport")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, paginationParameter.WhereFilter);
     

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector != null)
                    {
                        while (lector.Read())
                        {
                            reporte.Add(new Reporte
                            {
                                Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                                Anio = lector.IsDBNull(lector.GetOrdinal("Anio")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Anio")),
                                Mes = lector.IsDBNull(lector.GetOrdinal("Mes")) ? default(string) : lector.GetString(lector.GetOrdinal("Mes")),
                                Cliente = lector.IsDBNull(lector.GetOrdinal("Cliente")) ? default(string) : lector.GetString(lector.GetOrdinal("Cliente")),
                                Monto = lector.IsDBNull(lector.GetOrdinal("Monto")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("Monto")),
                                Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad")),

                            });
                        }
                    }
                }
            }
            return reporte;
        }
    }
}
