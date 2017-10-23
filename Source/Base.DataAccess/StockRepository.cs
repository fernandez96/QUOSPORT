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
   public class StockRepository
    {
        #region Attributos

        private readonly Database _database = new DatabaseProviderFactory().Create(ConectionStringRepository.ConnectionStringNameSQL);

        #endregion

    }
}
