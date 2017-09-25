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
    public class TablaRegistroRepository: Singleton<TablaRegistroRepository>, ITablaRegistroRepository<TablaRegistro, int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

        #region Métodos publicos de cabezera
        public int Add(TablaRegistro entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_OPCIONES_CAB_INSERT")))
            {
                _database.AddInParameter(comando, "@tbpc_vcod_tabla_opciones", DbType.String, entity.tbpc_vcod_tabla_opciones);
                _database.AddInParameter(comando, "@tbpc_vdescripcion", DbType.String, entity.tbpc_vdescripcion);
                _database.AddInParameter(comando, "@tbpc_iusuario_crea", DbType.String, entity.UsuarioCreacion);
                _database.AddInParameter(comando, "@tbpc_pc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "@tbpc_flag_estado", DbType.Int32, 1);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }
        public IList<TablaRegistro> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            List<TablaRegistro> tablaregistro = new List<TablaRegistro>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_OPCIONES_CAB_GetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.WhereFilter) ? string.Empty : paginationParameters.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.OrderBy) ? string.Empty : paginationParameters.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.Int32, paginationParameters.Start);
                _database.AddInParameter(comando, "@Rows", DbType.Int32, paginationParameters.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        tablaregistro.Add(new TablaRegistro
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            tbpc_vcod_tabla_opciones = lector.IsDBNull(lector.GetOrdinal("tbpc_vcod_tabla_opciones")) ? default(string) : lector.GetString(lector.GetOrdinal("tbpc_vcod_tabla_opciones")),
                            tbpc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("tbpc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("tbpc_vdescripcion")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("tbpc_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tbpc_flag_estado")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return tablaregistro;
        }
        public TablaRegistro GetById(TablaRegistro entity)
        {
            TablaRegistro tablaregistro = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_OPCIONES_CAB_GetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        tablaregistro = new TablaRegistro
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("tbpc_iid_tabla_opciones")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tbpc_iid_tabla_opciones")),
                            tbpc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("tbpc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("tbpc_vdescripcion")),
                            tbpc_vcod_tabla_opciones = lector.IsDBNull(lector.GetOrdinal("tbpc_vcod_tabla_opciones")) ? default(string) : lector.GetString(lector.GetOrdinal("tbpc_vcod_tabla_opciones")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("tbpc_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tbpc_flag_estado"))
                        };
                    }
                }
            }

            return tablaregistro;
        }
        public int Update(TablaRegistro entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_OPCIONES_CAB_UPDATE")))
            {
                _database.AddInParameter(comando, "@tbpc_vdescripcion", DbType.String, entity.tbpc_vdescripcion);
                _database.AddInParameter(comando, "@tbpc_iusuario_modifica", DbType.String, entity.UsuarioModificacion);
                _database.AddInParameter(comando, "@tbpc_pc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "@id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }

        public TablaRegistro GetCorrelativaCab()
        {
            TablaRegistro tablaregistro = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_OPCIONES_CAB_GetDocumento")))
            {
                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        tablaregistro = new TablaRegistro
                        {
                            correlativaCab = lector.IsDBNull(lector.GetOrdinal("Correlativo")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Correlativo")),

                        };
                    }
                }
            }

            return tablaregistro;
        }

        #endregion

        #region Métodos publicos de Detalle
        public int AddDetalle(TablaRegistro entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_OPCIONES_DET_INSERT")))
            {
                _database.AddInParameter(comando, "@tbpc_iid_tabla_opciones", DbType.Int32, entity.Id);
                _database.AddInParameter(comando, "@tbpd_vcod_tabla_opciones_det", DbType.String, entity.tbpd_vcod_tabla_opciones_det);
                _database.AddInParameter(comando, "@tbpd_vdescripcion_detalle", DbType.String, entity.tbpd_vdescripcion_detalle);
                _database.AddInParameter(comando, "@tbpd_iusuario_crea", DbType.String, entity.UsuarioModificacion);
                _database.AddInParameter(comando, "@tbpd_pc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "@tbpd_flag_estado", DbType.Int32, 1);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }
        public IList<TablaRegistro> GetAllPagingDetalle(PaginationParameter<int> paginationParameters)
        {
            List<TablaRegistro> tablaregistro = new List<TablaRegistro>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_OPCIONES_DET_GetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.WhereFilter) ? string.Empty : paginationParameters.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.OrderBy) ? string.Empty : paginationParameters.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.Int32, paginationParameters.Start);
                _database.AddInParameter(comando, "@Rows", DbType.Int32, paginationParameters.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        tablaregistro.Add(new TablaRegistro
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            tbpd_vcod_tabla_opciones_det = lector.IsDBNull(lector.GetOrdinal("tbpd_vcod_tabla_opciones_det")) ? default(string) : lector.GetString(lector.GetOrdinal("tbpd_vcod_tabla_opciones_det")),
                            tbpd_vdescripcion_detalle = lector.IsDBNull(lector.GetOrdinal("tbpd_vdescripcion_detalle")) ? default(string) : lector.GetString(lector.GetOrdinal("tbpd_vdescripcion_detalle")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("tbpd_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tbpd_flag_estado")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return tablaregistro;
        }
        public TablaRegistro GetByIdDetalle(TablaRegistro entity)
        {
            TablaRegistro tablaregistro = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_OPCIONES_DET_GetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        tablaregistro = new TablaRegistro
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("tbpd_iid_tabla_opciones_det")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tbpd_iid_tabla_opciones_det")),
                            tbpd_vdescripcion_detalle = lector.IsDBNull(lector.GetOrdinal("tbpd_vdescripcion_detalle")) ? default(string) : lector.GetString(lector.GetOrdinal("tbpd_vdescripcion_detalle")),
                            tbpd_vcod_tabla_opciones_det = lector.IsDBNull(lector.GetOrdinal("tbpd_vcod_tabla_opciones_det")) ? default(string) : lector.GetString(lector.GetOrdinal("tbpd_vcod_tabla_opciones_det")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("tbpd_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tbpd_flag_estado"))
                        };
                    }
                }
            }

            return tablaregistro;
        }
        public int UpdateDetalle(TablaRegistro entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_OPCIONES_DET_UPDATE")))
            {
                _database.AddInParameter(comando, "@tbpd_vdescripcion_detalle", DbType.String, entity.tbpd_vdescripcion_detalle);
                _database.AddInParameter(comando, "@tbpd_iusuario_modifica", DbType.String, entity.UsuarioModificacion);
                _database.AddInParameter(comando, "@tbpd_pc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "@id", DbType.Int32, entity.Id);
                _database.AddInParameter(comando, "@idTabla", DbType.Int32, entity.tbpc_iid_tabla_opciones);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }

        public int DeleteDetalle(TablaRegistro entity)
        {
            int idResult;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_OPCIONES_DET_DELETE")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddInParameter(comando, "@tbpd_pc_elimina", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "@tbpd_iusuario_eliminacion", DbType.String, entity.UsuarioModificacion);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                _database.ExecuteNonQuery(comando);
                idResult = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return idResult;
        }

        public TablaRegistro GetCorrelativaDet(TablaRegistro entity)
        {
            TablaRegistro tablaregistro = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_OPCIONES_DET_GetDocumento")))
            {
                _database.AddInParameter(comando, "@idtabla", DbType.Int32, entity.Id);
                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        tablaregistro = new TablaRegistro
                        {
                            correlativaDet = lector.IsDBNull(lector.GetOrdinal("Correlativo")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Correlativo"))
                        };
                    }
                }
            }

            return tablaregistro;
        }

        #endregion

        #region metodo universal 
        public IList<TablaRegistro> GetAll(int idtatble)
        {
            List<TablaRegistro> tablaregistro = new List<TablaRegistro>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TABLA_OPCIONES_GetById")))
            {
                _database.AddInParameter(comando, "@idTabla", DbType.Int32, idtatble);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        tablaregistro.Add(new TablaRegistro
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("tbpd_iid_tabla_opciones_det")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tbpd_iid_tabla_opciones_det")),
                            tbpd_vdescripcion_detalle = lector.IsDBNull(lector.GetOrdinal("tbpd_vdescripcion_detalle")) ? default(string) : lector.GetString(lector.GetOrdinal("tbpd_vdescripcion_detalle")),
                           
                        });
                    }
                }
            }

            return tablaregistro;
        }       
        #endregion

    }
}
