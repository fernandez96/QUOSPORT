using Base.DTO.Core;
namespace Base.DTO
{
    public class NotaSalidaDetalleDTO : EntityAuditableDTO<int>
    {
        public int nsalc_icod_nota_salida { get; set; }
        public string nsald_nro_item { get; set; }
        public int prdc_icod_producto { get; set; }
        public decimal nsald_cantidad { get; set; }
        public string prdc_vdescripcion { get; set; }
        public int kardc_icod_correlativo { get; set; }
        public int kardc_tipo_movimiento { get; set; }
        public int status { get; set; }
        public string dninc_v_unidad { get; set; }
    }
}
