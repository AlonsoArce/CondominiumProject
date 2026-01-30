using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ApplicationCore.Services;
using Infraestructure.Models;
using Infraestructure.Utils;
using Web.Security;

namespace Web.Controllers
{
    public class UsuarioController : Controller
    {


        // GET: Usuario
        [CustomAuthorize("Administrador")]
        public ActionResult Index()
        {
            IEnumerable<Usuario> lista = null;
            try
            {
                IServiceUsuario _ServiceAutor = new ServiceUsuario();
                lista = _ServiceAutor.GetUsuarios();
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");
            }
            return View(lista);
            
        }

        [CustomAuthorize("Administrador")]
        //GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Libro/Edit/5
        [HttpPost]
        [CustomAuthorize("Administrador")]
        public ActionResult Save(Usuario usuario)
        {
            IServiceUsuario _ServiceUsuario = new ServiceUsuario();
            try
            {               
                if (ModelState.IsValid)
                {
                    Usuario oUsuario = _ServiceUsuario.Save(usuario);
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Utils.Util.ValidateErrors(this);
                    //ViewBag.idAutor = listAutores(libro.IdAutor);
                    //ViewBag.idRubro = listaRubros(usuario.RubroCobro);
                    //Cargar la vista crear o actualizar
                    //Lógica para cargar vista correspondiente
                    if (usuario.IdUsuario > 0)
                    {
                        return (ActionResult)View("Edit", usuario);
                    }
                    else
                    {
                        return View("Create", usuario);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Usuario";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        public ActionResult Edit(int? id)
        {
            IServiceUsuario _ServiceUsuario = new ServiceUsuario();
            Usuario oUsuario = null;
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                oUsuario = _ServiceUsuario.GetUsuarioByID((int)id);
                if (oUsuario == null)
                {
                    TempData["Message"] = "No existe el usuario solicitado";
                    TempData["Redirect"] = "Usuario";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                return View(oUsuario);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "No existe el usuario solicitado";
                TempData["Redirect"] = "Usuario";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            IServiceUsuario _ServiceUsuario = new ServiceUsuario();
            Usuario oUsuario = null;
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                oUsuario = _ServiceUsuario.GetUsuarioByID((int)id);
                if (oUsuario == null)
                {
                    TempData["Message"] = "No existe el usuario solicitado";
                    TempData["Redirect"] = "Usuario";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                return View(oUsuario);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "No existe el usuario solicitado";
                TempData["Redirect"] = "Usuario";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }               
    }
}
