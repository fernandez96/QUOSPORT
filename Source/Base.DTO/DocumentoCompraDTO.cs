using System;

namespace Base.DTO
{
    public class DocumentoCompraDTO
    {
        public int proc_icod_proveedor { get; set; }
        public int tdocc_icod_tipo_doc { get; set; }
        public string facc_num_doc { get; set; }
        public DateTime facc_sfecha_doc { get; set; }
        public int almac_icod_almacen { get; set; }
        public DateTime facc_svencimiento { get; set; }
        public int tablc_iid_tipo_moneda { get; set; }
        public int facc_iforma_pago { get; set; }
        public string facc_vobservaciones { get; set; }
        public decimal facc_nporcent_imp_doc { get; set; }
        public decimal facc_nmonto_neto_doc { get; set; }
        public decimal facc_nmonto_imp { get; set; }
        public decimal facc_nmonto_total_doc { get; set; }
    }
}
