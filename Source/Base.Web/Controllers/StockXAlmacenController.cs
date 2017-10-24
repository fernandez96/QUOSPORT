using Base.BusinessEntity;
using Base.BusinessLogic;
using Base.Common;
using Base.Common.DataTable;
using Base.DTO;
using Base.DTO.AutoMapper;
using Base.Web.Core;
using Base.Web.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Base.Web.Core.Hubs;
namespace Base.Web.Controllers
{
    public class StockXAlmacenController : ReporteController
    {
        // GET: StockXAlmacen
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ListaStockXAlmacen()
        {
            var stockXAlmacenHub = GlobalHost.ConnectionManager.GetHubContext<StockXAlmacenHub>();
            stockXAlmacenHub.Clients.All.ListarStockXAlmacenData();

            return Json(new { Mensaje = Mensajes.EncontraronDatos }, JsonRequestBehavior.AllowGet);
        }
    }
}