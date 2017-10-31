using System;

namespace Base.Web.Models
{
    public class NotaSalidaFilterModel
    {
        public string numeroSearch { get; set; }
        public DateTime fechaInicioSearch { get; set; }
        public DateTime fechaFinSearch { get; set; }
    }
}