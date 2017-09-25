using Base.Web.Core;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Base.Web.Controllers
{
    public class ReporteController : BaseController
    {
        [HttpPost]
        public ActionResult StimulsoftControl(string controllerGetSnapshot, Dictionary<string, string> parametros)
        {
            TempData["Parametros"] = parametros;
            ViewData["ControllerGetSnapshot"] = controllerGetSnapshot;
            return PartialView();
        }
    }
}