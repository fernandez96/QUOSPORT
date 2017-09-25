using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DTO
{
   public class ReporteDTO
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
