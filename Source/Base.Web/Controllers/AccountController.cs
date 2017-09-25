using Base.ActiveDirectory;
using Base.BusinessEntity;
using Base.BusinessLogic;
using Base.Common;
using Base.DTO;
using Base.DTO.AutoMapper;
using Base.Web.ApiService;
using Base.Web.Core;
using Base.Web.Models;
using Base.Web.Utilities;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Base.Web.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        private static readonly ILog logger = LogManager.GetLogger(string.Empty);

        [AllowAnonymous]
        public ActionResult Login()
        {
            var modelo = new AccountModel
            {
                Username = "",
                Password = ""
            };
            return View(modelo);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(AccountModel accountModel)
        {
            try
            {
                var jsonResponse = new JsonResponse { Success = false };
                accountModel.ValidacionAD = ConfigurationAppSettings.ValidacionAD();
                jsonResponse = Login_(accountModel);

                if (jsonResponse.Success && !jsonResponse.Warning)
                {

                    return RedirectToAction(ConstantesWeb.HomeAction, ConstantesWeb.HomeController);
                }
                else if (jsonResponse.Warning)
                {
                    ViewBag.MessageError = jsonResponse.Message;
                }
            }
            catch (Exception exception)
            {
                logger.Error(string.Format("Mensaje: {0} Trace: {1}", exception.Message, exception.StackTrace));
                ViewBag.MessageError = exception.Message;
            }
            return View(accountModel);
        }

        public ActionResult LogOut()
        {
            LimpiarAutenticacion();

            return RedirectToAction(ConstantesWeb.LoginAction);
        }

        [HttpPost]
        public JsonResult VerifySession()
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                HttpSessionStateBase session = HttpContext.Session;
                if (session.Contents[ConstantesWeb.UsuarioSesion] == null)
                {
                    jsonResponse.Title = ConstantesWeb.SesionTerminada;
                    jsonResponse.Warning = true;
                    jsonResponse.Message = ConstantesWeb.SeTerminoLaSession;
                }
            }
            catch (Exception exception)
            {
                logger.Error(string.Format("Mensaje: {0} Trace: {1}", exception.Message, exception.StackTrace));
                jsonResponse.Success = false;
                jsonResponse.Message = ConstantesWeb.IntenteloMasTarde;
            }

            return Json(jsonResponse);
        }


        public JsonResponse Login_(AccountModel loginDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                if (loginDTO.ValidacionAD)
                {
                    UsuarioAD usuarioAD = new UsuarioAD();
                    if (usuarioAD.AutenticarEnDominio(loginDTO.Username, loginDTO.Password))
                    {
                        string passwordencriptado = Seguridad.Encriptar(loginDTO.Password.Trim());
                        loginDTO.Password = passwordencriptado;
                        var usuario = UsuarioBL.Instancia.GetByUsername(loginDTO.Username, loginDTO.Password);
                        if (usuario != null)
                        {
                            var usuarioLoginDTO = MapperHelper.Map<Usuario, UsuarioLoginDTO>(usuario);
                            jsonResponse.Data = usuarioLoginDTO;

                            GenerarTickectAutenticacion(usuarioLoginDTO);

                            LogBL.Instancia.Add(new Log
                            {
                                Accion = Mensajes.Login,
                                Controlador = Mensajes.AccountController,
                                Identificador = usuario.Id,
                                Mensaje = Mensajes.AccesoAlSistema,
                                Usuario = usuario.Username,
                                Objeto = JsonConvert.SerializeObject(usuario)
                            });
                        }
                        else
                        {
                            jsonResponse.Warning = true;
                            jsonResponse.Message = Mensajes.UsuarioNoExiste;
                        }
                    }
                    else
                    {
                        jsonResponse.Warning = true;
                        jsonResponse.Message = Mensajes.CredencialesDominioIncorrectas;
                    }
                }
                else
                {
                    string passwordencriptado = Seguridad.Encriptar(loginDTO.Password.Trim());
                    loginDTO.Password = passwordencriptado;
                    var usuario = UsuarioBL.Instancia.GetByUsername(loginDTO.Username, loginDTO.Password);
                    if (usuario != null)
                    {
                        var usuarioLoginDTO = MapperHelper.Map<Usuario, UsuarioLoginDTO>(usuario);
                        jsonResponse.Data = usuarioLoginDTO;
                        GenerarTickectAutenticacion(usuarioLoginDTO);
                        LogBL.Instancia.Add(new Log
                        {
                            Accion = Mensajes.Login,
                            Controlador = Mensajes.AccountController,
                            Identificador = usuario.Id,
                            Mensaje = Mensajes.AccesoAlSistema,
                            Usuario = usuario.Username,
                            Objeto = JsonConvert.SerializeObject(usuario)
                        });
                    }
                    else
                    {
                        jsonResponse.Warning = true;
                        jsonResponse.Message = Mensajes.UsuarioNoExiste;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                jsonResponse.Success = false;
                jsonResponse.Message = Mensajes.IntenteloMasTarde;
            }

            return jsonResponse;
        }

        #region Metodos Privados

        private void GenerarTickectAutenticacion(UsuarioLoginDTO u)
        {
            UsuarioModel usuarioModel = new UsuarioModel();
            usuarioModel.Username = u.Username;
            usuarioModel.RolId = u.RolId;
            usuarioModel.RolNombre = u.RolNombre;
            usuarioModel.TimeZoneId = ConfigurationAppSettings.TimeZoneId();
            usuarioModel.TimeZoneGMT = ConfigurationAppSettings.TimeZoneGMT();

            AuthenticationHelper.CreateAuthenticationTicket(usuarioModel.Username, usuarioModel.TimeZoneId);

            WebSession.Usuario = usuarioModel;
            WebSession.Formularios = SeguridadBL.Instancia.GetFormulario().Where(p => p.RolId == usuarioModel.RolId);

        }

        private void LimpiarAutenticacion()
        {
            AuthenticationHelper.SignOut();

            WebSession.Usuario = null;
            WebSession.Formularios = new List<Formulario>();
        }

        private IList<Formulario> GetFormulario()
        {
            return new List<Formulario>
            {
                new Formulario { Id = 1,  Direccion = "//Home/Index", Orden = 1, RolId = 1, Nombre = "Home"  },
                new Formulario { Id = 2,  Direccion = "//Usuario/Index", Orden = 2, RolId = 1, Nombre = "Usuario"  },
                new Formulario { Id = 3,  Direccion = "//Configuracion/Index", Orden = 3, RolId = 1, Nombre = "Configuración"  },
                new Formulario { Id = 4,  Direccion = "//Reporte/Index", Orden = 7, RolId = 1, Nombre = "Envíos"  },


                new Formulario { Id = 5,  Direccion = "//Home/Index", Orden = 1, RolId = 3, Nombre = "Home"  },

                new Formulario { Id = 6,  Direccion = "//TipoDocumento/Index", Orden = 4, RolId = 1, Nombre = "Tipo documento"  },
                new Formulario { Id = 7,  Direccion = "//TipoOpcione/Index", Orden = 7, RolId = 1, Nombre = "Tablas de opciones"  },
                new Formulario { Id = 8,  Direccion = "//Acceso/Index", Orden = 7, RolId = 1, Nombre = "Acceso"  },
                new Formulario { Id = 9,  Direccion = "//Parametro/Index", Orden = 7, RolId = 1, Nombre = "Parametro"  },

            };
        }

        #endregion
    }
}