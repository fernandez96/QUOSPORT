using Base.BusinessEntity.Core;

namespace Base.BusinessEntity
{
    public  class Transportista: EntityAuditable<int>
    {

        public string tranc_vid_transportista { get; set; }
        public string tranc_vnombre_transportista { get; set; }
        public string tranc_vdireccion  { get; set; }
        public string tranc_vnumero_telefono { get; set; }
        public string tranc_vnum_marca_placa { get; set; }
        public string tranc_vnum_certif_inscrip { get; set; }
        public string tranc_vnum_licencia_conducir { get; set; }
        public string tranc_ruc { get; set; }
    }
}
