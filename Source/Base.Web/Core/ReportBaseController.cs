using System.Collections.Generic;
using System.Web.Mvc;
using Stimulsoft.Report.Mvc;

namespace Base.Web.Core
{
    public abstract class ReportBaseController: BaseController
    {


        #region Propiedades

        protected Dictionary<string, string> ParametrosReport
        {
            get
            {
                return (Dictionary<string, string>)TempData["Parametros"];
            }
        }

        #endregion

        #region Constructor

        #endregion

        #region Métodos

        public abstract ActionResult GetReportSnapshot();

        //public virtual ActionResult DesignReport()
        //{
        //    throw new NotImplementedException();
        //}

        [HttpGet]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }

        [HttpPost]
        [ActionName("ViewerEvent")]
        public ActionResult ViewerEventPost()
        {
            return StiMvcViewer.ViewerEventResult();
        }

       
        [HttpPost]
        public ActionResult PrintReport()
        {
            return StiMvcViewer.PrintReportResult(HttpContext);
        }

        [HttpPost]
        public ActionResult ExportReport()
        {
            return StiMvcViewer.ExportReportResult();
        }

        [HttpPost]
        public ActionResult Interaction()
        {
            return StiMvcViewer.InteractionResult(HttpContext);
        }

        #endregion
    }
}