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
    public class TransferenciaController : BaseController
    {
        // GET: Transferencia
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Listar(DataTableModel<TransferenciaFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var transferenciaList = TransferenciaBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var transferecniaDTOList = MapperHelper.Map<IEnumerable<Transferencia>, IEnumerable<TransferenciaDTO>>(transferenciaList);
                dataTableModel.data = transferecniaDTOList;
                if (transferecniaDTOList != null)
                {
                    dataTableModel.recordsTotal = transferenciaList[0].Cantidad;
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
        public JsonResult ListarNotaSalidaDetalle(TransferenciaDTO transferenciaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var transferencia = MapperHelper.Map<TransferenciaDTO, Transferencia>(transferenciaDTO);
                var transferenciadetalleList = TransferenciaBL.Instancia.GetAll(transferencia);
                var transferenciadetalleDTOList = MapperHelper.Map<IEnumerable<TransferenciaDetalle>, IEnumerable<TransferenciaDetalleDTO>>(transferenciadetalleList);

                if (transferenciadetalleDTOList != null)
                {
                    jsonResponse.Data = transferenciadetalleDTOList;
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
        public JsonResult Add(TransferenciaDTO transferenciaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                int resultado = 0;
                var transferencia = MapperHelper.Map<TransferenciaDTO, Transferencia>(transferenciaDTO);
                resultado = TransferenciaBL.Instancia.Add(transferencia);
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
                    Usuario = transferenciaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(transferenciaDTO)
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
                    Usuario = transferenciaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(transferenciaDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Update(TransferenciaDTO transferenciaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var transferencia = MapperHelper.Map<TransferenciaDTO, Transferencia>(transferenciaDTO);

                int resultado = 0;
                resultado = TransferenciaBL.Instancia.Update(transferencia);
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
                    Usuario = transferenciaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(transferenciaDTO)
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
                    Usuario = transferenciaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(transferenciaDTO)
                });
            }

            return Json(jsonResponse);
        }


        [HttpPost]
        public JsonResult Delete(TransferenciaDTO transferenciaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var transferencia = MapperHelper.Map<TransferenciaDTO, Transferencia>(transferenciaDTO);
                int resultado = TransferenciaBL.Instancia.Delete(transferencia);

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
                    Usuario = transferenciaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(transferenciaDTO)
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
                    Usuario = transferenciaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(transferenciaDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult GetById(TransferenciaDTO transferenciaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var transferencia = MapperHelper.Map<TransferenciaDTO, Transferencia>(transferenciaDTO);
                var transferenciaByDTO = TransferenciaBL.Instancia.GetById(transferencia);
                if (transferenciaByDTO != null)
                {
                    transferenciaDTO = MapperHelper.Map<Transferencia, TransferenciaDTO>(transferenciaByDTO);
                    jsonResponse.Data = transferenciaDTO;
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
        public JsonResult GetCorrelativo()
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var getcorrelativoDTO = TransferenciaBL.Instancia.GetCorrelativo();
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

        public void FormatDataTable(DataTableModel<TransferenciaFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }
            string WhereModel = "WHERE n.trfc_flag_estado in(1)";


            if (dataTableModel.filter.numeroSearch != null)
            {
                WhereModel += "  AND n.trfc_inum_transf = '" + dataTableModel.filter.numeroSearch + "' ";
            }
            if (dataTableModel.filter.fechaInicioSearch != null)
            {
                WhereModel += " AND n.trfc_sfecha_transf>= CAST('" + dataTableModel.filter.fechaInicioSearch + "' AS DATETIME) ";
            }
            if (dataTableModel.filter.fechaFinSearch != null)
            {
                WhereModel += " AND N.trfc_sfecha_transf<= DATEADD(DAY,1,CAST('" + dataTableModel.filter.fechaFinSearch + "' AS DATETIME)) ";
            }
            dataTableModel.whereFilter = WhereModel;
        }

        #endregion
    }
}