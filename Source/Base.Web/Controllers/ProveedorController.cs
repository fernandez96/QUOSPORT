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
    public class ProveedorController : BaseController
    {
        //GET: Proveedor
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Listar(DataTableModel<ProveedorFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var proveedorList = ProveedorBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var proveedorDTOList = MapperHelper.Map<IEnumerable<Proveedor>, IEnumerable<ProveedorDTO>>(proveedorList);
                dataTableModel.data = proveedorDTOList;
                if (proveedorList.Count > 0)
                {
                    dataTableModel.recordsTotal = proveedorList[0].Cantidad;
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
        public JsonResult Add(ProveedorDTO proveedorDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                var proveedor = MapperHelper.Map<ProveedorDTO, Proveedor>(proveedorDTO);
                resultado = ProveedorBL.Instancia.Add(proveedor);
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
                    Usuario = proveedorDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(proveedorDTO)
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
                    Usuario = proveedorDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(proveedorDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Delete(ProveedorDTO proveedorDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var proveedor = MapperHelper.Map<ProveedorDTO, Proveedor>(proveedorDTO);
                int resultado = ProveedorBL.Instancia.Delete(proveedor);

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
                    Usuario = proveedorDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(proveedorDTO)
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
                    Usuario = proveedorDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(proveedorDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult GetById(ProveedorDTO proveedorDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var proveedor = MapperHelper.Map<ProveedorDTO, Proveedor>(proveedorDTO);
                var proveedorResult = ProveedorBL.Instancia.GetById(proveedor);
                if (proveedorResult != null)
                {
                    proveedorDTO = MapperHelper.Map<Proveedor, ProveedorDTO>(proveedorResult);
                    jsonResponse.Data = proveedorDTO;
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
        public JsonResult Update(ProveedorDTO proveedorDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var proveedor = MapperHelper.Map<ProveedorDTO, Proveedor>(proveedorDTO);
                int resultado = 0;


                resultado = ProveedorBL.Instancia.Update(proveedor);
                if (resultado > 0)
                {
                    jsonResponse.Title = Title.TitleActualizar;
                    jsonResponse.Message = Mensajes.ActualizacionSatisfactoria;
                }
                else
                {
                    jsonResponse.Title = Title.TitleAlerta;
                    jsonResponse.Warning = true;
                    jsonResponse.Message = Mensajes.ActualizacionFallida;
                }
                LogBL.Instancia.Add(new Log
                {
                    Accion = Mensajes.Update,
                    Controlador = Mensajes.UsuarioController,
                    Identificador = resultado,
                    Mensaje = jsonResponse.Message,
                    Usuario = proveedorDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(proveedorDTO)
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
                    Usuario = proveedorDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(proveedorDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult GetCargoAll(PaginationParameter<int> paginationParameters)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var usuarioList = CargoBL.Instancia.GetAll(paginationParameters);
                var usuarioDTOList = MapperHelper.Map<IEnumerable<Cargo>, IEnumerable<CargoDTO>>(usuarioList);
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
        public JsonResult GetAllActives()
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var rolList = RolBL.Instancia.GetAllActives();
                var rolDTOList = MapperHelper.Map<IEnumerable<Rol>, IEnumerable<RolDTO>>(rolList);
                jsonResponse.Data = rolDTOList;
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

        public void FormatDataTable(DataTableModel<ProveedorFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }

            dataTableModel.whereFilter = "WHERE U.Estado IN (1)";

            if (!string.IsNullOrWhiteSpace(dataTableModel.filter.CodigoSearch))
                dataTableModel.whereFilter += (" AND RolId = " + dataTableModel.filter.CodigoSearch);

            if (!string.IsNullOrWhiteSpace(dataTableModel.filter.DescripcionSearch))
                dataTableModel.whereFilter += (" AND U.Username LIKE '%" + dataTableModel.filter.DescripcionSearch + "%'");
        }
    }
    #endregion

}