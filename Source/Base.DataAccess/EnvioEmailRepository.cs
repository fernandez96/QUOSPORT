using Base.BusinessEntity;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Base.DataAccess
{
   public class EnvioEmailRepository:Singleton<EnvioEmailRepository>, IEnvioEmailRepository<EnvioEmail>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

        public long Add(string url)
        {
            long id = 0;
            PropertyInfo[] listaPropiedadesProceso = (new EnvioEmail()).GetType().GetProperties();
            using (var conexionBulkCopy = new SqlConnection(_database.ConnectionString))
            {
                

                EnvioEmail objeto = new EnvioEmail();
                DataTable tablaDatos = new DataTable();
                DataRow filaDatos = tablaDatos.NewRow();

                listaPropiedadesProceso = objeto.GetType().GetProperties();
                

                foreach (PropertyInfo propiedadProceso in listaPropiedadesProceso)
                {
                    if (propiedadProceso.PropertyType == typeof(string))
                    {
                        filaDatos[propiedadProceso.Name] = ((string)propiedadProceso.GetValue(objeto, null) ?? "").Length > 250 ? "" : ((string)propiedadProceso.GetValue(objeto, null) ?? "");
                    }
                    else
                    {
                        filaDatos[propiedadProceso.Name] = propiedadProceso.GetValue(objeto, null);
                    }
                }

                tablaDatos.Rows.Add(filaDatos);

                //bulkCopy
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conexionBulkCopy))
                {

                    bulkCopy.BulkCopyTimeout = int.MaxValue;
                    bulkCopy.DestinationTableName = "Base.ClienteCorreo";

                    listaPropiedadesProceso = (new EnvioEmail()).GetType().GetProperties();
                    foreach (PropertyInfo propiedadProceso in listaPropiedadesProceso)
                    {
                        bulkCopy.ColumnMappings.Add(propiedadProceso.Name, propiedadProceso.Name);
                    }
                    //Carga masiva
                    bulkCopy.WriteToServer(tablaDatos);
                }     
            }
            return id;
        }
    }
}
