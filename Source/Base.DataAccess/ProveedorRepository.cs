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
    public class ProveedorRepository: Singleton<ProveedorRepository>, IProveedorRepository<Proveedor, int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion
        #region Métodos Públicos

        public int Add(Proveedor entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_PROVEEDOR_INSERT")))
            {
                _database.AddInParameter(comando, "@proc_vcod_proveedor", DbType.String, entity.proc_vcod_proveedor);
                _database.AddInParameter(comando, "@proc_tipo_persona", DbType.Int32, entity.proc_tipo_persona);
                _database.AddInParameter(comando, "@proc_sfecha", DbType.DateTime, entity.proc_sfecha);
                _database.AddInParameter(comando, "@proc_tipo_doc", DbType.Int32, entity.proc_tipo_doc);
                _database.AddInParameter(comando, "@proc_documento", DbType.String, entity.proc_documento);
                _database.AddInParameter(comando, "@proc_vnombrecompleto", DbType.String, entity.proc_vnombrecompleto);
                _database.AddInParameter(comando, "@proc_vcomercial", DbType.String, entity.proc_vcomercial);
                _database.AddInParameter(comando, "@proc_vnombre", DbType.String, entity.proc_vnombre);
                _database.AddInParameter(comando, "@proc_vpaterno", DbType.String, entity.proc_vpaterno);
                _database.AddInParameter(comando, "@proc_vmaterno", DbType.String, entity.proc_vmaterno);
                _database.AddInParameter(comando, "@proc_vruc", DbType.String, entity.proc_vruc);
                _database.AddInParameter(comando, "@proc_vdireccion", DbType.String, entity.proc_vdireccion);
                _database.AddInParameter(comando, "@proc_vtelefono", DbType.String, entity.proc_vtelefono);
                _database.AddInParameter(comando, "@proc_vfax", DbType.String, entity.proc_vfax);
                _database.AddInParameter(comando, "@proc_vcorreo", DbType.String, entity.proc_vcorreo);
                _database.AddInParameter(comando, "@proc_vrepresentante", DbType.String, entity.proc_vrepresentante);
                _database.AddInParameter(comando, "@proc_iid_usuario_crea", DbType.String, entity.UsuarioCreacion);
                _database.AddInParameter(comando, "@proc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "@proc_flag_estado", DbType.Int32, entity.Estado);
                _database.AddOutParameter(comando,"@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }
        public int Update(Proveedor entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_PROVEEDOR_UPDATE")))
            {


                _database.AddInParameter(comando, "@proc_vcod_proveedor", DbType.String, entity.proc_vcod_proveedor);
                _database.AddInParameter(comando, "@proc_tipo_persona", DbType.Int32, entity.proc_tipo_persona);
                _database.AddInParameter(comando, "@proc_sfecha", DbType.DateTime, entity.proc_sfecha);
                _database.AddInParameter(comando, "@proc_tipo_doc", DbType.Int32, entity.proc_tipo_doc);
                _database.AddInParameter(comando, "@proc_documento", DbType.String, entity.proc_documento);
                _database.AddInParameter(comando, "@proc_vnombrecompleto", DbType.String, entity.proc_vnombrecompleto);
                _database.AddInParameter(comando, "@proc_vcomercial", DbType.String, entity.proc_vcomercial);
                _database.AddInParameter(comando, "@proc_vnombre", DbType.String, entity.proc_vnombre);
                _database.AddInParameter(comando, "@proc_vpaterno", DbType.String, entity.proc_vpaterno);
                _database.AddInParameter(comando, "@proc_vmaterno", DbType.String, entity.proc_vmaterno);
                _database.AddInParameter(comando, "@proc_vruc", DbType.String, entity.proc_vruc);
                _database.AddInParameter(comando, "@proc_vdireccion", DbType.String, entity.proc_vdireccion);
                _database.AddInParameter(comando, "@proc_vtelefono", DbType.String, entity.proc_vtelefono);
                _database.AddInParameter(comando, "@proc_vfax", DbType.String, entity.proc_vfax);
                _database.AddInParameter(comando, "@proc_vcorreo", DbType.String, entity.proc_vcorreo);
                _database.AddInParameter(comando, "@proc_vrepresentante", DbType.String, entity.proc_vrepresentante);
                _database.AddInParameter(comando, "@proc_vid_usuario_modifica", DbType.String, entity.UsuarioCreacion);
                _database.AddInParameter(comando, "@proc_vpc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "@proc_flag_estado", DbType.Int32, entity.Estado);
                _database.AddInParameter(comando, "@id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }
        public int Delete(Proveedor entity)
        {
            int idResult;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_PROVEEDOR_DELETE")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                _database.ExecuteNonQuery(comando);
                idResult = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return idResult;
        }
        public int GetExisteProveedor(int tipo, string dni, string ruc,string nombre)
        {
            int result;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "EXISTEPROVEEDOR")))
            {
                _database.AddInParameter(comando, "@TipoPersona", DbType.Int32 , tipo);
                _database.AddInParameter(comando, "@DNI", DbType.String, dni);
                _database.AddInParameter(comando, "@RUC", DbType.String, ruc);
                _database.AddInParameter(comando, "@proc_vnombre", DbType.String, nombre);
                _database.AddOutParameter(comando, "@Response", DbType.String, 11);
                _database.ExecuteNonQuery(comando);
                result = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return result;
        }
        public IList<Proveedor> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            List<Proveedor> proveedor = new List<Proveedor>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "UsuarioGetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.WhereFilter) ? string.Empty : paginationParameters.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.OrderBy) ? string.Empty : paginationParameters.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.Int32, paginationParameters.Start);
                _database.AddInParameter(comando, "@Rows", DbType.Int32, paginationParameters.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        proveedor.Add(new Proveedor
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            proc_vcod_proveedor = lector.IsDBNull(lector.GetOrdinal("proc_vcod_proveedor")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vcod_proveedor")),
                            proc_tipo_persona = lector.IsDBNull(lector.GetOrdinal("proc_tipo_persona")) ? default(int) : lector.GetInt32(lector.GetOrdinal("proc_tipo_persona")),
                            proc_sfecha = lector.IsDBNull(lector.GetOrdinal("proc_sfecha")) ? default(DateTime) : lector.GetDateTime(lector.GetOrdinal("proc_sfecha")),
                            proc_tipo_doc = lector.IsDBNull(lector.GetOrdinal("proc_tipo_doc")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_tipo_doc")),
                            proc_documento = lector.IsDBNull(lector.GetOrdinal("proc_documento")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_documento")),
                            proc_vnombrecompleto = lector.IsDBNull(lector.GetOrdinal("proc_vnombrecompleto")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vnombrecompleto")),
                            proc_vcomercial = lector.IsDBNull(lector.GetOrdinal("proc_vcomercial")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vcomercial")),
                            proc_vnombre = lector.IsDBNull(lector.GetOrdinal("proc_vnombre")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vnombre")),
                            proc_vpaterno = lector.IsDBNull(lector.GetOrdinal("proc_vpaterno")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vpaterno")),
                            proc_vmaterno = lector.IsDBNull(lector.GetOrdinal("proc_vmaterno")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vmaterno")),
                            proc_vruc = lector.IsDBNull(lector.GetOrdinal("proc_vruc")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vruc")),
                            proc_vdireccion = lector.IsDBNull(lector.GetOrdinal("proc_vdireccion")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vdireccion")),
                            proc_vtelefono = lector.IsDBNull(lector.GetOrdinal("proc_vtelefono")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vtelefono")),
                            proc_vfax = lector.IsDBNull(lector.GetOrdinal("proc_vfax")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vfax")),
                            proc_vcorreo = lector.IsDBNull(lector.GetOrdinal("proc_vcorreo")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vcorreo")),
                            proc_vrepresentante = lector.IsDBNull(lector.GetOrdinal("proc_vrepresentante")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vrepresentante")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("proc_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("proc_flag_estado")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))     
                        });
                    }
                }
            }

            return proveedor;
        }
        public Proveedor GetById(Proveedor entity)
        {
            Proveedor proveedor = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "UsuarioGetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        proveedor = new Proveedor
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("proc_iid_proveedor")) ? default(int) : lector.GetInt32(lector.GetOrdinal("proc_iid_proveedor")),
                            proc_vcod_proveedor = lector.IsDBNull(lector.GetOrdinal("proc_vcod_proveedor")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vcod_proveedor")),
                            proc_tipo_persona = lector.IsDBNull(lector.GetOrdinal("proc_tipo_persona")) ? default(int) : lector.GetInt32(lector.GetOrdinal("proc_tipo_persona")),
                            proc_sfecha = lector.IsDBNull(lector.GetOrdinal("proc_sfecha")) ? default(DateTime) : lector.GetDateTime(lector.GetOrdinal("proc_sfecha")),
                            proc_tipo_doc = lector.IsDBNull(lector.GetOrdinal("proc_tipo_doc")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_tipo_doc")),
                            proc_documento = lector.IsDBNull(lector.GetOrdinal("proc_documento")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_documento")),
                            proc_vnombrecompleto = lector.IsDBNull(lector.GetOrdinal("proc_vnombrecompleto")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vnombrecompleto")),
                            proc_vcomercial = lector.IsDBNull(lector.GetOrdinal("proc_vcomercial")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vcomercial")),
                            proc_vnombre = lector.IsDBNull(lector.GetOrdinal("proc_vnombre")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vnombre")),
                            proc_vpaterno = lector.IsDBNull(lector.GetOrdinal("proc_vpaterno")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vpaterno")),
                            proc_vmaterno = lector.IsDBNull(lector.GetOrdinal("proc_vmaterno")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vmaterno")),
                            proc_vruc = lector.IsDBNull(lector.GetOrdinal("proc_vruc")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vruc")),
                            proc_vdireccion = lector.IsDBNull(lector.GetOrdinal("proc_vdireccion")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vdireccion")),
                            proc_vtelefono = lector.IsDBNull(lector.GetOrdinal("proc_vtelefono")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vtelefono")),
                            proc_vfax = lector.IsDBNull(lector.GetOrdinal("proc_vfax")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vfax")),
                            proc_vcorreo = lector.IsDBNull(lector.GetOrdinal("proc_vcorreo")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vcorreo")),
                            proc_vrepresentante = lector.IsDBNull(lector.GetOrdinal("proc_vrepresentante")) ? default(string) : lector.GetString(lector.GetOrdinal("proc_vrepresentante")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("proc_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("proc_flag_estado")),
                        };
                    }
                }
            }

            return proveedor;
        }
        public int GetCorrelativo()
        {
            int Correlativo;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSFERENCIA_CORRELATIVA")))
            {
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                Correlativo = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return Correlativo;
        }



        #endregion
    }
}
