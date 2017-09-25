using Base.BusinessEntity;
using Base.BusinessLogic;
using Base.Common;
using Base.DTO;
using Base.DTO.AutoMapper;
using Base.Web.Core;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
namespace Base.Web.Controllers
{
    public class ParametroController : BaseController
    {
        // GET: Parametros
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Add(ParametroDTO parametroDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                int resultado = 0;
                var parametro = MapperHelper.Map<ParametroDTO, Parametro>(parametroDTO);


                resultado = ParametroBL.Instancia.Add(parametro);

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
                    Usuario = parametroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(parametroDTO)
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
                    Usuario = parametroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(parametroDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult Update(ParametroDTO parametroDTO)
        {
            var jsonResponse = new JsonResponse { Success = true };
            try
            {
                var parametro = MapperHelper.Map<ParametroDTO, Parametro>(parametroDTO);
                int resultado = ParametroBL.Instancia.Update(parametro);

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
                    Usuario = parametroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(parametroDTO)
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
                    Usuario = parametroDTO.UsuarioRegistro,
                    Objeto = JsonConvert.SerializeObject(parametroDTO)
                });
            }

            return Json(jsonResponse);
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            var jsonResponse = new JsonResponse { Success = true };
            ParametroDTO parametroDTO;
            try
            {
                var usuarioResult = ParametroBL.Instancia.GetAll();
                if (usuarioResult != null)
                {
                    parametroDTO = MapperHelper.Map<Parametro, ParametroDTO>(usuarioResult);
                    jsonResponse.Data = parametroDTO;
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
    }
}