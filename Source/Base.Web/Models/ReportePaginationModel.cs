using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Base.Web.Models
{
    public class ReportePaginationModel
    {
        public int Id { get; set; }
        public int Anio { get; set; }
        public string Mes { get; set; }
        public string Cliente { get; set; }
        public decimal Monto { get; set; }
        public int Cantidad { get; set; }
        public int CantidadRows { get; set; }
    }
}