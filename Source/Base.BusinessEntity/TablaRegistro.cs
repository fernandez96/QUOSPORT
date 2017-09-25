using Base.BusinessEntity.Core;

namespace Base.BusinessEntity
{
    public class TablaRegistro: EntityAuditable<int>
    {
        //Cabezera
        public string tbpc_vcod_tabla_opciones { get; set; }
        public string tbpc_vdescripcion { get; set; }
        public int correlativaCab { get; set; }


        //detalle
        public string tbpd_vcod_tabla_opciones_det { get; set; }
        public string tbpd_vdescripcion_detalle { get; set; }
        public int tbpc_iid_tabla_opciones { get; set; }
        public int correlativaDet { get; set; }

    }
}
