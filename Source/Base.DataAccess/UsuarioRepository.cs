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
    public class UsuarioRepository : Singleton<UsuarioRepository>, IUsuarioRepository<Usuario,int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

        #region Métodos Públicos

        public int Add(Usuario entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "UsuarioInsert")))
            {
                _database.AddInParameter(comando, "@Username", DbType.String, entity.Username);
                _database.AddInParameter(comando, "@Password", DbType.String, entity.Password);
                _database.AddInParameter(comando, "@ConfirmarPassword", DbType.String, entity.ConfirmarPassword);
                _database.AddInParameter(comando, "@Nombre", DbType.String, entity.Nombre);
                _database.AddInParameter(comando, "@Apellido", DbType.String, entity.Apellido);
                _database.AddInParameter(comando, "@Correo", DbType.String, entity.Correo);
                _database.AddInParameter(comando, "@CargoId", DbType.Int32, entity.CargoId);
                _database.AddInParameter(comando, "@RolId", DbType.Int32, entity.RolId);
                _database.AddInParameter(comando, "@Estado", DbType.Int32, entity.Estado);
                _database.AddInParameter(comando, "@UsuarioCreacion", DbType.String, entity.UsuarioCreacion);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }

        public int Delete(Usuario entity)
        {
            int idResult;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "UsuarioDelete")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                _database.ExecuteNonQuery(comando);
                idResult = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return idResult;
        }

        public bool Exists(Usuario entity)
        {
            bool existe = false;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "UsuarioGetByUsername")))
            {
                _database.AddInParameter(comando, "@Username", DbType.String, entity.Username);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector != null)
                    {
                        if (lector.Read())
                        {
                            existe = Convert.ToBoolean(lector.GetInt32(0));
                        }
                    }
                }
            }

            return existe;
        }

        public IList<Usuario> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            List<Usuario> usuarios = new List<Usuario>();
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
                        usuarios.Add(new Usuario
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            Username = lector.IsDBNull(lector.GetOrdinal("Username")) ? default(string) : lector.GetString(lector.GetOrdinal("Username")),
                            Nombre = lector.IsDBNull(lector.GetOrdinal("Nombre")) ? default(string) : lector.GetString(lector.GetOrdinal("Nombre")),
                            Apellido = lector.IsDBNull(lector.GetOrdinal("Apellido")) ? default(string) : lector.GetString(lector.GetOrdinal("Apellido")),
                            Correo = lector.IsDBNull(lector.GetOrdinal("Correo")) ? default(string) : lector.GetString(lector.GetOrdinal("Correo")),
                            RolId = lector.IsDBNull(lector.GetOrdinal("RolId")) ? default(int) : lector.GetInt32(lector.GetOrdinal("RolId")),
                            Rol = new Rol
                            {
                                Nombre = lector.IsDBNull(lector.GetOrdinal("RolNombre")) ? default(string) : lector.GetString(lector.GetOrdinal("RolNombre"))
                            },
                            Estado = lector.IsDBNull(lector.GetOrdinal("Estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Estado")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return usuarios;
        }

        public IList<Usuario> GetAll(string whereFilters)
        {
            throw new NotImplementedException();
        }

        public Usuario GetByIdGetById(Usuario entity)
        {
            Usuario usuario = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "UsuarioGetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            Username = lector.IsDBNull(lector.GetOrdinal("Username")) ? default(string) : lector.GetString(lector.GetOrdinal("Username")),
                            Password= lector.IsDBNull(lector.GetOrdinal("Password")) ? default(string) : lector.GetString(lector.GetOrdinal("Password")),
                            ConfirmarPassword = lector.IsDBNull(lector.GetOrdinal("ConfirmarPassword")) ? default(string) : lector.GetString(lector.GetOrdinal("ConfirmarPassword")),
                            Nombre = lector.IsDBNull(lector.GetOrdinal("Nombre")) ? default(string) : lector.GetString(lector.GetOrdinal("Nombre")),
                            Apellido = lector.IsDBNull(lector.GetOrdinal("Apellido")) ? default(string) : lector.GetString(lector.GetOrdinal("Apellido")),
                            Correo = lector.IsDBNull(lector.GetOrdinal("Correo")) ? default(string) : lector.GetString(lector.GetOrdinal("Correo")),
                            CargoId = lector.IsDBNull(lector.GetOrdinal("CargoId")) ? default(int) : lector.GetInt32(lector.GetOrdinal("CargoId")),
                            RolId = lector.IsDBNull(lector.GetOrdinal("RolId")) ? default(int) : lector.GetInt32(lector.GetOrdinal("RolId")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("Estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Estado"))
                        };
                    }
                }
            }

            return usuario;
        }

        public Usuario GetByUsername(string username, string pass)
        {
            Usuario usuario = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "UsuarioLogin")))
            {
                _database.AddInParameter(comando, "@Username", DbType.String, username);
                _database.AddInParameter(comando, "@Password", DbType.String, pass);
                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            Username = lector.IsDBNull(lector.GetOrdinal("Username")) ? default(string) : lector.GetString(lector.GetOrdinal("Username")),
                            RolId = lector.IsDBNull(lector.GetOrdinal("RolId")) ? default(int) : lector.GetInt32(lector.GetOrdinal("RolId")),
                            Rol = new Rol
                            {
                                Nombre = lector.IsDBNull(lector.GetOrdinal("RolNombre")) ? default(string) : lector.GetString(lector.GetOrdinal("RolNombre"))
                            },
                            Estado = lector.IsDBNull(lector.GetOrdinal("Estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Estado"))
                        };
                    }
                }
            }

            return usuario;
        }

        public int Update(Usuario entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "UsuarioUpdate")))
            {
                _database.AddInParameter(comando, "@Username", DbType.String, entity.Username);
                _database.AddInParameter(comando, "@Password", DbType.String, entity.Password);
                _database.AddInParameter(comando, "@ConfirmarPassword", DbType.String, entity.ConfirmarPassword);
                _database.AddInParameter(comando, "@Nombre", DbType.String, entity.Nombre);
                _database.AddInParameter(comando, "@Apellido", DbType.String, entity.Apellido);
                _database.AddInParameter(comando, "@Correo", DbType.String, entity.Correo);
                _database.AddInParameter(comando, "@CargoId", DbType.Int32, entity.CargoId);
                _database.AddInParameter(comando, "@RolId", DbType.Int32, entity.RolId);
                _database.AddInParameter(comando, "@Estado", DbType.Int32, entity.Estado);
                _database.AddInParameter(comando, "@UsuarioModificacion", DbType.String, entity.UsuarioModificacion);
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }


    
        #endregion
    }
}
