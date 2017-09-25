using System;

namespace Base.BusinessEntity.Core
{
    public class EntityAuditable<T>: EntityBase<T>
    {
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
