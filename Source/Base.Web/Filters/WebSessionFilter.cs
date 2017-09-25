using Base.BusinessEntity;
using Base.Web.Core;
using Base.Web.Models;
using Base.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Base.Web.Filters
{
    public class WebSessionFilter: ActionFilterAttribute
    {
        private Controller _controllerActual;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _controllerActual = filterContext.Controller as Controller;

            var actionName = filterContext.RouteData.Values["action"].ToString();
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var area = filterContext.RouteData.DataTokens["area"] != null
                ? filterContext.RouteData.DataTokens["area"].ToString()
                : string.Empty;


            if (string.Compare(controllerName, ConstantesWeb.LoginController, StringComparison.Ordinal) != 0 &&
                string.Compare(actionName.ToLowerInvariant(), ConstantesWeb.LoginAction, StringComparison.Ordinal) != 0)
            {
                if (WebSession.Usuario == null)
                {
                    filterContext.Result =
                        new RedirectToRouteResult(
                            new RouteValueDictionary(
                                new
                                {
                                    area = string.Empty,
                                    controller = ConstantesWeb.LoginController,
                                    action = ConstantesWeb.LoginAction
                                }));
                    return;
                }


                if (WebSession.Formularios != null)
                {
                    Formulario formularioActual = null;
                    var verb = filterContext.HttpContext.Request.HttpMethod;

                    var formulariosEncontradosPorControlador =
                                                WebSession.Formularios.Where(
                                                    p =>
                                                        String.Compare(p.Area, area, StringComparison.Ordinal) == 0 &&
                                                        String.Compare(p.Controlador, controllerName, StringComparison.Ordinal) == 0);

                    var encontradosPorControlador = formulariosEncontradosPorControlador as IList<Formulario> ??
                                                    formulariosEncontradosPorControlador.ToList();

                    if (encontradosPorControlador.Any())
                    {
                        formularioActual = encontradosPorControlador.FirstOrDefault();
                    }
                    else
                    {
                        filterContext.Result = HandlerUnauthorizationResponse(verb, controllerName, area, actionName);
                    }

                    WebSession.FormularioActual = formularioActual;
                }
                WebSession.FormularioActual = WebSession.FormularioActual ?? new Formulario();
            }

            base.OnActionExecuting(filterContext);
        }

        private RouteValueDictionary GetRedirectActionUnauthorized(string controllerActual, string areaActual, string accion)
        {
            var indexMethod = _controllerActual.GetType().GetMethod("Index");
            if (indexMethod != null && accion != indexMethod.Name)
            {
                return new RouteValueDictionary(
                    new
                    {
                        area = areaActual,
                        controller = controllerActual,
                        action = indexMethod.Name,
                        state = ConstantesWeb.Unauthorized
                    });
            }

            return new RouteValueDictionary(
                new
                {
                    area = "",
                    controller = "Error",
                    action = "Index",
                    state = ConstantesWeb.Unauthorized
                });
        }

        private ActionResult HandlerUnauthorizationResponse(string verb, string controllerName, string area, string accion)
        {
            if (string.Compare(verb, (HttpVerbs.Get.ToString().ToUpperInvariant()), StringComparison.Ordinal) == 0)
                return new RedirectToRouteResult(GetRedirectActionUnauthorized(controllerName, area, accion));

            if (string.Compare(verb, (HttpVerbs.Post.ToString().ToUpperInvariant()), StringComparison.Ordinal) == 0)
            {
                return new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            area = string.Empty,
                            controller = ConstantesWeb.LoginController,
                            action = ConstantesWeb.LoginAction
                        }));

            }

            return default(ActionResult);
        }
    }
}