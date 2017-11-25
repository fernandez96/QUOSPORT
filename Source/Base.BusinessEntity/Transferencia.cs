using Base.BusinessEntity.Core;
using System;
using System.Collections.Generic;

namespace Base.BusinessEntity
{
    public class Transferencia: EntityAuditable<int>
    {
        public string trfc_inum_transf { get; set; }
        public DateTime trfc_sfecha_transf { get; set; }
        public string trfc_sfecha_transf_ { get; set; }
        public int almac_icod_alm_sal { get; set; }
        public int almac_icod_alm_ing { get; set; }
        public string almac_v_alm_sal { get; set; }
        public string almac_v_alm_ing { get; set; }
        public int trnfc_iid_motivo { get; set; }
        public string trnfc_v_motivo { get; set; }
        public string trnfc_vobservaciones { get; set; }
        public int status { get; set; }
        public IList<TransferenciaDetalle> transferenciaDetalle { get; set; }

    }
}
