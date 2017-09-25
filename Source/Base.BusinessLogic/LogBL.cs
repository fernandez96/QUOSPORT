using Base.BusinessEntity;
using Base.BusinessLogic.Interfaces;
using Base.Common.Generics;
using Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessLogic
{
   public class LogBL:Singleton<LogBL>,ILogBL<Log,int>
    {
        public int Add(Log entity)
        {
            return LogRepository.Instancia.Add(entity);
        }
    }
}
