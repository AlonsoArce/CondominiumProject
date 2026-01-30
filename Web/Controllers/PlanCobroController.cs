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
    public class PlanCobroController : Controller
    {
        //private MyContext db = new MyContext();

        [CustomAuthorize("Administrador")]
        // GET: PlanCobro
        public ActionResult Index()
        {
            IEnumerable<PlanCobro> lista = null;
            try
            {
                IServicePlanCobro _ServicePlanCobro = new ServicePlanCobro();
                lista = _ServicePlanCobro.GetPlanesCobros();
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
        // GET: PlanCobro/Details/5
        public ActionResult Details(int? id)
        {
            ServicePlanCobro _ServicePlanCobro = new ServicePlanCobro();
            PlanCobro plancobro = null;
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            plancobro = _ServicePlanCobro.GetPlanCobroByID(Convert.ToInt32(id));
            if (plancobro == null)
            {
                TempData["Message"] = "No existe el Plan de cobro solicitado";
                TempData["Redirect"] = "PlanCobro";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
            return View(plancobro);
        }

        //este metodo es para retornar las categorias seleccionadas
        private MultiSelectList listaRubros(ICollection<RubroCobro> rubros = null)
        {
            IServiceRubroCobro _ServiceRubroCobro = new ServiceRubroCobro();
            IEnumerable<RubroCobro> lista = _ServiceRubroCobro.GetRubroCobros();
            //Seleccionar rubros
            int[] listaRubrosSelect = null;
            if (rubros != null)
            {
                listaRubrosSelect = rubros.Select(c => c.IdRubro).ToArray();
            }

            return new MultiSelectList(lista, "IdRubro", "Descripcion", listaRubrosSelect);
        }

        [CustomAuthorize("Administrador")]
        // GET: PlanCobro/Create
        public ActionResult Create()
        {
            //se necesita la lista de rubros
            ViewBag.IdRubro = listaRubros();
            //
            PlanCobro plan = new PlanCobro();
            plan.IdPlanCobro = 0;
            plan.MontoTotal = 0;
            plan.Descripcion = "";
            return View(plan);
        }

        [CustomAuthorize("Administrador")]
        // GET: PlanCobro/Edit/5
        public ActionResult Edit(int? id)
        {
            ServicePlanCobro _ServiceLibro = new ServicePlanCobro();
            PlanCobro plan = null;

            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Index");
                }

                plan = _ServiceLibro.GetPlanCobroByID(Convert.ToInt32(id));
                if (plan == null)
                {
                    TempData["Message"] = "No existe el plan solicitado";
                    TempData["Redirect"] = "PlanCobro";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                //Listados
                
                ViewBag.IdRubro = listaRubros(plan.RubroCobro);
                return View(plan);
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

        // POST: Libro/Edit/5
        [HttpPost]
        [CustomAuthorize("Administrador")]
        public ActionResult Save(PlanCobro plan, string[] selectedRubros)
        {
            
            
            IServicePlanCobro _ServicePlan = new ServicePlanCobro();
            try
            {
                if (plan.IdPlanCobro == 0)
                {
                    ModelState.Remove("MontoTotal");
                    ModelState.Remove("AsignaPlanCobro");
                    ModelState.Remove("RubroCobro");
                }
                if (ModelState.IsValid)
                {
                    PlanCobro oPlanI = _ServicePlan.Save(plan, selectedRubros);
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Utils.Util.ValidateErrors(this);
                    //ViewBag.idAutor = listAutores(libro.IdAutor);
                    ViewBag.idRubro = listaRubros(plan.RubroCobro);
                    //Cargar la vista crear o actualizar
                    //Lógica para cargar vista correspondiente
                    if (plan.IdPlanCobro > 0)
                    {
                        return (ActionResult)View("Edit", plan);
                    }
                    else
                    {
                        return View("Create", plan);
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
