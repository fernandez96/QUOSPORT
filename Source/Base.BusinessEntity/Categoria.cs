using Base.BusinessEntity.Core;
using System.Collections.Generic;

namespace Base.BusinessEntity
{
    public class Categoria: EntityAuditable<int>
    {
        public string ctgcc_vcod_categoria { get; set; }
        public string ctgcc_vdescripcion { get; set; }
        public IList<Linea> detalleLinea { get; set; }
        public IList<SubLinea> detalleSubLinea { get; set; }
    }
}
