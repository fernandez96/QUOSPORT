using Base.BusinessEntity;
using Base.Common;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Security.Principal;
namespace Base.DataAccess
{
    public class CategoriaRepository: Singleton<CategoriaRepository>, ICategoriaRepository<Categoria,Linea,SubLinea, int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        public const int add = 1;
        public const int update = 2;
        public const int delete = 3;
        #endregion

        #region Métodos Públicos
        public int Add(Categoria entity)
        {
            int idcategoria;
            int idlinea;
            int idsublinea;
            using (DbConnection conexion =_database.CreateConnection())
            {
                conexion.Open();
                using (var transaction=conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_CATEGORIAS_PRODUCTO_INSERT")))
                        {
                            _database.AddInParameter(comando, "@ctgcc_vcod_categoria", DbType.String, entity.ctgcc_vcod_categoria);
                            _database.AddInParameter(comando, "@ctgcc_vdescripcion", DbType.String, entity.ctgcc_vdescripcion);
                            _database.AddInParameter(comando, "@ctgc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                            _database.AddInParameter(comando, "@ctgc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                            _database.AddInParameter(comando, "@ctgcc_iflag_estado", DbType.Int32, 1);
                            _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                            _database.ExecuteNonQuery(comando);

                            idcategoria = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                            if (idcategoria == -1) throw new Exception("Ya existe la Categoria de producto " + entity.ctgcc_vdescripcion);
                        }
                        foreach (var item in entity.detalleLinea)
                        {
                            if (item.status == add)
                            {
                                using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_CAB_INSERT")))
                                {
                                    _database.AddInParameter(comando, "@ctgcc_iid_categoria", DbType.Int32, idcategoria);
                                    _database.AddInParameter(comando, "@linc_vcod_linea", DbType.String, item.linc_vcod_linea);
                                    _database.AddInParameter(comando, "@linc_vdescripcion", DbType.String, item.linc_vdescripcion);
                                    _database.AddInParameter(comando, "@linc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                                    _database.AddInParameter(comando, "@linc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                    _database.AddInParameter(comando, "@linc_iflag_estado", DbType.Int32, 1);
                                    _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                                    _database.ExecuteNonQuery(comando);

                                    idlinea = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                                    if (idlinea == -1) throw new Exception("Ya existe la Linea " + item.linc_vdescripcion);
                                }
                                foreach (var itemsublinea in entity.detalleSubLinea)
                                {
                                    if (itemsublinea.status == add)
                                    {
                                        if (item.Id == itemsublinea.idLinea)
                                        {
                                            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_DET_INSERT")))
                                            {
                                                _database.AddInParameter(comando, "@ctgcc_iid_categoria", DbType.Int32, idcategoria);
                                                _database.AddInParameter(comando, "@linc_iid_linea", DbType.Int32, idlinea);
                                                _database.AddInParameter(comando, "@lind_vcod_sublinea", DbType.String, itemsublinea.lind_vcod_sublinea);
                                                _database.AddInParameter(comando, "@lind_vdescripcion", DbType.String, itemsublinea.lind_vdescripcion);
                                                _database.AddInParameter(comando, "@lind_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                                                _database.AddInParameter(comando, "@lind_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                                _database.AddInParameter(comando, "@lind_iflag_estado", DbType.Int32, 1);
                                                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                                                _database.ExecuteNonQuery(comando);

                                                idsublinea = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                                                if (idsublinea == -1) throw new Exception("Ya existe la Sub-Linea " + itemsublinea.lind_vdescripcion);
                                            }
                                        }

                                    }

                                }
                            }


                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
            return idcategoria;
        }
        public int Update(Categoria entity)
        {
            int idcategoria;
            int idlinea;
            int idsublinea;
            using (DbConnection conexion = _database.CreateConnection())
            {
                conexion.Open();
                using (var transaction = conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_CATEGORIAS_PRODUCTO_UPDATE")))
                        {
                            _database.AddInParameter(comando, "@id", DbType.String, entity.Id);
                            _database.AddInParameter(comando, "@ctgcc_vdescripcion", DbType.String, entity.ctgcc_vdescripcion);
                            _database.AddInParameter(comando, "@ctgc_vusuario_modifica", DbType.String, entity.UsuarioModificacion);
                            _database.AddInParameter(comando, "@ctgc_vpc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                            _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                            _database.ExecuteNonQuery(comando);

                            idcategoria = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                            if (idcategoria == -1) throw new Exception("Ya existe la Categoria de producto " + entity.ctgcc_vdescripcion);
                        }
                        foreach (var item in entity.detalleLinea)
                        {
                            if (item.status == update)
                            {
                                using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_CAB_UPDATE")))
                                {
                                    _database.AddInParameter(comando, "@id", DbType.Int32, item.Id);
                                    _database.AddInParameter(comando, "@linc_vcod_linea", DbType.String, item.linc_vcod_linea);
                                    _database.AddInParameter(comando, "@linc_vdescripcion", DbType.String, item.linc_vdescripcion);
                                    _database.AddInParameter(comando, "@linc_vusuario_modifica", DbType.String, entity.UsuarioModificacion);
                                    _database.AddInParameter(comando, "@linc_vpc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                                    _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                                    _database.ExecuteNonQuery(comando);

                                    idlinea = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                                    if (idlinea == -1) throw new Exception("Ya existe la Linea " + item.linc_vdescripcion);
                                }
                                foreach (var itemsublinea in entity.detalleSubLinea)
                                {
                                    if (itemsublinea.status == update)
                                    {
                                        if (item.Id == itemsublinea.idLinea)
                                        {
                                            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_DET_INSERT")))
                                            {
                                                _database.AddInParameter(comando, "@id", DbType.Int32, itemsublinea.Id);
                                                _database.AddInParameter(comando, "@lind_vcod_sublinea", DbType.String, itemsublinea.lind_vcod_sublinea);
                                                _database.AddInParameter(comando, "@lind_vdescripcion", DbType.String, itemsublinea.lind_vdescripcion);
                                                _database.AddInParameter(comando, "@lind_vusuario_modifica", DbType.String, entity.UsuarioModificacion);
                                                _database.AddInParameter(comando, "@lind_vpc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                                                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                                                _database.ExecuteNonQuery(comando);

                                                idsublinea = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                                                if (idsublinea == -1) throw new Exception("Ya existe la Sub-Linea " + itemsublinea.lind_vdescripcion);
                                            }
                                        }

                                    }

                                }
                            }
                            else if (item.status==add)
                            {
                                using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_CAB_INSERT")))
                                {
                                    _database.AddInParameter(comando, "@ctgcc_iid_categoria", DbType.Int32, idcategoria);
                                    _database.AddInParameter(comando, "@linc_vcod_linea", DbType.String, item.linc_vcod_linea);
                                    _database.AddInParameter(comando, "@linc_vdescripcion", DbType.String, item.linc_vdescripcion);
                                    _database.AddInParameter(comando, "@linc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                                    _database.AddInParameter(comando, "@linc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                    _database.AddInParameter(comando, "@linc_iflag_estado", DbType.Int32, 1);
                                    _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                                    _database.ExecuteNonQuery(comando);

                                    idlinea = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                                    if (idlinea == -1) throw new Exception("Ya existe la Linea " + item.linc_vdescripcion);
                                }
                                foreach (var itemsublinea in entity.detalleSubLinea)
                                {
                                    if (itemsublinea.status == add)
                                    {
                                        if (item.Id == itemsublinea.idLinea)
                                        {
                                            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_DET_INSERT")))
                                            {
                                                _database.AddInParameter(comando, "@ctgcc_iid_categoria", DbType.Int32, idcategoria);
                                                _database.AddInParameter(comando, "@linc_iid_linea", DbType.Int32, idlinea);
                                                _database.AddInParameter(comando, "@lind_vcod_sublinea", DbType.String, itemsublinea.lind_vcod_sublinea);
                                                _database.AddInParameter(comando, "@lind_vdescripcion", DbType.String, itemsublinea.lind_vdescripcion);
                                                _database.AddInParameter(comando, "@lind_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                                                _database.AddInParameter(comando, "@lind_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                                                _database.AddInParameter(comando, "@lind_iflag_estado", DbType.Int32, 1);
                                                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                                                _database.ExecuteNonQuery(comando);

                                                idsublinea = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                                                if (idsublinea == -1) throw new Exception("Ya existe la Sub-Linea " + itemsublinea.lind_vdescripcion);
                                            }
                                        }

                                    }

                                }
                            }
                            else if (item.status == delete)
                            {
                                using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_CAB_DELETE")))
                                {
                                    _database.AddInParameter(comando, "@Id", DbType.Int32, idcategoria);
                                    _database.ExecuteNonQuery(comando);

                                    idlinea = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                                    if (idlinea == 0) throw new Exception("Error al iliminar " + item.linc_vdescripcion);
                                }
                                foreach (var itemsublinea in entity.detalleSubLinea)
                                {
                                    if (itemsublinea.status == delete)
                                    {
                                        if (item.Id == itemsublinea.idLinea)
                                        {
                                            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_DET_DELETE")))
                                            {
                                                _database.AddInParameter(comando, "@Id", DbType.Int32, idlinea);
                                                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                                                _database.ExecuteNonQuery(comando);

                                                idsublinea = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
                                                if (idsublinea == 0) throw new Exception("Ya existe la Sub-Linea " + itemsublinea.lind_vdescripcion);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
            return idcategoria;
        }
        public IList<Categoria> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            List<Categoria> categoria = new List<Categoria>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_CATEGORIAS_PRODUCTO_GetAllFilter")))
            {
                _database.AddInParameter(comando, "@WhereFilters", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.WhereFilter) ? string.Empty : paginationParameters.WhereFilter);
                _database.AddInParameter(comando, "@OrderBy", DbType.String, string.IsNullOrWhiteSpace(paginationParameters.OrderBy) ? string.Empty : paginationParameters.OrderBy);
                _database.AddInParameter(comando, "@Start", DbType.Int32, paginationParameters.Start);
                _database.AddInParameter(comando, "@Rows", DbType.Int32, paginationParameters.AmountRows);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        categoria.Add(new Categoria
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            ctgcc_vcod_categoria = lector.IsDBNull(lector.GetOrdinal("ctgcc_vcod_categoria")) ? default(string) : lector.GetString(lector.GetOrdinal("ctgcc_vcod_categoria")),
                            ctgcc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("ctgcc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("ctgcc_vdescripcion")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("ctgcc_iflag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("ctgcc_iflag_estado")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return categoria;
        }
        public IList<Categoria> GetAll()
        {
            List<Categoria> categoria = new List<Categoria>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_CATEGORIAS_PRODUCTO_GetAll")))
            {
                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        categoria.Add(new Categoria
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            ctgcc_vcod_categoria = lector.IsDBNull(lector.GetOrdinal("ctgcc_vcod_categoria")) ? default(string) : lector.GetString(lector.GetOrdinal("ctgcc_vcod_categoria")),
                            ctgcc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("ctgcc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("ctgcc_vdescripcion")),
                        });
                    }
                }
            }

            return categoria;
        }
        public IList<Linea> GetAllLinea(Linea entity)
        {
            List<Linea> linea = new List<Linea>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_CAB_GetAll")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        linea.Add(new Linea
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("linc_iid_linea")) ? default(int) : lector.GetInt32(lector.GetOrdinal("linc_iid_linea")),
                            linc_vcod_linea = lector.IsDBNull(lector.GetOrdinal("linc_vcod_linea")) ? default(string) : lector.GetString(lector.GetOrdinal("linc_vcod_linea")),
                            linc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("linc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("linc_vdescripcion")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("linc_iflag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("linc_iflag_estado")),
                        });
                    }
                }
            }

            return linea;
        }

        public IList<Linea> AllLinea()
        {
            List<Linea> linea = new List<Linea>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_CAB_All")))
            {
                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        linea.Add(new Linea
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("linc_iid_linea")) ? default(int) : lector.GetInt32(lector.GetOrdinal("linc_iid_linea")),
                            ctgcc_iid_categoria = lector.IsDBNull(lector.GetOrdinal("ctgcc_iid_categoria")) ? default(int) : lector.GetInt32(lector.GetOrdinal("ctgcc_iid_categoria")),
                            linc_vcod_linea = lector.IsDBNull(lector.GetOrdinal("linc_vcod_linea")) ? default(string) : lector.GetString(lector.GetOrdinal("linc_vcod_linea")),
                            linc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("linc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("linc_vdescripcion")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("linc_iflag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("linc_iflag_estado")),
                        });
                    }
                }
            }

            return linea;
        }
        public IList<SubLinea> GetAllSubLinea(SubLinea entity)
        {
            List<SubLinea> subLinea = new List<SubLinea>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_DET_GetAll")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        subLinea.Add(new SubLinea
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("lind_iid_sublinea")) ? default(int) : lector.GetInt32(lector.GetOrdinal("lind_iid_sublinea")),
                            idLinea = lector.IsDBNull(lector.GetOrdinal("linc_iid_linea")) ? default(int) : lector.GetInt32(lector.GetOrdinal("linc_iid_linea")),
                            lind_vcod_sublinea = lector.IsDBNull(lector.GetOrdinal("lind_vcod_sublinea")) ? default(string) : lector.GetString(lector.GetOrdinal("lind_vcod_sublinea")),
                            lind_vdescripcion = lector.IsDBNull(lector.GetOrdinal("lind_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("lind_vdescripcion")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("lind_iflag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("lind_iflag_estado")),
                        });
                    }
                }
            }

            return subLinea;
        }
        public IList<SubLinea> AllSubLinea()
        {
            List<SubLinea> subLinea = new List<SubLinea>();
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_LINEA_PRODUCTO_DET_All")))
            {

                using (var lector = _database.ExecuteReader(comando))
                {
                    while (lector.Read())
                    {
                        subLinea.Add(new SubLinea
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("lind_iid_sublinea")) ? default(int) : lector.GetInt32(lector.GetOrdinal("lind_iid_sublinea")),
                            linc_iid_linea = lector.IsDBNull(lector.GetOrdinal("linc_iid_linea")) ? default(int) : lector.GetInt32(lector.GetOrdinal("linc_iid_linea")),
                            idLinea = lector.IsDBNull(lector.GetOrdinal("linc_iid_linea")) ? default(int) : lector.GetInt32(lector.GetOrdinal("linc_iid_linea")),
                            lind_vcod_sublinea = lector.IsDBNull(lector.GetOrdinal("lind_vcod_sublinea")) ? default(string) : lector.GetString(lector.GetOrdinal("lind_vcod_sublinea")),
                            lind_vdescripcion = lector.IsDBNull(lector.GetOrdinal("lind_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("lind_vdescripcion")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("lind_iflag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("lind_iflag_estado")),
                        });
                    }
                }
            }

            return subLinea;
        }
        public Categoria GetById(Categoria entity)
        {
            Categoria categoria = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_CATEGORIAS_PRODUCTO_GetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        categoria = new Categoria
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("ctgcc_iid_categoria")) ? default(int) : lector.GetInt32(lector.GetOrdinal("ctgcc_iid_categoria")),
                            ctgcc_vcod_categoria = lector.IsDBNull(lector.GetOrdinal("ctgcc_vcod_categoria")) ? default(string) : lector.GetString(lector.GetOrdinal("ctgcc_vcod_categoria")),
                            ctgcc_vdescripcion = lector.IsDBNull(lector.GetOrdinal("ctgcc_vdescripcion")) ? default(string) : lector.GetString(lector.GetOrdinal("ctgcc_vdescripcion")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("ctgcc_iflag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("ctgcc_iflag_estado"))
                        };
                    }
                }
            }

            return categoria;
        }
        #endregion
    }
}
