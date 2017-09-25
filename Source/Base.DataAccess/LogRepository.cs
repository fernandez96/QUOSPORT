using Base.BusinessEntity;
using Base.Common.Generics;
using Base.DataAccess.Core;
using Base.DataAccess.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;

namespace Base.DataAccess
{
    public class LogRepository:Singleton<LogRepository>,ILogRepository<Log,int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

        #region Métodos Públicos

        public int Add(Log entity)
        {
            int id;

            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "LogInsert")))
            {
                _database.AddInParameter(comando, "@Usuario", DbType.String, entity.Usuario);
                _database.AddInParameter(comando, "@Mensaje", DbType.String, entity.Mensaje);
                _database.AddInParameter(comando, "@Controlador", DbType.String, entity.Controlador);
                _database.AddInParameter(comando, "@Accion", DbType.String, entity.Accion);
                _database.AddInParameter(comando, "@Objeto", DbType.String, entity.Objeto);
                _database.AddInParameter(comando, "@Identificador", DbType.Int32, entity.Identificador);
                _database.AddOutParameter(comando, "@Response", DbType.Int32, 11);

                _database.ExecuteNonQuery(comando);
                id = Convert.ToInt32(_database.GetParameterValue(comando, "@Response"));
            }

            return id;
        }

        #endregion
    }
}
