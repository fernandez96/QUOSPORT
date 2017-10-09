using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.Web.Core;
using Base.BusinessEntity;
using Base.BusinessLogic;
using Base.DTO;
using Base.Common;
using Newtonsoft.Json;
using Base.DTO.AutoMapper;
using Base.Web.Models;
using Base.Common.DataTable;

namespace Base.Web.Controllers
{
    public class CategoriaLineaSublineaController : BaseController
    {
        // GET: CategoriaLineaSublinea
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Listar(DataTableModel<CategoriaFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var CategoriaList = CategoriaBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var categoriaDTOList = MapperHelper.Map<IEnumerable<Categoria>, IEnumerable<CategoriaDTO>>(CategoriaList);
                dataTableModel.data = categoriaDTOList;
                if (categoriaDTOList.Count() > 0)
                {
                    dataTableModel.recordsTotal = CategoriaList[0].Cantidad;
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
        public JsonResult ListarLinea(LineaDTO lineaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var linea = MapperHelper.Map<LineaDTO, Linea>(lineaDTO);
                var lineaList = CategoriaBL.Instancia.GetAllLinea(linea);
                var lineaDTOList = MapperHelper.Map<IEnumerable<Linea>, IEnumerable<LineaDTO>>(lineaList);

                if (lineaDTOList.Count() > 0)
                {
                    jsonResponse.Data = lineaDTOList;
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
        public JsonResult ListarSubLinea(SubLineaDTO sublineaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var sublinea = MapperHelper.Map<SubLineaDTO, SubLinea>(sublineaDTO);
                var sublineaList = CategoriaBL.Instancia.AllSubLineaIdCategoria(sublinea);
                var sublineaDTOList = MapperHelper.Map<IEnumerable<SubLinea>, IEnumerable<SubLineaDTO>>(sublineaList);
                if (sublineaDTOList.Count() > 0)
                {
                    jsonResponse.Data = sublineaDTOList;
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
        public JsonResult Add(CategoriaDTO categoriaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                        var categoria = MapperHelper.Map<CategoriaDTO, Categoria>(categoriaDTO);
                        resultado = CategoriaBL.Instancia.Add(categoria);
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
                    Usuario = categoriaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(categoriaDTO)
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
                    Usuario = categoriaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(categoriaDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Update(CategoriaDTO categoriaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var categoria = MapperHelper.Map<CategoriaDTO, Categoria>(categoriaDTO);

                int resultado = 0;
                resultado = CategoriaBL.Instancia.Update(categoria);
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
                    Usuario = categoriaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(categoriaDTO)
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
                    Usuario = categoriaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(categoriaDTO)
                });
            }

            return Json(jsonResponse);
        }


        [HttpPost]
        public JsonResult Delete(CategoriaDTO categoriaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var categoria = MapperHelper.Map<CategoriaDTO, Categoria>(categoriaDTO);
                int resultado = CategoriaBL.Instancia.Delete(categoria);

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
                    Usuario = categoriaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(categoriaDTO)
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
                    Usuario = categoriaDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(categoriaDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult GetById(CategoriaDTO categoriaDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var categoria = MapperHelper.Map<CategoriaDTO, Categoria>(categoriaDTO);
                var categoriaI = CategoriaBL.Instancia.GetById(categoria);
                if (categoriaI != null)
                {
                    categoriaDTO = MapperHelper.Map<Categoria, CategoriaDTO>(categoriaI);
                    jsonResponse.Data = categoriaDTO;
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
        public JsonResult UpdateStatus(StatusDTO statusDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var status_ = MapperHelper.Map<StatusDTO, Status>(statusDTO);
                status_.tabla = status.TablaCategoria;
                status_.setStatus = status.setStatusCategoria + status_.Estado;
                status_.where = status.WhereCategoria + status_.Id;
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

        public void FormatDataTable(DataTableModel<CategoriaFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }
            string WhereModel = "WHERE ctgcc_iflag_estado in(1,2)";


            if (dataTableModel.filter.codigoSearch != null)
            {
                WhereModel += "  AND ctgcc_vcod_categoria = '" + dataTableModel.filter.codigoSearch + "' ";
            }
            if (dataTableModel.filter.descripcionSearch != null)
            {
                WhereModel += "  AND ctgcc_vdescripcion LIKE '%" + dataTableModel.filter.descripcionSearch + "%'";
            }
            dataTableModel.whereFilter = WhereModel;
        }

        #endregion
    }
}