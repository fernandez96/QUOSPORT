using Base.BusinessEntity.Core;

namespace Base.BusinessEntity
{
    public class Usuario: EntityAuditable<int>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmarPassword { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public int CargoId { get; set; }
        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
