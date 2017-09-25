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
using System.Linq;
using System.Web.Mvc;

namespace Base.Web.Controllers
{
    public class TipoDocumentoController : BaseController
    {
        // GET: TipoDocumento
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Listar(DataTableModel<TipoDocumentoFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var tipodocumentoList = TipoDocumentoBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var tipodocumentoDTOList = MapperHelper.Map<IEnumerable<TipoDocumento>, IEnumerable<TipoDocumentoDTO>>(tipodocumentoList);
                dataTableModel.data = tipodocumentoDTOList;
                if (tipodocumentoDTOList.Count() > 0)
                {
                    dataTableModel.recordsTotal = tipodocumentoList[0].Cantidad;
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
        public JsonResult ListarModulo(DataTableModel<ModuloFilterModel, int> dataTableModel)
        {
            try
            {
                //FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var modelo = ModeloBL.Instancia.GetAllActives();

                var moduloDTOList = MapperHelper.Map<IEnumerable<Modulo>, IEnumerable<ModuloDTO>>(modelo);
                dataTableModel.data = moduloDTOList;
                if (moduloDTOList.Count() > 0)
                {
                    dataTableModel.recordsTotal = moduloDTOList.Count();
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
        public JsonResult Add(TipoDocumentoDTO tipodocumentoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                var tipodocumento = MapperHelper.Map<TipoDocumentoDTO, TipoDocumento>(tipodocumentoDTO);


                resultado = TipoDocumentoBL.Instancia.Add(tipodocumento);

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
                    Usuario = tipodocumentoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tipodocumentoDTO)
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
                    Usuario = tipodocumentoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tipodocumentoDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Update(TipoDocumentoDTO tipodocumentoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var tipodocumento = MapperHelper.Map<TipoDocumentoDTO, TipoDocumento>(tipodocumentoDTO);
                int resultado = TipoDocumentoBL.Instancia.Update(tipodocumento);

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
                    Usuario = tipodocumentoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tipodocumentoDTO)
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
                    Usuario = tipodocumentoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tipodocumentoDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Delete(TipoDocumentoDTO tipodocumentoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var tipodocumento = MapperHelper.Map<TipoDocumentoDTO, TipoDocumento>(tipodocumentoDTO);
                int resultado = TipoDocumentoBL.Instancia.Delete(tipodocumento);

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
                    Usuario = tipodocumentoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tipodocumentoDTO)
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
                    Usuario = tipodocumentoDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(tipodocumentoDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult GetById(TipoDocumentoDTO tipodocumentoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var usuario = MapperHelper.Map<TipoDocumentoDTO, TipoDocumento>(tipodocumentoDTO);
                var tipodocumento = TipoDocumentoBL.Instancia.GetById(usuario);
                if (tipodocumento != null)
                {
                    tipodocumentoDTO = MapperHelper.Map<TipoDocumento, TipoDocumentoDTO>(tipodocumento);
                    jsonResponse.Data = tipodocumentoDTO;
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
        public JsonResult AddModulo(IList<TipoDocumentoDTO> tipodocumentoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                var tipodocumento = MapperHelper.Map<IEnumerable<TipoDocumentoDTO>, IEnumerable<TipoDocumento>>(tipodocumentoDTO);
                foreach (var item in tipodocumento)
                {
                    resultado = TipoDocumentoBL.Instancia.AddModulo(item);
                    jsonResponse.Message = Mensajes.RegistroSatisfactorio;
                    LogBL.Instancia.Add(new Log
                    {
                        Accion = Mensajes.Add,
                        Controlador = Mensajes.UsuarioController,
                        Identificador = resultado,
                        Mensaje = jsonResponse.Message,
                        Usuario = item.UsuarioModificacion,
                        Objeto = JsonConvert.SerializeObject(tipodocumentoDTO)
                    });
                }
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
            catch (Exception ex)
            {
                LogError(ex);
                jsonResponse.Success = false;
                jsonResponse.Title = Title.TitleAlerta;
                jsonResponse.Message = Mensajes.IntenteloMasTarde;
                foreach (var item in tipodocumentoDTO)
                {
                    LogBL.Instancia.Add(new Log
                    {
                        Accion = Mensajes.Add,
                        Controlador = Mensajes.UsuarioController,
                        Identificador = 0,
                        Mensaje = ex.Message,
                        Usuario = item.UsuarioRegistro,
                        Objeto = JsonConvert.SerializeObject(tipodocumentoDTO)
                    });
                }

            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult GetAllModulos()
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var moduloList = ModeloBL.Instancia.GetAllActives();
                var moduloDTOList = MapperHelper.Map<IEnumerable<Modulo>, IEnumerable<ModuloDTO>>(moduloList);
                jsonResponse.Data = moduloDTOList;
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
        public JsonResult GetIdModulo(TipoDocumentoDTO tipodocumentoDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var tipodocumento = MapperHelper.Map<TipoDocumentoDTO, TipoDocumento>(tipodocumentoDTO);
                var moduloList = TipoDocumentoBL.Instancia.GetIdModulo(tipodocumento);
                if (moduloList.Count > 0)
                {
                    var moduloDTOList = MapperHelper.Map<IEnumerable<TipoDocumento>, IEnumerable<TipoDocumentoDTO>>(moduloList);
                    jsonResponse.Data = moduloDTOList;
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

        #region Métodos Privados

        public void FormatDataTable(DataTableModel<TipoDocumentoFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }
            string WhereModel = "WHERE tdocc_flag_estado=1";


            if (dataTableModel.filter.codigoSearch != null)
            {
                WhereModel += "  AND tdocc_vabreviatura_tipo_doc = '" + dataTableModel.filter.codigoSearch + "' ";
            }
            if (dataTableModel.filter.descripcionSearch != null)
            {
                WhereModel += "  AND tdocc_vdescripcion LIKE '%" + dataTableModel.filter.descripcionSearch + "%'";
            }
            dataTableModel.whereFilter = WhereModel;
        }

        #endregion


    }
}