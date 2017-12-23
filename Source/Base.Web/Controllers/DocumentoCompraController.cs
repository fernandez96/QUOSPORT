using Base.BusinessEntity;
using Base.BusinessLogic;
using Base.Common;
using Base.Common.DataTable;
using Base.DTO;
using Base.DTO.AutoMapper;
using Base.Web.Core;
using Base.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace Base.Web.Controllers
{
    public class DocumentoCompraController : BaseController
    {
        // GET: DocumentoCompra
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAll(int idtabla)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var estadoList = TablaRegistroBL.Instancia.GetAll(idtabla);
                var usuarioDTOList = MapperHelper.Map<IEnumerable<TablaRegistro>, IEnumerable<TablaRegistroDTO>>(estadoList);
                jsonResponse.Data = usuarioDTOList;
            }
            catch (Exception ex)
            {
                LogError(ex);
                jsonResponse.Success = false;
                jsonResponse.Message = Mensajes.IntenteloMasTarde;
            }

            return Json(jsonResponse);
        }
    }
}