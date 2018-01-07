using Base.DTO.Core;

namespace Base.DTO
{
    public class TransportistaDTO: EntityAuditableDTO<int>
    {
        public string tranc_vid_transportista { get; set; }
        public string tranc_vnombre_transportista { get; set; }
        public string tranc_vdireccion { get; set; }
        public string tranc_vnumero_telefono { get; set; }
        public string tranc_vnum_marca_placa { get; set; }
        public string tranc_vnum_certif_inscrip { get; set; }
        public string tranc_vnum_licencia_conducir { get; set; }
        public int tranc_iid_situacion_transporte { get; set; }
        public string tranc_ruc { get; set; }
    }
}
