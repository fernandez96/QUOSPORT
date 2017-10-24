using Base.BusinessEntity;
using Base.DTO.Core;
using System;
using System.Collections.Generic;

namespace Base.DTO
{
    public class NotaIngresoDTO: EntityAuditableDTO<int>
    {
        public string ningc_numero_nota_ingreso { get; set; }
        public int almac_icod_almacen { get; set; }
        public string almac_vdescripcion { get; set; }
        public int ningc_iid_motivo { get; set; }
        public string ningc_v_motivo { get; set; }
        public string fecha { get; set; }
        public DateTime ningc_fecha_nota_ingreso { get; set; }
        public int tdocc_icod_tipo_doc { get; set; }
        public string ningc_numero_doc { get; set; }
        public string ningc_observaciones { get; set; }
        public string ningc_referencia { get; set; }
        public List<NotaIngresoDetalle> listaDetalleNI { get; set; }
        public List<Kardex> listaKardex { get; set; }
        public List<Stock> listastock { get; set; }
    }
}
