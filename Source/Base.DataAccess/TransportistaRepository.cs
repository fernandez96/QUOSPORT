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
    public class TransportistaRepository : Singleton<TransportistaRepository>, ITransportistaRepository<Transportista, int>
    {
        #region Attributos
        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);
        #endregion

        #region Métodos Públicos

        public int Add(Transportista entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "")))
            {
                _database.AddInParameter(comando, "", DbType.String, entity.tranc_cod_transportista);
                _database.AddInParameter(comando, "", DbType.String, entity.tranc_vnombre_transportista);
                _database.AddInParameter(comando, "", DbType.String, entity.tranc_vdireccion);
                _database.AddInParameter(comando, "", DbType.String, entity.tranc_vnumero_telefono);
                _database.AddInParameter(comando, "", DbType.String, entity.tranc_vnum_marca_placa);
                _database.AddInParameter(comando, "", DbType.String, entity.tranc_vnum_certif_inscrip);
                _database.AddInParameter(comando, "", DbType.Int32, entity.tranc_vnum_licencia_conducir);
                _database.AddInParameter(comando, "", DbType.Int32, entity.tranc_ruc);
                _database.AddInParameter(comando, "", DbType.Int32, entity.Estado);
                _database.AddInParameter(comando, "", DbType.String, entity.UsuarioCreacion);
                _database.AddInParameter(comando, "", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddOutParameter(comando, "", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }
        public int Delete(Transportista entity)
        {
            int idResult;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                _database.ExecuteNonQuery(comando);
                idResult = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return idResult;
        }
        public IList<Transportista> GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            List<Transportista> usuarios = new List<Transportista>();
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
                        usuarios.Add(new Transportista
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            tranc_cod_transportista = lector.IsDBNull(lector.GetOrdinal("Username")) ? default(string) : lector.GetString(lector.GetOrdinal("Username")),
                            tranc_vnombre_transportista = lector.IsDBNull(lector.GetOrdinal("Nombre")) ? default(string) : lector.GetString(lector.GetOrdinal("Nombre")),
                            tranc_vdireccion = lector.IsDBNull(lector.GetOrdinal("Apellido")) ? default(string) : lector.GetString(lector.GetOrdinal("Apellido")),
                            tranc_vnumero_telefono = lector.IsDBNull(lector.GetOrdinal("Correo")) ? default(string) : lector.GetString(lector.GetOrdinal("Correo")),
                            tranc_vnum_marca_placa = lector.IsDBNull(lector.GetOrdinal("RolId")) ? default(string) : lector.GetString(lector.GetOrdinal("RolId")),
                            tranc_vnum_licencia_conducir = lector.IsDBNull(lector.GetOrdinal("RolId")) ? default(string) : lector.GetString(lector.GetOrdinal("RolId")),
                            tranc_ruc = lector.IsDBNull(lector.GetOrdinal("RolId")) ? default(string) : lector.GetString(lector.GetOrdinal("RolId")),

                            Estado = lector.IsDBNull(lector.GetOrdinal("Estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Estado")),
                            Cantidad = lector.IsDBNull(lector.GetOrdinal("Cantidad")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return usuarios;
        }
        public Transportista GetById(Transportista entity)
        {
            Transportista transportita = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "UsuarioGetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        transportita = new Transportista
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            tranc_vnombre_transportista = lector.IsDBNull(lector.GetOrdinal("Username")) ? default(string) : lector.GetString(lector.GetOrdinal("Username")),
                            tranc_vdireccion = lector.IsDBNull(lector.GetOrdinal("Password")) ? default(string) : lector.GetString(lector.GetOrdinal("Password")),
                            tranc_vnumero_telefono = lector.IsDBNull(lector.GetOrdinal("ConfirmarPassword")) ? default(string) : lector.GetString(lector.GetOrdinal("ConfirmarPassword")),
                            tranc_ruc = lector.IsDBNull(lector.GetOrdinal("Apellido")) ? default(string) : lector.GetString(lector.GetOrdinal("Apellido")),
                            tranc_vnum_marca_placa = lector.IsDBNull(lector.GetOrdinal("Correo")) ? default(string) : lector.GetString(lector.GetOrdinal("Correo")),
                            tranc_vnum_licencia_conducir = lector.IsDBNull(lector.GetOrdinal("CargoId")) ? default(string) : lector.GetString(lector.GetOrdinal("CargoId")),
                            tranc_cod_transportista = lector.IsDBNull(lector.GetOrdinal("RolId")) ? default(string) : lector.GetString(lector.GetOrdinal("RolId")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("Estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Estado")),
                            tranc_vnum_certif_inscrip = lector.IsDBNull(lector.GetOrdinal("Estado")) ? default(string) : lector.GetString(lector.GetOrdinal("Estado")),

                        };
                    }
                }
            }

            return transportita;
        }
        public int Update(Transportista entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "")))
            {
                _database.AddInParameter(comando, "", DbType.String, entity.tranc_cod_transportista);
                _database.AddInParameter(comando, "", DbType.String, entity.tranc_vnombre_transportista);
                _database.AddInParameter(comando, "", DbType.String, entity.tranc_vdireccion);
                _database.AddInParameter(comando, "", DbType.String, entity.tranc_vnumero_telefono);
                _database.AddInParameter(comando, "", DbType.String, entity.tranc_vnum_marca_placa);
                _database.AddInParameter(comando, "", DbType.String, entity.tranc_vnum_certif_inscrip);
                _database.AddInParameter(comando, "", DbType.Int32, entity.tranc_vnum_licencia_conducir);
                _database.AddInParameter(comando, "", DbType.Int32, entity.tranc_ruc);
                _database.AddInParameter(comando, "", DbType.Int32, entity.Estado);
                _database.AddInParameter(comando, "", DbType.String, entity.UsuarioCreacion);
                _database.AddInParameter(comando, "", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "", DbType.String, entity.Id);
                _database.AddOutParameter(comando, "", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }
        public int GetCorrelativo()
        {
            int correlativo;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "")))
            {
                correlativo = 0;
            }
            return correlativo;
        }




        #endregion
    }
}
