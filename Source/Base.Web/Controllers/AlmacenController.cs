using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.Web.Core;
using Base.BusinessLogic;
using Base.Web.Models;
using Base.Common.DataTable;
using Base.Common;
using Base.DTO.AutoMapper;
using Base.BusinessEntity;
using Base.DTO;
using Newtonsoft.Json;

namespace Base.Web.Controllers
{
    public class AlmacenController : BaseController
    {
        // GET: Almacen
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Listar(DataTableModel<AlmacenFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var almacenList = AlmacenBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var almacenDTOList = MapperHelper.Map<IEnumerable<Almacen>, IEnumerable<AlmacenDTO>>(almacenList);
                dataTableModel.data = almacenDTOList;
                if (almacenList.Count > 0)
                {
                    dataTableModel.recordsTotal = almacenList[0].Cantidad;
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
        public JsonResult Add(AlmacenDTO almacenDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                var almacen = MapperHelper.Map<AlmacenDTO, Almacen>(almacenDTO);

                      resultado = AlmacenBL.Instancia.Add(almacen);

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
                    Usuario = almacenDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(almacenDTO)
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
                    Usuario = almacenDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(almacenDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Delete(AlmacenDTO almacenDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var almacen = MapperHelper.Map<AlmacenDTO, Almacen>(almacenDTO);
                int resultado = AlmacenBL.Instancia.Delete(almacen);

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
                    Usuario = almacenDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(almacenDTO)
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
                    Usuario = almacenDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(almacenDTO)
                });
            }

            return Json(jsonResponse);
        }

      
        [HttpPost]
        public JsonResult GetById(AlmacenDTO almacenDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var almacen = MapperHelper.Map<AlmacenDTO, Almacen>(almacenDTO);
                var almacenResult = AlmacenBL.Instancia.GetById(almacen);
                if (almacenResult != null)
                {
                    almacenDTO = MapperHelper.Map<Almacen, AlmacenDTO>(almacenResult);
                    jsonResponse.Data = almacenDTO;
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
        public JsonResult Update(AlmacenDTO almacenDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var almacen = MapperHelper.Map<AlmacenDTO, Almacen>(almacenDTO);
            
                int resultado = 0;
                      resultado = AlmacenBL.Instancia.Update(almacen);
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
                    Usuario = almacenDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(almacenDTO)
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
                    Usuario = almacenDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(almacenDTO)
                });
            }

            return Json(jsonResponse);
        }

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

        [HttpPost]
        public JsonResult UpdateStatus(StatusDTO statusDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var status_ = MapperHelper.Map<StatusDTO, Status>(statusDTO);
                status_.tabla = status.TablaAlmacen;
                status_.setStatus = status.setStatusAlmacen + status_.Estado;
                status_.where = status.WhereAlmacen + status_.Id;
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
        public void FormatDataTable(DataTableModel<AlmacenFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }
            string WhereModel = "WHERE A.almac_bflag_estado in(1,2)";


            if (dataTableModel.filter.CodigoSearch != null)
            {
                WhereModel += "  AND A.almac_vcod_almacen = '" + dataTableModel.filter.CodigoSearch + "' ";
            }
            if (dataTableModel.filter.DescripcionSearch != null)
            {
                WhereModel += "  AND A.almac_vdescripcion LIKE '%" + dataTableModel.filter.DescripcionSearch + "%'";
            }
            dataTableModel.whereFilter = WhereModel;
        }
    }
    #endregion
}