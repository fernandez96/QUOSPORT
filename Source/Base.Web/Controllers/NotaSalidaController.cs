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
                if (categoriaDTOList!= null)
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
                int respuesta=1;
                var notaSalida = MapperHelper.Map<NotaSalidaDTO, NotaSalida>(notaSalidaDTO);
                foreach (var item in notaSalidaDTO.listaDetalleNS)
                {
                    if (stockProducto(item.prdc_icod_producto, item.nsald_cantidad) == 0)
                    {
                        respuesta = 0;
                        break;
                    }
                }

                if (respuesta > 0)
                {
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
        public JsonResult Update(NotaIngresoDTO notaIngresoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var notaIngreso = MapperHelper.Map<NotaIngresoDTO, NotaIngreso>(notaIngresoDTO);

                int resultado = 0;
                resultado = NotaIngresoBL.Instancia.UpdateNI(notaIngreso);
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
                    Usuario = notaIngresoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(notaIngresoDTO)
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
                    Usuario = notaIngresoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(notaIngresoDTO)
                });
            }

            return Json(jsonResponse);
        }


        [HttpPost]
        public JsonResult Delete(NotaIngresoDTO notaIngresoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var notatIngreso = MapperHelper.Map<NotaIngresoDTO, NotaIngreso>(notaIngresoDTO);
                int resultado = NotaIngresoBL.Instancia.DeleteNI(notatIngreso);

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
                    Usuario = notaIngresoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(notaIngresoDTO)
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
                    Usuario = notaIngresoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(notaIngresoDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult GetById(NotaIngresoDTO notaIngresoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var notaingreso = MapperHelper.Map<NotaIngresoDTO, NotaIngreso>(notaIngresoDTO);
                var notaingresoI = NotaIngresoBL.Instancia.GetById(notaingreso);
                if (notaingresoI != null)
                {
                    notaIngresoDTO = MapperHelper.Map<NotaIngreso, NotaIngresoDTO>(notaingresoI);
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
        public JsonResult GetCorrelativo(NotaIngresoDTO notaIngresoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var notatIngreso = MapperHelper.Map<NotaIngresoDTO, NotaIngreso>(notaIngresoDTO);
                var getcorrelativoDTO = NotaIngresoBL.Instancia.GetCorrelativo(notatIngreso);
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

        public int stockProducto(int  idproducto, decimal stock)
        {
            int result;
            decimal resultStock = ProductoBL.Instancia.GetStockProducto(idproducto);
                if (stock >= resultStock)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }            

            return result;
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
            string WhereModel = "WHERE n.ningc_ilag_estado in(1)";


            if (dataTableModel.filter.numeroSearch != null)
            {
                WhereModel += "  AND n.ningc_numero_nota_ingreso = '" + dataTableModel.filter.numeroSearch + "' ";
            }
            //if (dataTableModel.filter.fechaInicioSearch != null)
            //{
            //    WhereModel += "  AND n.ningc_fecha_nota_ingreso LIKE '%" + dataTableModel.filter.fechaInicioSearch + "%'";
            //}
            dataTableModel.whereFilter = WhereModel;
        }

        #endregion

    }
}