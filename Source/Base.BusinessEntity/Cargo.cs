using Base.BusinessEntity.Core;

namespace Base.BusinessEntity
{
    public class Cargo: EntityAuditable<int>
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
