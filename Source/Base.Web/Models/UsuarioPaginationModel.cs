namespace Base.Web.Models
{
    public class UsuarioPaginationModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string RolNombre { get; set; }
        public int Estado { get; set; }
        public int RolId { get; set; }
        public int Cantidad { get; set; }
    }
}