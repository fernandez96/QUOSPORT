using Base.BusinessEntity.Core;
using System;
using System.Collections.Generic;

namespace Base.BusinessEntity
{
    public class NotaSalida : EntityAuditable<int>
    {
        public string nsalc_numero_nota_salida { get; set; }
        public int almac_icod_almacen { get; set; }
        public string almac_vdescripcion { get; set; }
        public int nsalc_iid_motivo { get; set; }
        public string ningc_v_motivo { get; set; }
        public int tdocc_icod_tipo_doc { get; set; }
        public DateTime nsalc_fecha_nota_salida { get; set; }
        public string nsalc_referencia { get; set; }
        public string nsalc_observaciones { get; set; }
        public int status { get; set; }
        public IList<NotaSalidaDetalle> listaDetalleNS { get; set; }

    }
}
