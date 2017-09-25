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
    public class TipoDocumentoRepository : Singleton<TipoDocumentoRepository>, ITipoDocumentoRepository<TipoDocumento, int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion
        #region Métodos Públicos

        public int Add(TipoDocumento entity)
        {
            int idresult;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TIPO_DOCUMENTO_INSERT")))
            {
                _database.AddInParameter(comando, "@tdocc_vabreviatura_tipo_doc", DbType.String, entity.tdocc_vabreviatura_tipo_doc);
                _database.AddInParameter(comando, "@tdocc_vdescripcion", DbType.String, entity.tdocc_vdescripcion);
                _database.AddInParameter(comando, "@tdocc_flag_estado", DbType.Int32, 1);
                _database.AddInParameter(comando, "@tdocc_iusuario_crea", DbType.String, entity.UsuarioCreacion);
                _database.AddInParameter(comando, "@tdocc_pc_creacion", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                idresult = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }
            return idresult;
        }

        public int Update(TipoDocumento entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TIPO_DOCUMENTO_UPDATE")))
            {
                _database.AddInParameter(comando, "@tdocc_vdescripcion", DbType.String, entity.tdocc_vdescripcion);
                _database.AddInParameter(comando, "@tdocc_iusuario_modificado", DbType.String, entity.UsuarioModificacion);
                _database.AddInParameter(comando, "@tdocc_pc_modificado", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "@id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }
        public int Delete(TipoDocumento entity)
        {
            int idResult;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TIPO_DOCUMENTO_DELETE")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                _database.ExecuteNonQuery(comando);
                idResult = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return idResult;
        }
        public TipoDocumento GetById(TipoDocumento entity)
        {
            TipoDocumento tipodocumento = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TIPO_DOCUMENTO_GetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        tipodocumento = new TipoDocumento
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("tdocc_icod_tipo_doc")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tdocc_icod_tipo_doc")),
                            tdocc_vabreviatura_tipo_doc = lector.IsDBNull(lector.GetOrdinal("tdocc_vabreviatura_tipo_doc")) ? default(string) : lector.GetString(lector.GetOrdinal("tdocc_vabreviatura_tipo_doc")),
                            tdocc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("tdocc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("tdocc_vdescripcion")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("tdocc_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tdocc_flag_estado"))
                        };
                    }
                }
            }

            return tipodocumento;
        }
        public IList<TipoDocumento> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            List<TipoDocumento> tipodocumento = new List<TipoDocumento>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TIPO_DOCUMENTO_GetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.WhereFilter) ? string.Empty : paginationParameters.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.OrderBy) ? string.Empty : paginationParameters.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.Int32, paginationParameters.Start);
                _database.AddInParameter(comando, "@Rows", DbType.Int32, paginationParameters.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        tipodocumento.Add(new TipoDocumento
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            tdocc_vabreviatura_tipo_doc= lector.IsDBNull(lector.GetOrdinal("tdocc_vabreviatura_tipo_doc")) ? default(string) : lector.GetString(lector.GetOrdinal("tdocc_vabreviatura_tipo_doc")),
                            tdocc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("tdocc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("tdocc_vdescripcion")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("tdocc_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tdocc_flag_estado")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return tipodocumento;
        }

        public int AddModulo(TipoDocumento entity)
        {
            int idresult;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TIPO_DOCUMENTO_DET_INSERT")))
            {
                _database.AddInParameter(comando, "@tdocc_icod_tipo_doc", DbType.Int32, entity.Id);
                _database.AddInParameter(comando, "@tablc_icod_modulo", DbType.Int32, entity.tablc_icod_modulo);
                _database.AddInParameter(comando, "@tdocd_flag_estado", DbType.Int32, 1);
                _database.AddInParameter(comando, "@tdocd_iusuario_crea", DbType.String, entity.UsuarioModificacion);
                _database.AddInParameter(comando, "@tdocd_pc_creacion", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                _database.ExecuteNonQuery(comando);
                idresult = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }
            return idresult;
        }
        public IList<TipoDocumento> GetIdModulo(TipoDocumento entity)
        {
            List<TipoDocumento> tipodocumento_de = new List<TipoDocumento>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TIPO_DOCUMENTO_DET_GetById")))
            {
                
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        tipodocumento_de.Add(new TipoDocumento
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("tdocc_icod_tipo_doc")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tdocc_icod_tipo_doc")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("tdocd_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tdocd_flag_estado")),
                            tablc_icod_modulo = lector.IsDBNull(lector.GetOrdinal("tablc_icod_modulo")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tablc_icod_modulo"))
                        });
                    }
                }
            }

            return tipodocumento_de;
        }


        #endregion
    }
}
