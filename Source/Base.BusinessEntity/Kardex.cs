using Base.BusinessEntity.Core;
using System;

namespace Base.BusinessEntity
{
    public class Kardex : EntityAuditable<int>
    {
        public int kardc_ianio { get; set; }
        public DateTime kardc_fecha_movimiento { get; set; }
        public int almac_icod_almacen { get; set; }
        public int prdc_icod_producto { get; set; }
        public decimal kardc_icantidad_prod { get; set; }
        public int tdocc_icod_tipo_doc { get; set; }
        public string kardc_numero_doc { get; set; }
        public int kardc_tipo_movimiento { get; set; }
        public int kardc_iid_motivo { get; set; }
        public string kardc_observaciones { get; set; }
        public int status { get; set; }
        public string strMotivo { get; set; }
        public string strAlmacen { get; set; }
        public string strCodProducto { get; set; }
        public string strProducto { get; set; }
        public decimal dblIngreso { get; set; }
        public decimal dblSalida { get; set; }
        public decimal dblSaldo { get; set; }
        public string strDocumento { get; set; }

    }
}
