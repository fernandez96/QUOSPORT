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

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSPORTISTA_INSERT")))
            {
                _database.AddInParameter(comando, "@tranc_vid_transportista", DbType.String, entity.tranc_vid_transportista);
                _database.AddInParameter(comando, "@tranc_vnombre_transportista", DbType.String, entity.tranc_vnombre_transportista);
                _database.AddInParameter(comando, "@tranc_vdireccion", DbType.String, entity.tranc_vdireccion);
                _database.AddInParameter(comando, "@tranc_vnumero_telefono", DbType.String, entity.tranc_vnumero_telefono);
                _database.AddInParameter(comando, "@tranc_vnum_marca_placa", DbType.String, entity.tranc_vnum_marca_placa);
                _database.AddInParameter(comando, "@tranc_vnum_certif_inscrip", DbType.String, entity.tranc_vnum_certif_inscrip);
                _database.AddInParameter(comando, "@tranc_vnum_licencia_conducir", DbType.String, entity.tranc_vnum_licencia_conducir);
                _database.AddInParameter(comando, "@tranc_vruc", DbType.String, entity.tranc_ruc);
                _database.AddInParameter(comando, "@tranc_flag_estado", DbType.Int32, entity.Estado);
                _database.AddInParameter(comando, "@tranc_vusuario_crea", DbType.String, entity.UsuarioCreacion);
                _database.AddInParameter(comando, "@tranc_vpc_crea", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }
        public int Delete(Transportista entity)
        {
            int idResult;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSPORTISTA_DELETE")))
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
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSPORTISTA_GetAllFilter")))
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
                            tranc_vid_transportista = lector.IsDBNull(lector.GetOrdinal("tranc_vid_transportista")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vid_transportista")),
                            tranc_vnombre_transportista = lector.IsDBNull(lector.GetOrdinal("tranc_vnombre_transportista")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vnombre_transportista")),
                            tranc_vdireccion = lector.IsDBNull(lector.GetOrdinal("tranc_vdireccion")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vdireccion")),
                            tranc_vnumero_telefono = lector.IsDBNull(lector.GetOrdinal("tranc_vnumero_telefono")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vnumero_telefono")),
                            tranc_vnum_marca_placa = lector.IsDBNull(lector.GetOrdinal("tranc_vnum_marca_placa")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vnum_marca_placa")),
                            tranc_vnum_certif_inscrip= lector.IsDBNull(lector.GetOrdinal("tranc_vnum_certif_inscrip")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vnum_certif_inscrip")),
                            tranc_vnum_licencia_conducir = lector.IsDBNull(lector.GetOrdinal("tranc_vnum_licencia_conducir")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vnum_licencia_conducir")),
                            tranc_ruc = lector.IsDBNull(lector.GetOrdinal("tranc_vruc")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vruc")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("tranc_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tranc_flag_estado")),
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
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSPORTISTA_GetById")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);

                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        transportita = new Transportista
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Id")),
                            tranc_vid_transportista = lector.IsDBNull(lector.GetOrdinal("tranc_vid_transportista")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vid_transportista")),
                            tranc_vnombre_transportista = lector.IsDBNull(lector.GetOrdinal("tranc_vnombre_transportista")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vnombre_transportista")),
                            tranc_vdireccion = lector.IsDBNull(lector.GetOrdinal("tranc_vdireccion")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vdireccion")),
                            tranc_vnumero_telefono = lector.IsDBNull(lector.GetOrdinal("tranc_vnumero_telefono")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vnumero_telefono")),
                            tranc_ruc = lector.IsDBNull(lector.GetOrdinal("tranc_vruc")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vruc")),
                            tranc_vnum_marca_placa = lector.IsDBNull(lector.GetOrdinal("tranc_vnum_marca_placa")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vnum_marca_placa")),
                            tranc_vnum_licencia_conducir = lector.IsDBNull(lector.GetOrdinal("tranc_vnum_licencia_conducir")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vnum_licencia_conducir")),
                            Estado = lector.IsDBNull(lector.GetOrdinal("tranc_flag_estado")) ? default(int) : lector.GetInt32(lector.GetOrdinal("tranc_flag_estado")),
                            tranc_vnum_certif_inscrip = lector.IsDBNull(lector.GetOrdinal("tranc_vnum_certif_inscrip")) ? default(string) : lector.GetString(lector.GetOrdinal("tranc_vnum_certif_inscrip")),

                        };
                    }
                }
            }

            return transportita;
        }
        public int Update(Transportista entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_TRANSPORTISTA_UPDATE")))
            {
                _database.AddInParameter(comando, "@tranc_vid_transportista", DbType.String, entity.tranc_vid_transportista);
                _database.AddInParameter(comando, "@tranc_vnombre_transportista", DbType.String, entity.tranc_vnombre_transportista);
                _database.AddInParameter(comando, "@tranc_vdireccion", DbType.String, entity.tranc_vdireccion);
                _database.AddInParameter(comando, "@tranc_vnumero_telefono", DbType.String, entity.tranc_vnumero_telefono);
                _database.AddInParameter(comando, "@tranc_vnum_marca_placa", DbType.String, entity.tranc_vnum_marca_placa);
                _database.AddInParameter(comando, "@tranc_vnum_certif_inscrip", DbType.String, entity.tranc_vnum_certif_inscrip);
                _database.AddInParameter(comando, "@tranc_vnum_licencia_conducir", DbType.String, entity.tranc_vnum_licencia_conducir);
                _database.AddInParameter(comando, "@tranc_vruc", DbType.String, entity.tranc_ruc);
                _database.AddInParameter(comando, "@tranc_flag_estado", DbType.Int32, entity.Estado);
                _database.AddInParameter(comando, "@tranc_vusuario_modifica", DbType.String, entity.UsuarioModificacion);
                _database.AddInParameter(comando, "@tranc_vpc_modifica", DbType.String, WindowsIdentity.GetCurrent().Name);
                _database.AddInParameter(comando, "@id", DbType.Int32, entity.Id);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }
        public int GetCorrelativo()
        {
            int correlativo=0;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_GetCORRELATIBA_TRANSPORTISTA")))
            {
                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        correlativo = lector.IsDBNull(lector.GetOrdinal("Correlativo")) ? default(int) : lector.GetInt32(lector.GetOrdinal("Correlativo")); 
                    }                
                }
            }
            return correlativo;
        }




        #endregion
    }
}
