using Base.BusinessEntity;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Base.DataAccess
{
    public class UnidadMedidaRepository: Singleton<UnidadMedidaRepository>, IUnidadMedidaRepository<UnidadMedida, int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

        #region Métodos Públicos

        public int Add(UnidadMedida entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_UNIDAD_MEDIDA_INSERT")))
            {
                _database.AddInParameter(comando, "@umec_vcod_unidad_medida", DbType.String, entity.umec_vcod_unidad_medida);
                _database.AddInParameter(comando, "@umec_vdescripcion", DbType.String, entity.umec_vdescripcion);
                _database.AddInParameter(comando, "@umec_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                _database.AddInParameter(comando, "@umec_bflag_estado", DbType.Int32, 1);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }

        public int Delete(UnidadMedida entity)
        {
            int idResult;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_UNIDAD_MEDIDA_DELETE")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                _database.ExecuteNonQuery(comando);
                idResult = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return idResult;
        }

        public IList<UnidadMedida> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            List<UnidadMedida> unidad = new List<UnidadMedida>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_UNIDAD_MEDIDA_GetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.WhereFilter) ? string.Empty : paginationParameters.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.OrderBy) ? string.Empty : paginationParameters.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.Int32, paginationParameters.Start);
                _database.AddInParameter(comando, "@Rows", DbType.Int32, paginationParameters.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        unidad.Add(new UnidadMedida
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            umec_vcod_unidad_medida = lector.IsDBNull(lector.GetOrdinal("umec_vcod_unidad_medida")) ? default(string) : lector.GetString(lector.GetOrdinal("umec_vcod_unidad_medida")),
                            umec_vdescripcion = lector.IsDBNull(lector.GetOrdinal("umec_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("umec_vdescripcion")),                     
                            Estado = lector.IsDBNull(lector.GetOrdinal("umec_bflag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("umec_bflag_estado")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return unidad;
        }

        public  UnidadMedida GetById(UnidadMedida entity)
        {
            UnidadMedida unidad = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_UNIDAD_MEDIDA_GetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        unidad = new UnidadMedida
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("umec_iid_unidad_medida")) ? default(int) : lector.GetInt32(lector.GetOrdinal("umec_iid_unidad_medida")),
                            umec_vcod_unidad_medida = lector.IsDBNull(lector.GetOrdinal("umec_vcod_unidad_medida")) ? default(string) : lector.GetString(lector.GetOrdinal("umec_vcod_unidad_medida")),
                            umec_vdescripcion = lector.IsDBNull(lector.GetOrdinal("umec_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("umec_vdescripcion")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("umec_bflag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("umec_bflag_estado"))
                        };
                    }
                }
            }

            return unidad;
        }

        public int Update(UnidadMedida entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_UNIDAD_MEDIDA_UPDATE")))
            {
                _database.AddInParameter(comando, "@umec_vdescripcion", DbType.String, entity.umec_vdescripcion);
                _database.AddInParameter(comando, "@umec_vusuario_modifica", DbType.String, entity.UsuarioModificacion);
                _database.AddInParameter(comando, "@id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }

        public int CambioEstado(UnidadMedida entity)
        {
            int id;
            using (var comando=_database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName,"")))
            {

                id = 1;
            }
            return id;
        }


        #endregion
    }
}
