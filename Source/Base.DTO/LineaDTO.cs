using Base.DTO.Core;

namespace Base.DTO
{
    public class LineaDTO: EntityAuditableDTO<int>
    {
        public string linc_vcod_linea { get; set; }
        public string linc_vdescripcion { get; set; }
        public int status { get; set; }
    }
}
