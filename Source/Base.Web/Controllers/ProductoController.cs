using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.Web.Core;
namespace Base.Web.Controllers
{
    public class ProductoController : BaseController
    {
        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }
    }
}