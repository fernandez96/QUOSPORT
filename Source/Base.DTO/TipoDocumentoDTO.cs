using Base.DTO.Core;

namespace Base.DTO
{
    public class TipoDocumentoDTO: EntityAuditableDTO<int>
    {
        //cabezera
        public int tdocc_icod_tipo_doc { get; set; }
        public string tdocc_vabreviatura_tipo_doc { get; set; }
        public string tdocc_vdescripcion { get; set; }
        public string tdocc_nro_correlativo { get; set; }

        //detalle
        public int tdocd_iid_correlativo { get; set; }
        public int tablc_icod_modulo { get; set; }
    }
}
