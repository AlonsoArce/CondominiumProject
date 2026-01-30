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
using Web.Security;
using Web.Utils;

namespace Web.Controllers
{
    public class ResidenciaController : Controller
    {

        // GET: Residencia
        [CustomAuthorize("Administrador")]
        public ActionResult Index()
        {
            //crear arreglo de residencias
            IEnumerable<Residencia> lista = null;
            try
            {
                IServiceResidencia _ServiceResidencia = new ServiceResidencia();
                lista = _ServiceResidencia.GetResidencias();
                ViewBag.tittle = "Lista Residencias";
                //Lista Usuarios
                IServiceUsuario _ServiceUsuario = new ServiceUsuario();
                ViewBag.listaUsuarios = _ServiceUsuario.GetUsuarios();
                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");               
            }
            
        }

        // GET: Residencia/Details/5
        [CustomAuthorize("Administrador")]
        public ActionResult Details(int? id)
        {
            ServiceResidencia _ServiceResidencia = new ServiceResidencia();
            Residencia residencia = null;
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                residencia = _ServiceResidencia.GetResidenciaByID(Convert.ToInt32(id));
                if (residencia == null)
                {
                    TempData["Message"] = "No existe el libro solicitado";
                    TempData["Redirect"] = "Libro";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                return View(residencia);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Libro";
                TempData["Redirect-Action"] = "IndexAdmin";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }

        }

        private SelectList listaEstados(String estadoSeleccionado = "")
        {
            IServiceResidencia _ServiceResidencia = new ServiceResidencia();
            IEnumerable<String> lista = _ServiceResidencia.GetEstados();
            return new SelectList(lista, estadoSeleccionado);
        }

        [CustomAuthorize("Administrador")]
        public ActionResult Create()
        {
            ViewBag.Estado = listaEstados();
            return View();
        }

        [CustomAuthorize("Administrador")]
        public ActionResult Edit(int? id)
        {
            IServiceResidencia _ServiceResidencia = new ServiceResidencia();
            Residencia oResidencia = null;
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                oResidencia = _ServiceResidencia.GetResidenciaByID((int)id);
                if (oResidencia == null)
                {
                    TempData["Message"] = "No existe el la residencia solicitado";
                    TempData["Redirect"] = "Residencia";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                ViewBag.EstadoSeleccionado = listaEstados(oResidencia.Estado);
                return View(oResidencia);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Administrador")]
        public ActionResult Save(Residencia residencia)
        {
            IServiceResidencia _ServiceResidencia = new ServiceResidencia();
            try
            {
                if (ModelState.IsValid)
                {
                    Residencia oResidencia = _ServiceResidencia.Save(residencia);
                }
                else
                {
                    if (residencia.IdResidencia > 0)
                    {
                        return (ActionResult)View("Edit", residencia);
                    }
                    else
                    {
                        return View("Create", residencia);
                    }

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos";
                TempData["Redirect"] = "Residencia";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }
    }
}
