using Base.DTO.Core;

namespace Base.DTO
{
    public class ClienteDTO: EntityAuditableDTO<int>
    {
        public string cliec_vcod_cliente { get; set; }
        public int tablc_iid_tipo_cliente { get; set; }
        public string cliec_sfecha_registro_cliente { get; set; }
        public int tabl_iid_tipo_documento { get; set; }
        public string cliec_vnumero_doc_cli { get; set; }
        public string cliec_vapellido_paterno { get; set; }
        public string cliec_vapellido_materno { get; set; }
        public string cliec_vnombres { get; set; }
        public int tabl_iid_giro { get; set; }
        public string cliec_rozon_social_cliente { get; set; }
        public string cliec_vnombre_establecimiento { get; set; }
        public string cliec_cruc { get; set; }
        public string cliec_vdireccion_cliente { get; set; }
        public string cliec_vnro_telefono { get; set; }
        public string cliec_vnro_fax { get; set; }
        public string cliec_vnro_celular { get; set; }
        public string cliec_vcorreo_electronico { get; set; }
        public string cliec_vnombre_representante { get; set; }
    }
}
