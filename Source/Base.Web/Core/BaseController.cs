using Base.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using log4net;
using Base.Common;
using System.Text;
namespace Base.Web.Core
{
    public class BaseController:Controller
    {
        protected static readonly ILog logger = LogManager.GetLogger(string.Empty);


        #region Control Error

        protected override void OnException(ExceptionContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];

            logger.Error(string.Format("Controlador:{0}  Action:{1}  Mensaje:{2}", controllerName, actionName, WebUtils.GetExceptionMessage(filterContext.Exception)));

            switch (filterContext.HttpContext.Response.StatusCode)
            {
                case 404:
                    filterContext.Result =
                        new RedirectToRouteResult(
                            new RouteValueDictionary(
                                new
                                {
                                    area = string.Empty,
                                    controller = ConstantesWeb.ErrorController,
                                    action = ConstantesWeb.NotFoundAction
                                }));
                    break;
                case 500:
                    filterContext.Result =
                        new RedirectToRouteResult(
                            new RouteValueDictionary(
                                new
                                {
                                    area = string.Empty,
                                    controller = ConstantesWeb.ErrorController,
                                    action = ConstantesWeb.ServerErrorAction
                                }));
                    break;
                default:
                    filterContext.Result = View("Error");
                    break;
            }
        }

        protected override void HandleUnknownAction(string actionName)
        {
            RedirectToAction("Index").ExecuteResult(ControllerContext);
        }

        protected JsonResult MensajeError(string mensaje = "Ocurrio un error al cargar...")
        {
            Response.StatusCode = 404;
            return Json(new JsonResponse { Message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        protected void LogError(Exception exception)
        {
            logger.Error(string.Format("Mensaje: {0} Trace: {1}", exception.Message, exception.StackTrace));
        }

        #endregion Control Error

        protected JsonResult Json(object data)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = "application/json; charset=utf-8",
                ContentEncoding = Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        protected string GetWhere(string columna, string operacion, string valor)
        {
            var opciones = new Dictionary<string, string>();
            {
                opciones.Add("eq", "=");
                opciones.Add("ne", "<>");
                opciones.Add("lt", "<");
                opciones.Add("le", "<=");
                opciones.Add("gt", ">");
                opciones.Add("ge", ">=");
                opciones.Add("bw", "LIKE");
                opciones.Add("bn", "NOT LIKE");
                opciones.Add("in", "LIKE");
                opciones.Add("ni", "NOT LIKE");
                opciones.Add("ew", "LIKE");
                opciones.Add("en", "NOT LIKE");
                opciones.Add("cn", "LIKE");
                opciones.Add("nc", "NOT LIKE");
            }

            if (string.IsNullOrEmpty(operacion))
            {
                return string.Empty;
            }

            if (operacion.Equals("bw") || operacion.Equals("bn"))
            {
                valor = valor + "%";
            }
            if (operacion.Equals("ew") || operacion.Equals("en"))
            {
                valor = "%" + valor;
            }
            if (operacion.Equals("cn") || operacion.Equals("nc") || operacion.Equals("in") || operacion.Equals("ni"))
            {
                valor = "%" + valor + "%";
            }
            if (opciones.Take(6).ToDictionary(p => p.Key, p => p.Value).ContainsKey(operacion))
            {
                return string.IsNullOrEmpty(valor) ? string.Empty : (columna + " ") + opciones[operacion] + " " + valor;
            }

            return columna + " " + opciones[operacion] + " '" + valor + "'";
        }
    }
}