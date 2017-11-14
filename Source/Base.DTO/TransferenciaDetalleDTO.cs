using Base.DTO.Core;

namespace Base.DTO
{
    public class TransferenciaDetalleDTO: EntityAuditableDTO<int>
    {

        public int trfc_icod_transf { get; set; }
        public string trfd_nro_item { get; set; }
        public int prdc_icod_producto { get; set; }
        public int kardc_icod_correlativo_sal { get; set; }
        public int kardc_icod_correlativo_ing { get; set; }
        public decimal trfd_ncantidad { get; set; }
    }
}
