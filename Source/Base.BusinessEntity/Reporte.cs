using Base.BusinessEntity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessEntity
{
   public class Reporte: EntityAuditable<int>
    {
        public int Anio { get; set; }
        public string Mes { get; set; }
        public string Cliente { get; set; }
        public decimal Monto { get; set; }
        public int CantidadRows { get; set; }


    }
}
