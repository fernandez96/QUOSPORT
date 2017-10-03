using Base.DTO.Core;

namespace Base.DTO
{
    public class ProductoDTO: EntityAuditableDTO<int>
    {
        public int ctgc_iid_categoria { get; set; }
        public int linc_iid_linea { get; set; }
        public int lind_iid_sublinea { get; set; }
        public string prdc_vcod_producto { get; set; }
        public string prdc_vdescripcion { get; set; }
        public int tablc_iid_iclasif_prod { get; set; }
        public decimal prdc_dstock_minimo { get; set; }
        public decimal prdc_npeso_unitario { get; set; }
        public int umec_iid_unidad_medida { get; set; }
        public string prdc_vmaterial1 { get; set; }
        public string prdc_vmaterial2 { get; set; }
        public string prdc_vcodigo_ean { get; set; }
        public string prdc_vcodigo_upc { get; set; }
        public string prdc_vorder_code { get; set; }
        public string prdc_vcolor { get; set; }
        public string prdc_vnumero_serie { get; set; }
        public string prdc_vcaracteristicas { get; set; }
        public int prdc_bflag_estado { get; set; }
        public string prdc_vpc_crea { get; set; }
    }
}
