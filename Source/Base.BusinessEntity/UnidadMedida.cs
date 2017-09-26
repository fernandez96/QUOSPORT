using Base.BusinessEntity.Core;

namespace Base.BusinessEntity
{
    public class UnidadMedida: EntityAuditable<int>
    {
        public string umec_vcod_unidad_medida { get; set; }
        public string umec_vdescripcion { get; set; }

    }
}
