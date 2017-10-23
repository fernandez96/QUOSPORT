using Base.BusinessEntity.Core;

namespace Base.BusinessEntity
{
    public class NotaIngresoDetalle: EntityAuditable<int>
    {
        public int ningc_icod_nota_ingreso { get; set; }
        public string dninc_nro_item { get; set; }
        public int prdc_icod_producto { get; set; }
        public decimal dninc_cantidad { get; set; }
        public int kardc_icod_correlativo { get; set; }
        public int kardc_tipo_movimiento { get; set; }
        public decimal dninc_costo { get; set; }
        public int status { get; set; }
        public string dninc_v_unidad { get; set; }
    }
}
