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
    public class IncidenciaController : Controller
    {
        [CustomAuthorize("Residente")]
        public ActionResult IndexResidente()
        {
            IEnumerable<Incidencia> lista = null;
            int idUsuario = 0;
            try
            {
                IServiceIncidencia _ServiceIncidencia = new ServiceIncidencia();
                if (Session["User"] != null)
                {
                    Usuario usuario = Session["User"] as Usuario;
                    idUsuario = usuario.IdUsuario;
                }
                lista = _ServiceIncidencia.GetIncidenciasByUsuario(Convert.ToInt32(idUsuario));
                ViewBag.tittle = "Lista Incidencias";
                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");
            }

        }

        [CustomAuthorize("Administrador")]
        // GET: Incidencia
        public ActionResult IndexAdmin()
        {
            Usuario oUsuario = null;
            IServiceUsuario _ServiceUsuario = new ServiceUsuario();
            IEnumerable<Incidencia> lista = null;
            try
            {             
                IServiceIncidencia _ServiceIncidencia = new ServiceIncidencia();
                lista = _ServiceIncidencia.GetIncidencias();
                ViewBag.tittle = "Lista Incidencias";
                ViewBag.Estados = "";
                if (Session["User"] != null)
                {
                    TempData["Usuario"] = Session["User"] as Usuario;
                    oUsuario = _ServiceUsuario.GetUsuarioByID(((Usuario)TempData["Usuario"]).IdUsuario);
                    TempData["Usuario"] = oUsuario;
                }
                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize("Administrador", "Residente")]
        public ActionResult incidenciasPorEstado(string estado)
        {
            IEnumerable<Incidencia> lista = null;
            IServiceIncidencia _ServiceIncidencia = new ServiceIncidencia();
            if (string.IsNullOrEmpty(estado))
            {
                lista = _ServiceIncidencia.GetIncidencias();
            }
            else
            {
                lista = _ServiceIncidencia.GetIncidenciaByEstado(estado);
            }

            return PartialView("_PartialViewEstado", lista);
        }



        [CustomAuthorize("Residente")]
        // GET: Incidencia/Create
        public ActionResult Create()
        {           
            Usuario oUsuario = null;
            Usuario oUser = null;
            int idUsuario = 0;
            IServiceUsuario _ServiceUsuario = new ServiceUsuario();
            Incidencia i = new Incidencia();
            try
            {
                if (Session["User"] != null)
                {
                    oUsuario = Session["User"] as Usuario;
                    idUsuario = oUsuario.IdUsuario;
                }
                oUser = _ServiceUsuario.GetUsuarioByID(idUsuario);
                i.IdUsuario = oUser.IdUsuario;
                i.Usuario = oUsuario;
                i.EstadoIncidencia = "En solicitud";
                i.FechaIncidencia = DateTime.Now;
                //lista = _ServiceIncidencia.GetIncidenciasByUsuario(Convert.ToInt32(idUsuario));
                TempData["Usuario"] = i.Usuario;
                return View(i);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");
            }            
        }

        private SelectList listaEstados(String estadoSeleccionado = "")
        {
            IServiceIncidencia _ServiceIncidencia = new ServiceIncidencia();
            IEnumerable<String> lista = _ServiceIncidencia.GetEstados();
            return new SelectList(lista, estadoSeleccionado);
        }


        [CustomAuthorize("Administrador")]
        // GET: Incidencia/Edit/5
        public ActionResult Edit(int? id)
        {
            ServiceIncidencia _ServiceIncidencia = new ServiceIncidencia();
            Incidencia oIncidencia = null;
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                oIncidencia = _ServiceIncidencia.GetIncidenciaByID((int)id);               
                if (oIncidencia == null)
                {
                    TempData["Message"] = "No existe el rubro solicitado";
                    TempData["Redirect"] = "Rubro";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                ViewBag.EstadoIncidencia = listaEstados(oIncidencia.EstadoIncidencia);
                return View(oIncidencia);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Administrador", "Residente")]
        public ActionResult Save(Incidencia incidencia)
        {
            IServiceIncidencia _serviceIncidencia = new ServiceIncidencia();
            string bandera = "";
            try
            {
                Usuario oUsuario = null;
                if (ModelState.IsValid)
                {
                    
                    if(Session["User"] != null)
                    {
                        oUsuario = TempData["Usuario"] as Usuario;
                        if(oUsuario.Rol!= "Administrador")
                            //incidencia.Usuario = oUsuario;
                            incidencia.IdUsuario = oUsuario.IdUsuario;
                    }
                    
                    Incidencia oIncidencia = _serviceIncidencia.Save(incidencia);
                    if (TempData["Usuario"] != null)
                    {
                        oUsuario = TempData["Usuario"] as Usuario;
                        if (oUsuario.Rol == "Administrador")
                        {
                            bandera = "IndexAdmin";
                        }
                        else
                        {
                            bandera = "IndexResidente";
                        }
                    }
                }
                else
                {
                    if (incidencia.IdIncidencia > 0)
                    {
                        return (ActionResult)View("Edit", incidencia);
                    }
                    else
                    {
                        return View("Create", incidencia);
                    }

                }              
                return RedirectToAction(bandera);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Incidencia";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

    }
}
