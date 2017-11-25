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
    public class NotaSalidaController : BaseController
    {
        // GET: NotaSalida
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Listar(DataTableModel<NotaSalidaFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var NotaSalidaList = NotaSalidaBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var categoriaDTOList = MapperHelper.Map<IEnumerable<NotaSalida>, IEnumerable<NotaSalidaDTO>>(NotaSalidaList);
                dataTableModel.data = categoriaDTOList;
                if (categoriaDTOList != null)
                {
                    dataTableModel.recordsTotal = NotaSalidaList[0].Cantidad;
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
        public JsonResult ListarNotaSalidaDetalle(NotaSalidaDTO notaSalidaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var notaSalida = MapperHelper.Map<NotaSalidaDTO, NotaSalida>(notaSalidaDTO);
                var notasalidadetalleList = NotaSalidaBL.Instancia.GetAll(notaSalida);
                var notaSalidadetalleDTOList = MapperHelper.Map<IEnumerable<NotaSalidaDetalle>, IEnumerable<NotaSalidaDetalleDTO>>(notasalidadetalleList);

                if (notaSalidadetalleDTOList != null)
                {
                    jsonResponse.Data = notaSalidadetalleDTOList;
                    jsonResponse.Message = "datos encontrados";
                }
            }
            catch (Exception ex)
            {
                LogError(ex);

                ViewBag.MessageError = ex.Message;
                jsonResponse.Data = new List<UsuarioPaginationModel>();
            }
            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Add(NotaSalidaDTO notaSalidaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                int resultado = 0;
                var notaSalida = MapperHelper.Map<NotaSalidaDTO, NotaSalida>(notaSalidaDTO);
                resultado = NotaSalidaBL.Instancia.AddNS(notaSalida);
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
                    Usuario = notaSalidaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(notaSalidaDTO)
                });
            }
            catch (Exception ex)
            {
                LogError(ex);
                jsonResponse.Success = false;
                jsonResponse.Title = Title.TitleAlerta;
                jsonResponse.Message = ex.Message;

                LogBL.Instancia.Add(new Log
                {
                    Accion = Mensajes.Add,
                    Controlador = Mensajes.UsuarioController,
                    Identificador = 0,
                    Mensaje = ex.Message,
                    Usuario = notaSalidaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(notaSalidaDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Update(NotaSalidaDTO notaSalidaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var notaSalida = MapperHelper.Map<NotaSalidaDTO, NotaSalida>(notaSalidaDTO);

                int resultado = 0;
                resultado = NotaSalidaBL.Instancia.UpdateNS(notaSalida);
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
                    Usuario = notaSalidaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(notaSalidaDTO)
                });
            }
            catch (Exception ex)
            {
                LogError(ex);
                jsonResponse.Success = false;
                jsonResponse.Title = Title.TitleAlerta;
                jsonResponse.Message = Mensajes.YaExisteRegistro;

                LogBL.Instancia.Add(new Log
                {
                    Accion = Mensajes.Update,
                    Controlador = Mensajes.UsuarioController,
                    Identificador = 0,
                    Mensaje = ex.Message,
                    Usuario = notaSalidaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(notaSalidaDTO)
                });
            }

            return Json(jsonResponse);
        }


        [HttpPost]
        public JsonResult Delete(NotaSalidaDTO notaSalidaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var notatSalida = MapperHelper.Map<NotaSalidaDTO, NotaSalida>(notaSalidaDTO);
                int resultado = NotaSalidaBL.Instancia.DeleteNS(notatSalida);

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
                    Usuario = notaSalidaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(notaSalidaDTO)
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
                    Usuario = notaSalidaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(notaSalidaDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult GetById(NotaSalidaDTO notaIngresoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var notaSalida = MapperHelper.Map<NotaSalidaDTO, NotaSalida>(notaIngresoDTO);
                var notaSalidadDTO = NotaSalidaBL.Instancia.GetById(notaSalida);
                if (notaSalidadDTO != null)
                {
                    notaIngresoDTO = MapperHelper.Map<NotaSalida, NotaSalidaDTO>(notaSalidadDTO);
                    jsonResponse.Data = notaIngresoDTO;
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
        public JsonResult GetCorrelativo(NotaSalidaDTO notaSalidaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var notaSalida = MapperHelper.Map<NotaSalidaDTO, NotaSalida>(notaSalidaDTO);
                var getcorrelativoDTO = NotaSalidaBL.Instancia.GetCorrelativo(notaSalida);
                if (getcorrelativoDTO > 0)
                {
                    jsonResponse.Data = getcorrelativoDTO;
                }
                else
                {
                    jsonResponse.Success = true;
                    jsonResponse.Data = getcorrelativoDTO;

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
        public JsonResult stockProducto(int idproducto, int idalmacen)
        {
            var jsonResponse = new JsonResponse { Success = true };

            decimal resultStock = ProductoBL.Instancia.GetStockProducto(idproducto, idalmacen);
            if (resultStock > 0)
            {
                jsonResponse.Data = resultStock;
            }
            else
            {
                jsonResponse.Warning = true;
                jsonResponse.Message = Mensajes.IntenteloMasTarde;
            }
            return Json(jsonResponse);
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
        #region Métodos Privados

        public void FormatDataTable(DataTableModel<NotaSalidaFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }
            string WhereModel = "WHERE n.nsalc_ilag_estado in(1)";


            if (dataTableModel.filter.numeroSearch != null)
            {
                WhereModel += "  AND n.nsalc_numero_nota_salida = '" + dataTableModel.filter.numeroSearch + "' ";
            }
            if (dataTableModel.filter.fechaInicioSearch != null)
            {
                WhereModel += " AND n.nsalc_fecha_nota_salida>= CAST('" + dataTableModel.filter.fechaInicioSearch + "' AS DATETIME) ";
            }
            if (dataTableModel.filter.fechaFinSearch != null)
            {
                WhereModel += " AND N.nsalc_fecha_nota_salida<= DATEADD(DAY,1,CAST('" + dataTableModel.filter.fechaFinSearch + "' AS DATETIME)) ";
            }
            dataTableModel.whereFilter = WhereModel;
        }

        #endregion

    }
}