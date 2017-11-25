using Base.BusinessEntity;
using Base.DTO.Core;
using System;
using System.Collections.Generic;

namespace Base.DTO
{
    public class TransferenciaDTO : EntityAuditableDTO<int>
    {
        public string trfc_inum_transf { get; set; }
        public DateTime trfc_sfecha_transf { get; set; }
        public string trfc_sfecha_transf_ { get; set; }
        public string fecha { get; set; }
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
