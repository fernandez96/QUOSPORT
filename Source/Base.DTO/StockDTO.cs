namespace Base.DTO
{
    public class StockDTO
    {
        public int Id { get; set; }
        public int almac_icod_almacen { get; set; }
        public int prdc_icod_producto { get; set; }
        public decimal stocc_stock_producto { get; set; }
        public int Estado { get; set; }
    }
}
