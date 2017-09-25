namespace Base.DTO.Core
{
    public class EntityAuditableDTO<T> : EntityBaseDTO<T>
    {
        public string UsuarioRegistro { get; set; }
    }
}
