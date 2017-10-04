﻿using Base.BusinessEntity;
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
using System.Linq;
using System.Web.Mvc;

namespace Base.Web.Controllers
{
    public class ProductoController : BaseController
    {
        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Listar(DataTableModel<ProductoFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var productoList = ProductoBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var productoDTOList = MapperHelper.Map<IEnumerable<Producto>, IEnumerable<ProductoDTO>>(productoList);
                dataTableModel.data = productoDTOList;
    
                if (productoDTOList.Count() > 0)
                {
                    dataTableModel.recordsTotal = productoList[0].Cantidad;
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
        public JsonResult Add(ProductoDTO productoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                var producto = MapperHelper.Map<ProductoDTO, Producto>(productoDTO);


                resultado = ProductoBL.Instancia.Add(producto);

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
                    Usuario = productoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(productoDTO)
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
                    Usuario = productoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(productoDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Update(ProductoDTO productoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var producto = MapperHelper.Map<ProductoDTO, Producto>(productoDTO);
                int resultado = ProductoBL.Instancia.Update(producto);

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
                    Usuario = productoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(productoDTO)
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
                    Usuario = productoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(productoDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Delete(ProductoDTO productoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var producto = MapperHelper.Map<ProductoDTO, Producto>(productoDTO);
                int resultado = ProductoBL.Instancia.Delete(producto);

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
                    Usuario = productoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(productoDTO)
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
                    Usuario = productoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(productoDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult GetById(ProductoDTO productoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var producto = MapperHelper.Map<ProductoDTO, Producto>(productoDTO);
                var producto_ = ProductoBL.Instancia.GetById(producto);
                if (producto_ != null)
                {
                    productoDTO = MapperHelper.Map<Producto, ProductoDTO>(producto_);
                    jsonResponse.Data = productoDTO;
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
        public JsonResult ListarCategoria()
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
 
                var CategoriaList = CategoriaBL.Instancia.GetAll();
                var categoriaDTOList = MapperHelper.Map<IEnumerable<Categoria>, IEnumerable<CategoriaDTO>>(CategoriaList);

                if (categoriaDTOList.Count() > 0)
                {
                    jsonResponse.Data = categoriaDTOList;
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
                var sublineaList = CategoriaBL.Instancia.GetAllSubLinea(sublinea);
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

        #region Métodos Privados

        public void FormatDataTable(DataTableModel<ProductoFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }
            string WhereModel = "WHERE  P.prdc_bflag_estado=1";


            if (dataTableModel.filter.codigoSearch != null)
            {
                WhereModel += "  AND P.prdc_vcod_producto = '" + dataTableModel.filter.codigoSearch + "' ";
            }
            if (dataTableModel.filter.descripcionSearch != null)
            {
                WhereModel += "  AND P.prdc_vdescripcion LIKE '%" + dataTableModel.filter.descripcionSearch + "%'";
            }
            dataTableModel.whereFilter = WhereModel;
        }

        #endregion
    }
}