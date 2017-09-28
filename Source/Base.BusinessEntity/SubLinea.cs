using Base.BusinessEntity.Core;

namespace Base.BusinessEntity
{
    public class SubLinea : EntityAuditable<int>
    {
        public string lind_vcod_sublinea { get; set; }
        public string lind_vdescripcion { get; set; }
        public int status { get; set; }

    }
}
