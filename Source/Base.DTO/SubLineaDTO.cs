using Base.DTO.Core;

namespace Base.DTO
{
    public class SubLineaDTO: EntityAuditableDTO<int>
    {
        public string lind_vcod_sublinea { get; set; }
        public string lind_vdescripcion { get; set; }
        public int status { get; set; }
    }
}
