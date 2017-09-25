using Base.BusinessEntity;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;

namespace Base.DataAccess
{
    public class ParametroRepository :Singleton<ParametroRepository>, IParametroRepository<Parametro,int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

        #region metodos publicos
         public int Add(Parametro entity)
        {
            int id;
            using (var comando= _database.GetStoredProcCommand(string.Format("{0}{1}",ConectionStringRepository.EsquemaName, "SGE_PARAMETRO_INSERT")))
            {
                _database.AddInParameter(comando, "@parm_nombre_empresa", DbType.String, entity.empresa);
                _database.AddInParameter(comando, "@parm_direccion_empresa", DbType.String, entity.direccion);
                _database.AddInParameter(comando, "@parm_nigv_parametro", DbType.Decimal, entity.igv);
                _database.AddInParameter(comando, "@parm_iusuario_crea", DbType.String, entity.UsuarioCreacion);
                _database.AddInParameter(comando, "@parm_vruc", DbType.String, entity.ruc);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);
                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Reponse"));
            }

            return id;
        }

        public int Update(Parametro entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_PARAMETRO_UPDATE")))
            {
                _database.AddInParameter(comando, "@Id", DbType.Int32, entity.Id);
                _database.AddInParameter(comando, "@parm_nombre_empresa", DbType.String, entity.empresa);
                _database.AddInParameter(comando, "@parm_direccion_empresa", DbType.String, entity.direccion);
                _database.AddInParameter(comando, "@parm_vruc", DbType.String, entity.ruc);
                _database.AddInParameter(comando, "@parm_nigv_parametro", DbType.Decimal, entity.igv);
                _database.AddInParameter(comando, "@parm_iusuario_modificar", DbType.String, entity.UsuarioModificacion);
              
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }

        public Parametro GetAll()
        {
            Parametro parametro = null;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_PARAMETRO_INSERT_GetAllActives")))
            {
                using (var lector = _database.ExecuteReader(comando))
                {
                    if (lector.Read())
                    {
                        parametro = new Parametro
                        {
                            Id = lector.IsDBNull(lector.GetOrdinal("parm_icod_parametro")) ? default(int) : lector.GetInt32(lector.GetOrdinal("parm_icod_parametro")),
                            empresa = lector.IsDBNull(lector.GetOrdinal("parm_nombre_empresa")) ? default(string) : lector.GetString(lector.GetOrdinal("parm_nombre_empresa")),
                            direccion = lector.IsDBNull(lector.GetOrdinal("parm_direccion_empresa")) ? default(string) : lector.GetString(lector.GetOrdinal("parm_direccion_empresa")),
                            igv = lector.IsDBNull(lector.GetOrdinal("parm_nigv_parametro")) ? default(decimal) : lector.GetDecimal(lector.GetOrdinal("parm_nigv_parametro")),
                            ruc = lector.IsDBNull(lector.GetOrdinal("parm_vruc")) ? default(string) : lector.GetString(lector.GetOrdinal("parm_vruc"))
                          
                        };
                    }
                }
            }

            return parametro;
        }
        #endregion
    }
}
