using Base.BusinessEntity;
using Base.Web.Models;
using Base.Web.Utilities;
using System.Collections.Generic;
using System.Web;

namespace Base.Web.Core
{
    public  static class WebSession
    {
        public static UsuarioModel Usuario
        {
            get { return HttpContext.Current.Session[ConstantesWeb.UsuarioSesion] as UsuarioModel; }
            set { HttpContext.Current.Session.Add(ConstantesWeb.UsuarioSesion, value); }
        }

        public static IEnumerable<Formulario> Formularios
        {
            get { return HttpContext.Current.Session[ConstantesWeb.FormulariosSesion] as IEnumerable<Formulario>; }
            set { HttpContext.Current.Session.Add(ConstantesWeb.FormulariosSesion, value); }
        }

        public static IEnumerable<Formulario> Roles
        {
            get { return HttpContext.Current.Session[ConstantesWeb.FormulariosSesion] as IEnumerable<Formulario>; }
            set { HttpContext.Current.Session.Add(ConstantesWeb.FormulariosSesion, value); }
        }

        public static Formulario FormularioActual
        {
            get { return HttpContext.Current.Session[ConstantesWeb.FormularioActualSesion] as Formulario; }
            set { HttpContext.Current.Session.Add(ConstantesWeb.FormularioActualSesion, value); }
        }
    }
}