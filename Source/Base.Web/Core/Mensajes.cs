using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Base.Web.Core
{
    public class Mensajes
    {
        public static string IntenteloMasTarde = "Hubo un error, inténtelo más tarde.";
        public static string UsuarioNoExiste = "El usuario no pertenece al sistema o contraseña incorrecta.";
      
        public static string MapaCampanaNoExiste = "El mapa de campaña no pertenece al sistema.";
        public static string ConfiguracionNoExiste = "La configuración no pertenece al sistema.";
        public static string RegistroSatisfactorio = "Se realizó el registro satisfactoriamente.";
        public static string ContraseñaNoConsiden = "No son iguales las contraseñas.";
        public static string RegistroFallido = "No se pudo realizar el registro.";
        public static string ActualizacionSatisfactoria = "Se realizó la actualización satisfactoriamente.";
        public static string ActualizacionFallida = "No se pudo realizar la actualización.";
        public static string EliminacionSatisfactoria = "Se realizó la eliminación satisfactoriamente.";
        public static string EliminacionFallida = "No se pudo realizar la eliminación.";
        public static string NoExisteRegistro = "No existe el registro solicitado.";
        //public static string YaExisteRegistro = "Ya existe un registro con estos mismo datos, por lo tanto no se puede registrar";
        public static string YaExisteRegistro = "El registro ya existe.";
        public static string CredencialesDominioIncorrectas = "Las credenciales de dominio son incorrectas.";
        public static string AccesoAlSistema = "Acceso al sistema";
        public static string NoEncontraronDatos = "No se encontraron datos.";
        public static string EncontraronDatos = "Se encontraron datos.";
        public static string CargaYaQuemado = "No se pudo modificar porque ya se ha quemado.";
        public static string CargaFallaEliminar = "No se puede eliminar debido a que no existe dicho registro o ya se ha Quemado esta giftcard.";
        public static string CanjeNoDisponible = "No hay GiftCard disponible para quemar con este tipo de producto.";
        public static string CorreoEnviado = "Correo enviado";
        public static string CorreoNoEnviado = "No se puede reenviar correo";
        public static string ValeRelacionado = "No se puede Actualizar debido a que hay registros relacionados a este registro";
        public static string OpenXmlFormats = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,";
        public static string cambiostatus = "Se realizo el cambio de estado satisfactoriamente.";

        public static string Add = "Add";
        public static string Delete = "Delete";
        public static string Login = "Login";
        public static string Update = "Update";
        public static string Upload = "Upload";

        public static string AccountController = "Account";
        public static string MapaCampanaController = "MapaController";
        public static string UsuarioController = "Usuario";
        public static string EnvioEmailController = "EnvioEmail";
        public static string ConfiguracionController = "Configuracion";
    }
}