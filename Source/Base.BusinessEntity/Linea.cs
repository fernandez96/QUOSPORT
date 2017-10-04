using Base.BusinessEntity.Core;

namespace Base.BusinessEntity
{
    public class Linea: EntityAuditable<int>
    {
        public int ctgcc_iid_categoria { get; set; }
        public string linc_vcod_linea { get; set; }
        public string linc_vdescripcion { get; set; }
        public int status { get; set; }
    }
}
