using Base.DTO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.DTO
{
   public class CategoriaDTO : EntityAuditableDTO<int>
    {
        public string ctgcc_vcod_categoria { get; set; }
        public string ctgcc_vdescripcion { get; set; }
        public IList<LineaDTO> detalleLinea { get; set; }
        public IList<SubLineaDTO> detalleSubLinea { get; set; }
        public int correlativaCab { get; set; }
    }
}
