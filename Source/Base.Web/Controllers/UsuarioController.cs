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
    public class UsuarioController : BaseController
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Listar(DataTableModel<UsuarioFilterModel, int> dataTableModel)
        {
            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponse = new JsonResponse { Success = false };
                var usuarioList = UsuarioBL.Instancia.GetAllPaging(new PaginationParameter<int>
                {
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    Start = dataTableModel.start,
                    OrderBy = dataTableModel.orderBy
                });
                var usuarioDTOList = MapperHelper.Map<IEnumerable<Usuario>, IEnumerable<UsuarioDTO>>(usuarioList);
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
        public JsonResult Add(UsuarioDTO usuarioDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                var usuario = MapperHelper.Map<UsuarioDTO, Usuario>(usuarioDTO);

                if (!UsuarioBL.Instancia.Exists(usuario))
                {

                    usuario.Password = Seguridad.Encriptar(usuario.Password.Trim());
                    usuario.ConfirmarPassword = Seguridad.Encriptar(usuario.ConfirmarPassword.Trim());


                    if (usuario.Password == usuario.ConfirmarPassword)
                    {
                        resultado = UsuarioBL.Instancia.Add(usuario);
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
                    else
                    {
                        jsonResponse.Warning = true;
                        jsonResponse.Title = Title.TitleAlerta;
                        jsonResponse.Message = Mensajes.ContraseñaNoConsiden;
                    }

                }
                else
                {
                    jsonResponse.Title = Title.TitleAlerta;
                    jsonResponse.Warning = true;
                    jsonResponse.Message = Mensajes.YaExisteRegistro;
                }

                LogBL.Instancia.Add(new Log
                {
                    Accion = Mensajes.Add,
                    Controlador = Mensajes.UsuarioController,
                    Identificador = resultado,
                    Mensaje = jsonResponse.Message,
                    Usuario = usuarioDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(usuarioDTO)
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
                    Usuario = usuarioDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(usuarioDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Delete(UsuarioDTO usuarioDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var usuario = MapperHelper.Map<UsuarioDTO, Usuario>(usuarioDTO);
                int resultado = UsuarioBL.Instancia.Delete(usuario);

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
                    Usuario = usuarioDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(usuarioDTO)
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
                    Usuario = usuarioDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(usuarioDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult GetAllPaging(PaginationParameter<int> paginationParameters)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var usuarioList = UsuarioBL.Instancia.GetAllPaging(paginationParameters);
                var usuarioDTOList = MapperHelper.Map<IEnumerable<Usuario>, IEnumerable<UsuarioDTO>>(usuarioList);
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
        public JsonResult GetById(UsuarioDTO usuarioDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var usuario = MapperHelper.Map<UsuarioDTO, Usuario>(usuarioDTO);
                var usuarioResult = UsuarioBL.Instancia.GetById(usuario);
                if (usuarioResult != null)
                {
                    usuarioDTO = MapperHelper.Map<Usuario, UsuarioDTO>(usuarioResult);
                    usuarioDTO.Password = Seguridad.Desencriptar(usuarioDTO.Password.Trim());
                    usuarioDTO.ConfirmarPassword = Seguridad.Desencriptar(usuarioDTO.ConfirmarPassword.Trim());
                    jsonResponse.Data = usuarioDTO;
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
        public JsonResponse GetByUsername(LoginDTO loginDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };

            try
            {
                var usuario = UsuarioBL.Instancia.GetByUsername(loginDTO.Username, loginDTO.Password);
                if (usuario != null)
                {
                    var usuarioLoginDTO = MapperHelper.Map<Usuario, UsuarioLoginDTO>(usuario);
                    jsonResponse.Data = usuarioLoginDTO;
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

            return jsonResponse;
        }

        [HttpPost]
        public JsonResult Update(UsuarioDTO usuarioDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var usuario = MapperHelper.Map<UsuarioDTO, Usuario>(usuarioDTO);
                usuario.Password = Seguridad.Encriptar(usuario.Password);
                usuario.ConfirmarPassword = Seguridad.Encriptar(usuario.ConfirmarPassword);
                int resultado = 0;

                if (usuario.Password == usuario.ConfirmarPassword)
                {
                    resultado = UsuarioBL.Instancia.Update(usuario);
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
                }
                else
                {
                    jsonResponse.Warning = true;
                    jsonResponse.Title = Title.TitleAlerta;
                    jsonResponse.Message = Mensajes.ContraseñaNoConsiden;

                }



                LogBL.Instancia.Add(new Log
                {
                    Accion = Mensajes.Update,
                    Controlador = Mensajes.UsuarioController,
                    Identificador = resultado,
                    Mensaje = jsonResponse.Message,
                    Usuario = usuarioDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(usuarioDTO)
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
                    Usuario = usuarioDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(usuarioDTO)
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

        public void FormatDataTable(DataTableModel<UsuarioFilterModel, int> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }

            dataTableModel.whereFilter = "WHERE U.Estado IN (1)";

            if (dataTableModel.filter.RolIdSearch > 0)
                dataTableModel.whereFilter += (" AND RolId = " + dataTableModel.filter.RolIdSearch);

            if (!string.IsNullOrWhiteSpace(dataTableModel.filter.UsernameSearch))
                dataTableModel.whereFilter += (" AND U.Username LIKE '%" + dataTableModel.filter.UsernameSearch + "%'");
        }
    }
    #endregion
}