﻿using System.Configuration;

namespace Base.Web.Utilities
{
    public class ConstantesWeb
    {
        #region WebServices

        #endregion

        #region KeyString

        public const string UsuarioSesion = "UsuarioSesion";
        public const string NoUsuario = "NoUsuario";
        public const string FormulariosSesion = "FormulariosSesion";
        public const string FormularioActualSesion = "FormularioActualSesion";
        public const string TimeOutSession = "TimeOutSession";
        public const string EmailPattern = "EmailPattern";

        public const string LoginController = "Account";
        public const string LoginAction = "Login";        

        public const string HomeController = "Home";
        public const string HomeAction = "Index";

        public const string ErrorController = "Error";
        public const string NotFoundAction = "NotFound";
        public const string ServerErrorAction = "ServerError";

        public const string FormatoFechaPorDefecto = "dd/MM/yyyy";
        public const string FormatoFechaHoraPorDefecto = "dd/MM/yyyy hh:mm";
        public const string FormatoHoraPorDefecto = "HH:mm:ss";
        public const string FormatoMonedaPorDefecto = "N2";
        public const string FormatoDecimalesPorDefecto = "{0:N2}";

        public const int Unauthorized = 1;

        public const int Error2146233087 = -2146233087;

        #endregion

        #region MethodType

        public const string METHODPOST = "POST";
        public const string METHODGET = "GET";

        #endregion

        #region Mensajes

        public static string IntenteloMasTarde = "Hubo un error, inténtelo más tarde";
        public static string CredencialesDominioIncorrectas = "Las credenciales de dominio son incorrectas";
        public static string SeTerminoLaSession = "Se terminó la sesión";
        public static string SesionTerminada = "Sesión Terminada";

        #endregion

        #region Roles

        public const int RolAdministrador = 1;
        public const int Rolcontabilidad = 2;
        public const int RolAlmacen = 2;
        public const int RolOperador = 3;


        #endregion

        #region acciones
        public const int add = 1;
        public const int update = 2;
        public const int delete = 3;
        #endregion
    }
}