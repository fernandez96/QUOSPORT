using Base.BusinessEntity.Core;

namespace Base.BusinessEntity
{
    public class Almacen: EntityAuditable<int>
    {
        public string almac_vcod_almacen { get; set; }
        public string almac_vdescripcion { get; set; }
        public string almac_vubicacion { get; set; }
        public string almac_vresponsable { get; set; }
        public string almac_vtelefono { get; set; }
        public string almac_vcorreo { get; set; }
        public int almac_itipo { get; set; }
        public string almac_vtipo { get; set; }
    }
}
