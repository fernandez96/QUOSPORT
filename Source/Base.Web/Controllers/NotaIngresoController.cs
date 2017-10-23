using Base.BusinessEntity;
using Base.BusinessLogic;
using Base.Common;
using Base.Common.DataTable;
using Base.DTO;
using Base.DTO.AutoMapper;
using Base.Web.Core;
using Base.Web.Models;
using Newtonsoft.Json;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Base.Web.Controllers
{
    public class NotaIngresoController : ReportBaseController
    {
        // GET: NotaIngreso
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Listar(DataTableModel<NotaIngresoFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var NotaIngresoList = NotaIngresoBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var categoriaDTOList = MapperHelper.Map<IEnumerable<NotaIngreso>, IEnumerable<NotaIngresoDTO>>(NotaIngresoList);
                dataTableModel.data = categoriaDTOList;
                if (categoriaDTOList.Count() > 0)
                {
                    dataTableModel.recordsTotal = NotaIngresoList[0].Cantidad;
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
        public JsonResult ListarNotaIngresoDetalle(NotaIngresoDTO notaIngresoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var notaIngreso = MapperHelper.Map<NotaIngresoDTO, NotaIngreso>(notaIngresoDTO);
                var notaingresodetalleList = NotaIngresoBL.Instancia.GetAll(notaIngreso);
                var notaIngresodetalleDTOList = MapperHelper.Map<IEnumerable<NotaIngresoDetalle>, IEnumerable<NotaIngresoDetalleDTO>>(notaingresodetalleList);

                if (notaIngresodetalleDTOList.Count() > 0)
                {
                    jsonResponse.Data = notaIngresodetalleDTOList;
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
        public JsonResult Add(NotaIngresoDTO notaIngresoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                var notatIngreso = MapperHelper.Map<NotaIngresoDTO, NotaIngreso>(notaIngresoDTO);
                resultado = NotaIngresoBL.Instancia.AddNI(notatIngreso);
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
                    Usuario = notaIngresoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(notaIngresoDTO)
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
                    Usuario = notaIngresoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(notaIngresoDTO)
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
        public JsonResult GetCorrelativoCab()
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {

                var getcorrelativoDTO = CategoriaBL.Instancia.GetCorrelativaCab();
                if (getcorrelativoDTO != null)
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

        //imprimir

        [HttpPost]
        public override ActionResult GetReportSnapshot()
        {
            var documentoId = Int32.Parse(ParametrosReport["TablaId"]);
            TablaRegistroDTO entity = new TablaRegistroDTO();
            entity.Id = documentoId;
            var tablaregistro = MapperHelper.Map<TablaRegistroDTO, TablaRegistro>(entity);
            var dataTabla = TablaRegistroBL.Instancia.GetById(tablaregistro);
            var dataBtabladetablle = TablaRegistroBL.Instancia.GetAllPagingDetalle(new PaginationParameter<int>
            {
                AmountRows = 100,
                WhereFilter = "WHERE tbpd_flag_estado=1 AND tbpc_iid_tabla_opciones =" + tablaregistro.Id + "",
                Start = 0,
                OrderBy = "",
            });

            var report = new StiReport();
            report.Load(Server.MapPath("~/Prints/M_Administrador/TablaRegistro/TablaOpciones.mrt"));
            report.RegBusinessObject("tabla", "tabla", dataTabla);
            report.RegBusinessObject("tabladetalle", "tabladetalle", dataBtabladetablle);

            return StiMvcViewer.GetReportResult(report);
        }

        #region Métodos Privados

        public void FormatDataTable(DataTableModel<NotaIngresoFilterModel, int> dataTableModel)
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
            if (dataTableModel.filter.fechaInicioSearch != null)
            {
                WhereModel += "  AND n.ningc_fecha_nota_ingreso LIKE '%" + dataTableModel.filter.fechaInicioSearch + "%'";
            }
            dataTableModel.whereFilter = WhereModel;
        }

        #endregion
    }
}