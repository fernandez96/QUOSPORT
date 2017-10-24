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
using System.Web.Mvc;

namespace Base.Web.Controllers
{
    public class UnidadMedidaController : BaseController
    {
        // GET: UnidadMedida
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Listar(DataTableModel<UnidadMedidaFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var unidadList = UnidadMedidaBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var unidadDTOList = MapperHelper.Map<IEnumerable<UnidadMedida>, IEnumerable<UnidadMedidaDTO>>(unidadList);
                dataTableModel.data = unidadDTOList;
                if (unidadList.Count > 0)
                {
                    dataTableModel.recordsTotal = unidadList[0].Cantidad;
                    dataTableModel.recordsFiltered = dataTableModel.recordsTotal;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);

                ViewBag.MessageError = ex.Message;
                dataTableModel.data = new List<UsuarioPaginationModel>();
            }
            return Json(dataTableModel);
        }

        [HttpPost]
        public JsonResult Add(UnidadMedidaDTO unidadMedidaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                var unidad = MapperHelper.Map<UnidadMedidaDTO, UnidadMedida>(unidadMedidaDTO);

                resultado = UnidadMedidaBL.Instancia.Add(unidad);

                if (resultado > 0)
                {
                    jsonResponse.Title = Title.TitleRegistro;
                    jsonResponse.Message = Mensajes.RegistroSatisfactorio;
                }
                else
                {
                    jsonResponse.Title = Title.TitleAlerta;
                    jsonResponse.Warning = true;
                    jsonResponse.Message = Mensajes.RegistroFallido;
                }


                LogBL.Instancia.Add(new Log
                {
                    Accion = Mensajes.Add,
                    Controlador = Mensajes.UsuarioController,
                    Identificador = resultado,
                    Mensaje = jsonResponse.Message,
                    Usuario = unidadMedidaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(unidadMedidaDTO)
                });
            }
            catch (Exception ex)
            {
                LogError(ex);
                jsonResponse.Success = false;
                jsonResponse.Title = Title.TitleAlerta;
                jsonResponse.Message = Mensajes.IntenteloMasTarde;

                LogBL.Instancia.Add(new Log
                {
                    Accion = Mensajes.Add,
                    Controlador = Mensajes.UsuarioController,
                    Identificador = 0,
                    Mensaje = ex.Message,
                    Usuario = unidadMedidaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(unidadMedidaDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Delete(UnidadMedidaDTO unidadMedidaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var unidad = MapperHelper.Map<UnidadMedidaDTO, UnidadMedida>(unidadMedidaDTO);
                int resultado = UnidadMedidaBL.Instancia.Delete(unidad);

                if (resultado > 0)
                {
                    jsonResponse.Title = Title.TitleEliminar;
                    jsonResponse.Message = Mensajes.EliminacionSatisfactoria;
                }
                else
                {
                    jsonResponse.Title = Title.TitleAlerta;
                    jsonResponse.Warning = true;
                    jsonResponse.Message = Mensajes.EliminacionFallida;
                }

                LogBL.Instancia.Add(new Log
                {
                    Accion = Mensajes.Delete,
                    Controlador = Mensajes.UsuarioController,
                    Identificador = resultado,
                    Mensaje = jsonResponse.Message,
                    Usuario = unidadMedidaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(unidadMedidaDTO)
                });
            }
            catch (Exception ex)
            {
                LogError(ex);
                jsonResponse.Success = false;
                jsonResponse.Title = Title.TitleAlerta;
                jsonResponse.Message = Mensajes.IntenteloMasTarde;

                LogBL.Instancia.Add(new Log
                {
                    Accion = Mensajes.Delete,
                    Controlador = Mensajes.UsuarioController,
                    Identificador = 0,
                    Mensaje = ex.Message,
                    Usuario = unidadMedidaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(unidadMedidaDTO)
                });
            }

            return Json(jsonResponse);
        }


        [HttpPost]
        public JsonResult GetById(UnidadMedidaDTO unidadMedidaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var unidad = MapperHelper.Map<UnidadMedidaDTO, UnidadMedida>(unidadMedidaDTO);
                var unidadResult = UnidadMedidaBL.Instancia.GetById(unidad);
                if (unidadResult != null)
                {
                    unidadMedidaDTO = MapperHelper.Map<UnidadMedida, UnidadMedidaDTO>(unidadResult);
                    jsonResponse.Data = unidadMedidaDTO;
                }
                else
                {
                    jsonResponse.Warning = true;
                    jsonResponse.Message = Mensajes.UsuarioNoExiste;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                jsonResponse.Success = false;
                jsonResponse.Message = Mensajes.IntenteloMasTarde;
            }

            return Json(jsonResponse);
        }


        [HttpPost]
        public JsonResult Update(UnidadMedidaDTO unidadMedidaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var unidad = MapperHelper.Map<UnidadMedidaDTO, UnidadMedida>(unidadMedidaDTO);

                int resultado = 0;
                resultado = UnidadMedidaBL.Instancia.Update(unidad);
                if (resultado > 0)
                {
                    jsonResponse.Title = Title.TitleActualizar;
                    jsonResponse.Message = Mensajes.ActualizacionSatisfactoria;
                }
                if (resultado == -2)
                {
                    jsonResponse.Title = Title.TitleAlerta;
                    jsonResponse.Warning = true;
                    jsonResponse.Message = Mensajes.ActualizacionFallida;
                }
                if (resultado == -1)
                {
                    jsonResponse.Title = Title.TitleAlerta;
                    jsonResponse.Warning = true;
                    jsonResponse.Message = Mensajes.YaExisteRegistro;
                }



                LogBL.Instancia.Add(new Log
                {
                    Accion = Mensajes.Update,
                    Controlador = Mensajes.UsuarioController,
                    Identificador = resultado,
                    Mensaje = jsonResponse.Message,
                    Usuario = unidadMedidaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(unidadMedidaDTO)
                });
            }
            catch (Exception ex)
            {
                LogError(ex);
                jsonResponse.Success = false;
                jsonResponse.Title = Title.TitleAlerta;
                jsonResponse.Message = Mensajes.IntenteloMasTarde;

                LogBL.Instancia.Add(new Log
                {
                    Accion = Mensajes.Update,
                    Controlador = Mensajes.UsuarioController,
                    Identificador = 0,
                    Mensaje = ex.Message,
                    Usuario = unidadMedidaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(unidadMedidaDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult UpdateStatus(StatusDTO statusDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var status_ = MapperHelper.Map<StatusDTO, Status>(statusDTO);
                status_.tabla = status.TablaUnidadMedida;
                status_.setStatus = status.setStatusUnidadMedida + status_.Estado;
                status_.where = status.WhereUnidadMedida + status_.Id;
                int resultado = StatusBL.Instancia.status(status_);

                if (resultado > 0)
                {
                    jsonResponse.Title = Title.TitleActualizar;
                    jsonResponse.Message = Mensajes.cambiostatus;
                }
                else
                {
                    jsonResponse.Title = Title.TitleAlerta;
                    jsonResponse.Warning = true;
                    jsonResponse.Message = Mensajes.IntenteloMasTarde;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                jsonResponse.Success = false;
                jsonResponse.Title = Title.TitleAlerta;
                jsonResponse.Message = Mensajes.IntenteloMasTarde;
            }

            return Json(jsonResponse);
        }


        #region Métodos Privados
        public void FormatDataTable(DataTableModel<UnidadMedidaFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }
            string WhereModel = "WHERE umec_bflag_estado in(1,2)";


            if (dataTableModel.filter.CodigoSearch != null)
            {
                WhereModel += "  AND umec_vcod_unidad_medida = '" + dataTableModel.filter.CodigoSearch + "' ";
            }
            if (dataTableModel.filter.DescripcionSearch != null)
            {
                WhereModel += "  AND umec_vdescripcion LIKE '%" + dataTableModel.filter.DescripcionSearch + "%'";
            }
            dataTableModel.whereFilter = WhereModel;
        }
    }
    #endregion

}