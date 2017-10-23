using System;

namespace Base.Web.Models
{
    public class NotaIngresoFilterModel
    {
        public string numeroSearch { get; set; }
        public DateTime fechaInicioSearch { get; set; }
        public DateTime fechaFinSearch { get; set; }
    }
}