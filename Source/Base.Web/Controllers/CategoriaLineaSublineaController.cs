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
                jsonResponse.Message = Mensajes.IntenteloMasTarde;

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
    }
}