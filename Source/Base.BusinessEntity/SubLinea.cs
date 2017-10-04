using Base.BusinessEntity.Core;

namespace Base.BusinessEntity
{
    public class SubLinea : EntityAuditable<int>
    {
        public int linc_iid_linea { get; set; }
        public string lind_vcod_sublinea { get; set; }
        public string lind_vdescripcion { get; set; }
        public int status { get; set; }
        public int idLinea { get; set; }

    }
}
