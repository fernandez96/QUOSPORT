using Base.BusinessEntity.Core;

namespace Base.BusinessEntity
{
    public class Rol:EntityBase<int>
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
