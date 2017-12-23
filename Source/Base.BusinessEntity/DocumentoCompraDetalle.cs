namespace Base.BusinessEntity
{
    public  class DocumentoCompraDetalle
    {
        public int facc_icod_doc { get; set; }
        public string facd_iitem { get; set; }
        public int prd_icod_producto { get; set; }
        public decimal facd_ncantidad { get; set; }
        public decimal facd_nmonto_unit { get; set; }
        public int facd_nmonto_total { get; set; }
        public int facd_icod_kardex { get; set; }
        public string facd_vdescripción_item { get; set; }
    }
}
