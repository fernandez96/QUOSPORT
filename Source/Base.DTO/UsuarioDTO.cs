using Base.DTO.Core;

namespace Base.DTO
{
    public class UsuarioDTO: EntityAuditableDTO<int>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmarPassword { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public int CargoId { get; set; }
        public int RolId { get; set; }
        public string RolNombre { get; set; }
    }
}
