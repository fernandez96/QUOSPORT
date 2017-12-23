using Base.BusinessEntity.Core;
using System;

namespace Base.BusinessEntity
{
    public class Proveedor: EntityAuditable<int>
    {
        public string proc_icod_proveedor { get; set; }
        public string proc_vcod_proveedor { get; set; }
        public int proc_situacion { get; set; }
        public int proc_tipo_persona { get; set; }
        public DateTime proc_sfecha { get; set; }
        public string proc_tipo_doc { get; set; }
        public string proc_vcomercial { get; set; }
        public string proc_vnombre { get; set; }
        public string proc_vpaterno { get; set; }
        public string proc_vmaterno { get; set; }
        public string proc_vdireccion { get; set; }
        public string proc_vtelefono { get; set; }
        public string proc_vruc { get; set; }
        public string proc_vfax { get; set; }
        public string proc_vcorreo { get; set; }
        public string proc_vrepresentante { get; set; }
  
    }
}
