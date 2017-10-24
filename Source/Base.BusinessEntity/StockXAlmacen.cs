namespace Base.BusinessEntity
{
    public class StockXAlmacen
    {
        public decimal stockactual { get; set; }
        public string prdc_vdescripcion { get; set; }
        public string almac_vdescripcion { get; set; }
        public string umec_vdescripcion { get; set; }
        public int prdc_iid_producto { get; set; }
        public int Cantidad { get; set; }
    }
}
