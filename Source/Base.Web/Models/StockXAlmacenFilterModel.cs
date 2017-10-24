using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Base.Web.Models
{
    public class StockXAlmacenFilterModel
    {
        public string almacenSearch { get; set; }
        public string FechaInicialSearch { get; set; }
        public string FechaFinalSearch { get; set; }
        public string codigoSearch { get; set; }
        public string descripcionSearch { get; set; }
    }
}