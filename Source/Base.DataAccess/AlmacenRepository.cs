using Base.BusinessEntity;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Base.DataAccess
{
    public class AlmacenRepository: Singleton<AlmacenRepository>, IAlmacenRepository<Almacen, int>
    {

        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

        #region Métodos Públicos

        public int Add(Almacen entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_ALMACEN_INSERT")))
            {
                _database.AddInParameter(comando, "@almac_vcod_almacen", DbType.String, entity.almac_vcod_almacen);
                _database.AddInParameter(comando, "@almac_vdescripcion", DbType.String, entity.almac_vdescripcion);
                _database.AddInParameter(comando, "@almac_vubicacion", DbType.String, entity.almac_vubicacion);
                _database.AddInParameter(comando, "@almac_vresponsable", DbType.String, entity.almac_vresponsable);
                _database.AddInParameter(comando, "@almac_vtelefono", DbType.String, entity.almac_vtelefono);
                _database.AddInParameter(comando, "@almac_vcorreo", DbType.String, entity.almac_vcorreo);
                _database.AddInParameter(comando, "@almac_itipo", DbType.Int32, entity.almac_itipo);
                _database.AddInParameter(comando, "@almac_bflag_estado", DbType.Int32,  entity.Estado);
                _database.AddInParameter(comando, "@almac_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                _database.AddInParameter(comando, "@almac_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }

        public int Delete(Almacen entity)
        {
            int idResult;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_ALMACEN_DELETE")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                _database.ExecuteNonQuery(comando);
                idResult = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return idResult;
        }

        public IList<Almacen> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            List<Almacen> almances = new List<Almacen>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_ALMACEN_GetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.WhereFilter) ? string.Empty : paginationParameters.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.OrderBy) ? string.Empty : paginationParameters.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.Int32, paginationParameters.Start);
                _database.AddInParameter(comando, "@Rows", DbType.Int32, paginationParameters.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        almances.Add(new Almacen
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            almac_vcod_almacen = lector.IsDBNull(lector.GetOrdinal("almac_vcod_almacen")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vcod_almacen")),
                            almac_vdescripcion = lector.IsDBNull(lector.GetOrdinal("almac_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vdescripcion")),
                            almac_vubicacion = lector.IsDBNull(lector.GetOrdinal("almac_vubicacion")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vubicacion")),
                            almac_vtelefono = lector.IsDBNull(lector.GetOrdinal("almac_vtelefono")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vtelefono")),
                            almac_vresponsable = lector.IsDBNull(lector.GetOrdinal("almac_vresponsable")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vresponsable")),
                            almac_vtipo = lector.IsDBNull(lector.GetOrdinal("almac_vtipo")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vtipo")),
                            almac_itipo = lector.IsDBNull(lector.GetOrdinal("almac_itipo")) ? default(int) : lector.GetInt32(lector.GetOrdinal("almac_itipo")),
                            almac_vcorreo = lector.IsDBNull(lector.GetOrdinal("almac_vcorreo")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vcorreo")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("almac_bflag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("almac_bflag_estado")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return almances;
        }

        public Almacen GetById(Almacen entity)
        {
            Almacen almacen = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_ALMACEN_GetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        almacen = new Almacen
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            almac_vcod_almacen = lector.IsDBNull(lector.GetOrdinal("almac_vcod_almacen")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vcod_almacen")),
                            almac_vdescripcion = lector.IsDBNull(lector.GetOrdinal("almac_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vdescripcion")),
                            almac_vubicacion = lector.IsDBNull(lector.GetOrdinal("almac_vubicacion")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vubicacion")),
                            almac_vresponsable = lector.IsDBNull(lector.GetOrdinal("almac_vresponsable")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vresponsable")),
                            almac_vtelefono = lector.IsDBNull(lector.GetOrdinal("almac_vtelefono")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vtelefono")),
                            almac_vcorreo = lector.IsDBNull(lector.GetOrdinal("almac_vcorreo")) ? default(string) : lector.GetString(lector.GetOrdinal("almac_vcorreo")),
                            almac_itipo = lector.IsDBNull(lector.GetOrdinal("almac_itipo")) ? default(int) : lector.GetInt32(lector.GetOrdinal("almac_itipo")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("almac_bflag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("almac_bflag_estado"))
                        };
                    }
                }
            }

            return almacen;
        }

        public int Update(Almacen entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_ALMACEN_UPDATE")))
            {

                _database.AddInParameter(comando, "@almac_vdescripcion", DbType.String, entity.almac_vdescripcion);
                _database.AddInParameter(comando, "@almac_vubicacion", DbType.String, entity.almac_vubicacion);
                _database.AddInParameter(comando, "@almac_vresponsable", DbType.String, entity.almac_vresponsable);
                _database.AddInParameter(comando, "@almac_vtelefono", DbType.String, entity.almac_vtelefono);
                _database.AddInParameter(comando, "@almac_vcorreo", DbType.String, entity.almac_vcorreo);
                _database.AddInParameter(comando, "@almac_itipo", DbType.Int32, entity.almac_itipo);
                _database.AddInParameter(comando, "@almac_vusuario_modificado", DbType.String, entity.UsuarioModificacion);
                _database.AddInParameter(comando, "@almac_vpc_modificado", DbType.String, WindowsIdentity.GetCurrent().Name); ;
                _database.AddInParameter(comando, "@id", DbType.Int32, entity.Id);
                _database.AddInParameter(comando, "@almac_bflag_estado", DbType.Int32, entity.Estado);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }

        public int CambioEstado(Almacen entity)
        {
            int estado;
            using (var comando= _database.GetStoredProcCommand(string.Format("{0}{1}",ConectionStringRepository.EsquemaName,"")))
            {

                estado = 1;
            }
            return estado;
        }

        #endregion
    }
}
