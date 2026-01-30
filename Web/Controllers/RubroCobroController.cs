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
    public class RubroCobroController : Controller
    {


        // GET: RubroCobro
        [CustomAuthorize("Administrador")]
        public ActionResult Index()
        {
            
            IEnumerable<RubroCobro> lista = null;
            try
            {
                IServiceRubroCobro _ServiceResidencia = new ServiceRubroCobro();
                lista = _ServiceResidencia.GetRubroCobros();
                ViewBag.tittle = "Lista Rubros";
                
                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");
            }
        }

        // GET: RubroCobro/Create
        [CustomAuthorize("Administrador")]
        public ActionResult Create()
        {
            return View();
        }



        // GET: RubroCobro/Edit/5
        [CustomAuthorize("Administrador")]
        public ActionResult Edit(int? id)
        {
            ServiceRubroCobro _ServiceRubro = new ServiceRubroCobro();
            RubroCobro oRubro = null;
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                oRubro = _ServiceRubro.GetRubroByID((int)id);
                if (oRubro == null)
                {
                    TempData["Message"] = "No existe el rubro solicitado";
                    TempData["Redirect"] = "Rubro";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                return View(oRubro);
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
        [CustomAuthorize("Administrador")]
        public ActionResult Save(RubroCobro rubroCobro)
        {
            IServiceRubroCobro _serviceRubroCobro = new ServiceRubroCobro();
            try
            {
                

                if (ModelState.IsValid)
                {
                   RubroCobro oRubro = _serviceRubroCobro.Save(rubroCobro);                   
                }
                else
                {
                    if (rubroCobro.IdRubro > 0)
                    {
                        return (ActionResult)View("Edit", rubroCobro);
                    }
                    else
                    {
                        return View("Create", rubroCobro);
                    }

                }
                return RedirectToAction("Index");
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
    }
}
