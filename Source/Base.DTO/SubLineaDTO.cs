﻿using Base.DTO.Core;

namespace Base.DTO
{
    public class SubLineaDTO: EntityAuditableDTO<int>
    {
        public int linc_iid_linea { get; set; }
        public string lind_vcod_sublinea { get; set; }
        public string lind_vdescripcion { get; set; }
        public int status { get; set; }
        public int idLinea { get; set; }
        public int ctgcc_iid_categoria { get; set; }
    }
}
