﻿using Base.DTO.Core;

namespace Base.DTO
{
    public class UnidadMedidaDTO: EntityAuditableDTO<int>
    {
        public string umec_vcod_unidad_medida { get; set; }
        public string umec_vdescripcion { get; set; }

    }
}
