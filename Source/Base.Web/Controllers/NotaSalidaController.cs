using Base.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Base.Web.Controllers
{
    public class NotaSalidaController : BaseController
    {
        // GET: NotaSalida
        public ActionResult Index()
        {
            return View();
        }
    }
}