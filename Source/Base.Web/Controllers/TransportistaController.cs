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
    public class TransportistaController : BaseController
    {
        // GET: Transportista
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Listar(DataTableModel<TransportistaFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var usuarioList = TransportistaBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var usuarioDTOList = MapperHelper.Map<IEnumerable<Transportista>, IEnumerable<TransportistaDTO>>(usuarioList);
                dataTableModel.data = usuarioDTOList;
                if (usuarioList.Count > 0)
                {
                    dataTableModel.recordsTotal = usuarioList[0].Cantidad;
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
        public JsonResult Add(TransportistaDTO transportistaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                var transportista = MapperHelper.Map<TransportistaDTO, Transportista>(transportistaDTO);

                resultado = TransportistaBL.Instancia.Add(transportista);
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
                    Usuario = transportistaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(transportistaDTO)
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
                    Usuario = transportistaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(transportistaDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Delete(TransportistaDTO transportistaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var transportista = MapperHelper.Map<TransportistaDTO, Transportista>(transportistaDTO);
                int resultado = TransportistaBL.Instancia.Delete(transportista);

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
                    Usuario = transportistaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(transportistaDTO)
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
                    Usuario = transportistaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(transportistaDTO)
                });
            }

            return Json(jsonResponse);
        }


        [HttpPost]
        public JsonResult GetById(TransportistaDTO transportistaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var transportista = MapperHelper.Map<TransportistaDTO, Transportista>(transportistaDTO);
                var transportistaResult = TransportistaBL.Instancia.GetById(transportista);
                if (transportistaResult != null)
                {
                    transportistaDTO = MapperHelper.Map<Transportista, TransportistaDTO>(transportistaResult);
                    jsonResponse.Data = transportistaDTO;
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
        public JsonResult Update(TransportistaDTO transportistaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var transportista = MapperHelper.Map<TransportistaDTO, Transportista>(transportistaDTO);

                int resultado;
                resultado = TransportistaBL.Instancia.Update(transportista);
                if (resultado > 0)
                {
                    jsonResponse.Title = Title.TitleActualizar;
                    jsonResponse.Message = Mensajes.ActualizacionSatisfactoria;
                }
                if (resultado == -2)
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
                    Usuario = transportistaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(transportistaDTO)
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
                    Usuario = transportistaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(transportistaDTO)
                });
            }

            return Json(jsonResponse);
        }


        [HttpPost]
        public JsonResult GetAllEstado(int idtabla)
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

        [HttpPost]
        public JsonResult GetCorrelativoCab()
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {

                var getcorrelativoDTO = TransportistaBL.Instancia.GetCorrelativo();
                if (getcorrelativoDTO !=0)
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
        public JsonResult UpdateStatus(StatusDTO statusDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var status_ = MapperHelper.Map<StatusDTO, Status>(statusDTO);
                status_.tabla = status.TablaTransportista;
                status_.setStatus = status.setStatusTransportista + status_.Estado;
                status_.where = status.WhereTransportista + status_.Id;
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

        public void FormatDataTable(DataTableModel<TransportistaFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }

            dataTableModel.whereFilter = "WHERE tranc_flag_estado IN (1,2)";

            if (!string.IsNullOrWhiteSpace(dataTableModel.filter.RazonSocialSearch))
                dataTableModel.whereFilter += (" AND tranc_vid_transportista = " + dataTableModel.filter.CodigoSearch);

            if (!string.IsNullOrWhiteSpace(dataTableModel.filter.RazonSocialSearch))
                dataTableModel.whereFilter += (" AND tranc_vnombre_transportista LIKE '%" + dataTableModel.filter.RazonSocialSearch + "%'");
        }
    }
    #endregion

}