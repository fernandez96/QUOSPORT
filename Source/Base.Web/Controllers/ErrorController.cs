using System.Web.Mvc;

namespace Base.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View("Error");
        }
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult ServerError()
        {
            return View();
        }

        public ActionResult OperationError(string id)
        {
            ViewBag.ErrorMessage = id;
            return View("Error");
        }
    }
}