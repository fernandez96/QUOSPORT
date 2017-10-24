using Base.BusinessLogic;
using Base.Common;
using Base.Common.DataTable;
using Base.DTO;
using Base.DTO.AutoMapper;
using Base.Web.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;

namespace Base.Web.Core.Hubs
{
    [HubName("stockXAlmacenHub")]
    public class StockXAlmacenHub: Hub
    {
        public void Listar()
        {
            //DataTableModel<StockXAlmacenFilterModel, int> dataTableModel = new DataTableModel<StockXAlmacenFilterModel, int>();
            var jsonResponse = new JsonResponse { Success = false };
            try
            {
                //FormatDataTable(dataTableModel);
                var stockXalmacen = StockXAlmacenBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = 1000,
                    WhereFilter = "WHERE ND.dninc_ilag_estado in(1)",
                    Start = 0,
                    OrderBy =""
                });
                //var unidadDTOList = MapperHelper.Map<IEnumerable<StockXAlmacen>, IEnumerable<StockXAlmacenDTO>>(stockXalmacen);
                jsonResponse.Data = stockXalmacen;
                jsonResponse.Success = true;
                //if (stockXalmacen.Count > 0)
                //{
                //    dataTableModel.recordsTotal = stockXalmacen[0].Cantidad;
                //    dataTableModel.recordsFiltered = dataTableModel.recordsTotal;
                //}
            }
            catch (Exception )
            {
                jsonResponse.Success = false;
                jsonResponse.Message = Mensajes.NoEncontraronDatos;
            }
            Clients.Caller.listar(jsonResponse);


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
            string WhereModel = "WHERE ND.dninc_ilag_estado in(1)";

            if (dataTableModel.filter.almacenSearch != null)
            {
                WhereModel += "  AND A.almac_iid_almacen = " + dataTableModel.filter.almacenSearch + " ";
            }

            if (dataTableModel.filter.descripcionSearch != null)
            {
                WhereModel += "  AND P.prdc_vdescripcion LIKE '%" + dataTableModel.filter.descripcionSearch + "%'";
            }

            if (dataTableModel.filter.FechaInicialSearch != null)
            {
                WhereModel += " AND N.ningc_fecha_nota_ingreso>= CAST('" + dataTableModel.filter.FechaInicialSearch + "' AS DATETIME) ";
            }
            if (dataTableModel.filter.FechaFinalSearch != null)
            {
                WhereModel += " AND N.ningc_fecha_nota_ingreso<= DATEADD(DAY,1,CAST('" + dataTableModel.filter.FechaFinalSearch + "' AS DATETIME)) ";
            }
            dataTableModel.whereFilter = WhereModel;
        }
    }
    #endregion

}