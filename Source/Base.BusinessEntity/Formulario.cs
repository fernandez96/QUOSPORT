using System.Collections.Generic;

namespace Base.BusinessEntity
{
    public class Formulario
    {
        public int Id { get; set; }
        public string Direccion { get; set; }
        public string Nombre { get; set; }
        public string Icono { get; set; }
        public int Orden { get; set; }
        public int? FormularioParentId { get; set; }
        public int RolId { get; set; }
        public int idModulo { get; set; }
        public List<Formulario> FormulariosHijosList { get; set; }

        public string Area
        {
            get
            {
                var partes = Direccion.Split('/');
                if (partes.Length == 4) return partes[1];
                return string.Empty;
            }
        }

        public string Controlador
        {
            get
            {
                var partes = Direccion.Split('/');
                if (partes.Length == 4) return partes[2];
                return string.Empty;
            }
        }

        public string Accion
        {
            get
            {
                var partes = Direccion.Split('/');
                if (partes.Length == 4) return partes[3];
                return string.Empty;
            }
        }
    }
}
