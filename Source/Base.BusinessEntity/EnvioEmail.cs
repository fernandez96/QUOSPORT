using Base.BusinessEntity.Core;

namespace Base.BusinessEntity
{
    public class EnvioEmail: EntityAuditable<int>
    {
        public long Id_ { get; set; }
        public string correo { get; set; }

    }
}
