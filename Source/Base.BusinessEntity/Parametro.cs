using Base.BusinessEntity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.BusinessEntity
{
    public class Parametro : EntityAuditable<int>
    {
        public string empresa { get; set; }
        public string direccion { get; set; }
        public decimal igv { get; set; }
        public string ruc { get; set; }
    }
}
