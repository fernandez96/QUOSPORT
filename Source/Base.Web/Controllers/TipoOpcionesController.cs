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
    public class TipoOpcionesController : ReportBaseController
    {
        // GET: TipoOpcione
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ListarCabezera(DataTableModel<TablaRegistroFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTableCabezera(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var tablatoList = TablaRegistroBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var tablaDTOList = MapperHelper.Map<IEnumerable<TablaRegistro>, IEnumerable<TablaRegistroDTO>>(tablatoList);
                dataTableModel.data = tablaDTOList;
                if (tablaDTOList.Count() > 0)
                {
                    dataTableModel.recordsTotal = tablatoList[0].Cantidad;
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
        public JsonResult ListarDetalle(DataTableModel<TablaRegistroFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTableDetalle(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var tabladetalleList = TablaRegistroBL.Instancia.GetAllPagingDetalle(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });

                var tabladetalleDTOList = MapperHelper.Map<IEnumerable<TablaRegistro>, IEnumerable<TablaRegistroDTO>>(tabladetalleList);
                dataTableModel.data = tabladetalleDTOList;
                if (tabladetalleDTOList.Count() > 0)
                {
                    dataTableModel.recordsTotal = tabladetalleList[0].Cantidad;
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
        public JsonResult Add(TablaRegistroDTO tablaRegistroDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                var tablaregistro = MapperHelper.Map<TablaRegistroDTO, TablaRegistro>(tablaRegistroDTO);

                resultado = TablaRegistroBL.Instancia.Add(tablaregistro);

                if (resultado > 0)
                {
                    jsonResponse.Title = Title.TitleRegistro;
                    jsonResponse.Message = Mensajes.RegistroSatisfactorio;
                }
                else if (resultado == -1)
                {
                    jsonResponse.Title = Title.TitleAlerta;
                    jsonResponse.Warning = true;
                    jsonResponse.Message = Mensajes.YaExisteRegistro;
                }
                else if (resultado < 0)
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
                    Usuario = tablaRegistroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tablaRegistroDTO)
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
                    Usuario = tablaRegistroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tablaRegistroDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Update(TablaRegistroDTO tablaRegistroDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var tablaregistro = MapperHelper.Map<TablaRegistroDTO, TablaRegistro>(tablaRegistroDTO);
                int resultado = TablaRegistroBL.Instancia.Update(tablaregistro);

                if (resultado > 0)
                {
                    jsonResponse.Title = Title.TitleActualizar;
                    jsonResponse.Message = Mensajes.ActualizacionSatisfactoria;
                }
                if (resultado == 0)
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
                    Usuario = tablaRegistroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tablaRegistroDTO)
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
                    Usuario = tablaRegistroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tablaRegistroDTO)
                });
            }

            return Json(jsonResponse);
        }


        [HttpPost]
        public JsonResult GetById(TablaRegistroDTO tablaRegistroDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var tablaregistro = MapperHelper.Map<TablaRegistroDTO, TablaRegistro>(tablaRegistroDTO);
                var tablaregistroDTO = TablaRegistroBL.Instancia.GetById(tablaregistro);
                if (tablaregistroDTO != null)
                {
                    tablaRegistroDTO = MapperHelper.Map<TablaRegistro, TablaRegistroDTO>(tablaregistroDTO);
                    jsonResponse.Data = tablaRegistroDTO;
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

                var getcorrelativoDTO = TablaRegistroBL.Instancia.GetCorrelativaCab();
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


        [HttpPost]
        public JsonResult AddDetalle(TablaRegistroDTO tablaRegistroDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                var tablaregistro = MapperHelper.Map<TablaRegistroDTO, TablaRegistro>(tablaRegistroDTO);

                resultado = TablaRegistroBL.Instancia.AddDetalle(tablaregistro);

                if (resultado > 0)
                {
                    jsonResponse.Title = Title.TitleRegistro;
                    jsonResponse.Message = Mensajes.RegistroSatisfactorio;
                }
                else if (resultado == -1)
                {
                    jsonResponse.Title = Title.TitleAlerta;
                    jsonResponse.Warning = true;
                    jsonResponse.Message = Mensajes.YaExisteRegistro;
                }
                else if (resultado < 0)
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
                    Usuario = tablaRegistroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tablaRegistroDTO)
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
                    Usuario = tablaRegistroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tablaRegistroDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult UpdateDetalle(TablaRegistroDTO tablaRegistroDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var tablaregistro = MapperHelper.Map<TablaRegistroDTO, TablaRegistro>(tablaRegistroDTO);
                int resultado = TablaRegistroBL.Instancia.UpdateDetalle(tablaregistro);

                if (resultado > 0)
                {
                    jsonResponse.Title = Title.TitleActualizar;
                    jsonResponse.Message = Mensajes.ActualizacionSatisfactoria;
                }
                if (resultado == 0)
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
                    Usuario = tablaRegistroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tablaRegistroDTO)
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
                    Usuario = tablaRegistroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tablaRegistroDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult GetByIdDetalle(TablaRegistroDTO tablaRegistroDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var tablaregistro = MapperHelper.Map<TablaRegistroDTO, TablaRegistro>(tablaRegistroDTO);
                var tablaregistroDTO = TablaRegistroBL.Instancia.GetByIdDetalle(tablaregistro);
                if (tablaregistroDTO != null)
                {
                    tablaRegistroDTO = MapperHelper.Map<TablaRegistro, TablaRegistroDTO>(tablaregistroDTO);
                    jsonResponse.Data = tablaRegistroDTO;
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
        public JsonResult DeleteDetalle(TablaRegistroDTO tablaRegistroDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var tablaregistrodetalle = MapperHelper.Map<TablaRegistroDTO, TablaRegistro>(tablaRegistroDTO);
                int resultado = TablaRegistroBL.Instancia.DeleteDetalle(tablaregistrodetalle);

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
                    Usuario = tablaRegistroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tablaRegistroDTO)
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
                    Usuario = tablaRegistroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tablaRegistroDTO)
                });
            }

            return Json(jsonResponse);
        }

        public JsonResult GetCorrelativoDet(TablaRegistroDTO tablaRegistroDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var tablaregistro = MapperHelper.Map<TablaRegistroDTO, TablaRegistro>(tablaRegistroDTO);

                var getcorrelativoDTO = TablaRegistroBL.Instancia.GetCorrelativaDet(tablaregistro);
                if (getcorrelativoDTO != null)
                {
                    tablaRegistroDTO = MapperHelper.Map<TablaRegistro, TablaRegistroDTO>(getcorrelativoDTO);
                    jsonResponse.Data = tablaRegistroDTO;
                }
                else
                {
                    jsonResponse.Success = true;
                    tablaRegistroDTO = MapperHelper.Map<TablaRegistro, TablaRegistroDTO>(getcorrelativoDTO);
                    jsonResponse.Data = tablaRegistroDTO;
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
            // report.Load(Server.MapPath("~/Prints/M_Administrador/TablaRegistro/TablaOpciones.mrt"));
            report.RegBusinessObject("tabla", "tabla", dataTabla);
            report.RegBusinessObject("tabladetalle", "tabladetalle", dataBtabladetablle);

            return StiMvcViewer.GetReportResult(report);
        }

        #region Métodos Privados

        public void FormatDataTableCabezera(DataTableModel<TablaRegistroFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }
            string WhereModel = "WHERE tbpc_flag_estado=1";


            if (dataTableModel.filter.codigoSearch != null)
            {
                WhereModel += "  AND tbpc_vcod_tabla_opciones = '" + dataTableModel.filter.codigoSearch + "' ";
            }
            if (dataTableModel.filter.descripcionSearch != null)
            {
                WhereModel += "  AND tbpc_vdescripcion LIKE '%" + dataTableModel.filter.descripcionSearch + "%'";
            }
            dataTableModel.whereFilter = WhereModel;
        }

        public void FormatDataTableDetalle(DataTableModel<TablaRegistroFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }
            string WhereModel = "WHERE tbpd_flag_estado=1";

            if (dataTableModel.filter.idTablaSearch > 0)
            {
                WhereModel += " AND tbpc_iid_tabla_opciones= " + dataTableModel.filter.idTablaSearch + " ";
            }

            if (dataTableModel.filter.codigoSearch != null)
            {
                WhereModel += "  AND tbpd_vcod_tabla_opciones_det = '" + dataTableModel.filter.codigoSearch + "' ";
            }
            if (dataTableModel.filter.descripcionSearch != null)
            {
                WhereModel += "  AND tbpd_vdescripcion_detalle LIKE '%" + dataTableModel.filter.descripcionSearch + "%'";
            }
            dataTableModel.whereFilter = WhereModel;
        }
        #endregion
    }
}