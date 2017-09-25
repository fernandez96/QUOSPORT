using Base.DTO.Core;

namespace Base.DTO
{
    public class EnvioEmailDTO: EntityAuditableDTO<int>
    {
        public long Id_ { get; set; }
        public string correo { get; set; }
        public string FileCarga { get; set; }

    }
}
