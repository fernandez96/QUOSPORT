using Base.BusinessEntity.Core;
using System;

namespace Base.BusinessEntity
{
    public class Transferencia: EntityAuditable<int>
    {
        public string trfc_inum_transf { get; set; }
        public DateTime trfc_sfecha_transf { get; set; }
        public int almac_icod_alm_sal { get; set; }
        public int almac_icod_alm_ing { get; set; }
        public int trnfc_iid_motivo { get; set; }
        public string trnfc_vobservaciones { get; set; }
        public int status { get; set; }

    }
}
