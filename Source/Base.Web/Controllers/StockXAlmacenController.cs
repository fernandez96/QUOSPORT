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
        public JsonResult Listar(DataTableModel<StockXAlmacenFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var stockList = StockXAlmacenBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilterI = dataTableModel.whereFilterI,
                    WhereFilterS=dataTableModel.whereFilterS,
                    WhereFilterP=dataTableModel.whereFilterP,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var stockDTOList = MapperHelper.Map<IEnumerable<StockXAlmacen>, IEnumerable<StockXAlmacenDTO>>(stockList);
                dataTableModel.data = stockDTOList;
                if (stockList.Count > 0)
                {
                    dataTableModel.recordsTotal = stockList[0].Cantidad;
                    dataTableModel.recordsFiltered = dataTableModel.recordsTotal;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                ViewBag.MessageError = ex.Message;
            }
            return Json(dataTableModel);

        }


        [HttpPost]
        public JsonResult ListarKardex(KardexDTO kardexDTO)
        {
            DataTableModel<StockXAlmacenFilterModel, int> dataTableModel = new DataTableModel<StockXAlmacenFilterModel, int>();
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var kardex = MapperHelper.Map<KardexDTO, Kardex>(kardexDTO);
                var kadexList = KardexBL.Instancia.GetAll(kardex);
                var kardexDTOList = MapperHelper.Map<IEnumerable<Kardex>, IEnumerable<KardexDTO>>(kadexList);
                if (kardexDTOList != null)
                {
                    dataTableModel.data = kardexDTOList;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                ViewBag.MessageError = ex.Message;
            }
            return Json(dataTableModel);
        }
        [HttpPost]
        public JsonResult ListaStockXAlmacen()
        {
            var stockXAlmacenHub = GlobalHost.ConnectionManager.GetHubContext<StockXAlmacenHub>();
            stockXAlmacenHub.Clients.All.ListarStockXAlmacenData();

            return Json(new { Mensaje = Mensajes.EncontraronDatos }, JsonRequestBehavior.AllowGet);
        }

        #region Métodos Privados
        public void FormatDataTable(DataTableModel<StockXAlmacenFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }
            string WhereModelI = "WHERE ing.ningc_ilag_estado in(1)";
            string WhereModelS = "WHERE nsa.nsalc_ilag_estado in(1)";
            string WhereModelP="";


            if (dataTableModel.filter.almacenSearch != null)
            {
                WhereModelI += "  AND ing.almac_icod_almacen = " + dataTableModel.filter.almacenSearch + " ";
                WhereModelS += "  AND nsa.almac_icod_almacen = " + dataTableModel.filter.almacenSearch + " ";
            }

            if (dataTableModel.filter.descripcionSearch != null)
            {
                WhereModelP += "  AND prodt.prdc_vdescripcion LIKE '%" + dataTableModel.filter.descripcionSearch + "%'";
            }

            if (dataTableModel.filter.FechaInicialSearch != null)
            {
                WhereModelI += " AND ing.ningc_fecha_nota_ingreso>= CAST('" + dataTableModel.filter.FechaInicialSearch + "' AS DATETIME) ";
            }
            if (dataTableModel.filter.FechaFinalSearch != null)
            {
                WhereModelI += " AND ing.ningc_fecha_nota_ingreso<= DATEADD(DAY,1,CAST('" + dataTableModel.filter.FechaFinalSearch + "' AS DATETIME)) ";
            }
            dataTableModel.whereFilterI = WhereModelI;
            dataTableModel.whereFilterS = WhereModelS;
            dataTableModel.whereFilterP = WhereModelP;
        }


    }

    #endregion

}