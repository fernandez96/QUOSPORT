using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Base.Web.Core
{
    public class appSettings
    {
        public static string FormatoFecha { get { return ConfigurationManager.AppSettings.Get("formatoFecha"); } }
        public static string FormatoSeachFecha { get { return ConfigurationManager.AppSettings.Get("formatoSeachFecha"); } }
    }
}