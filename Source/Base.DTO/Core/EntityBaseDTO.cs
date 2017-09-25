namespace Base.DTO.Core
{
    public class EntityBaseDTO<T>
    {
        public T Id { get; set; }
        public int Estado { get; set; }
        public T Cantidad { get; set; }
    }
}
