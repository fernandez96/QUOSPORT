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
   public class StatusRepository: Singleton<StatusRepository>, IStatusRepository<Status, int>
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

        #region Métodos Públicos
        public int status(Status entity)
        {
            int id;
            using (var comando = _database.GetStoredProcCommand(string.Format("{0}{1}", ConectionStringRepository.EsquemaName, "SGE_STATUS")))
            {
                _database.AddInParameter(comando, "@tabla", DbType.String, entity.tabla);
                _database.AddInParameter(comando, "@estado", DbType.String, entity.setStatus);
                _database.AddInParameter(comando, "@where", DbType.String, entity.where);

              id =_database.ExecuteNonQuery(comando);
           
            }

            return id;
        }



        #endregion
    }
}
