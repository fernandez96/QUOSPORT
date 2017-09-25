using Base.DTO.Core;

namespace Base.DTO
{
    public  class ParametroDTO: EntityAuditableDTO<int>
    {
        public string empresa { get; set; }
        public string direccion { get; set; }
        public decimal igv { get; set; }
        public string ruc { get; set; }
    }
}
