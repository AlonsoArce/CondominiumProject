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
    public class AsignaPlanCobroController : Controller
    {
        [CustomAuthorize("Administrador")]
        // GET: AsignaPlanCobro
        public ActionResult Index()
        {
            IEnumerable<AsignaPlanCobro> lista = null;
            try
            {

                if (TempData.ContainsKey("NotificationMessage"))
                {
                    ViewBag.NotificationMessage = TempData["NotificationMessage"];
                }
                IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();
                lista = _ServiceAsignaPlanCobro.GetAsignaPlanesCobros();
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");
            }
            return View(lista);
        }

        public ActionResult EstadoCuenta()
        {
            IEnumerable<AsignaPlanCobro> lista = null;
            IServiceResidencia _serviceResidencia = new ServiceResidencia();
            Residencia oResidencia = null;
            try
            {                
                IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();
                if(Session["User"] != null)
                {
                    oResidencia = _serviceResidencia.GetResidenciaByUsuario(((Usuario)Session["User"]).IdUsuario);
                }
                lista = _ServiceAsignaPlanCobro.GetAsignaPlanCobroByResidencia(oResidencia.IdResidencia);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");
            }
            return View(lista);
        }

        public ActionResult GestionDeudas()
        {
            IEnumerable<AsignaPlanCobro> lista = null;           
            try
            {
                if (TempData.ContainsKey("NotificationMessage"))
                {
                    ViewBag.NotificationMessage = TempData["NotificationMessage"];
                }
                IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();               
                lista = _ServiceAsignaPlanCobro.GetAsignaPlanCobroByEstado("Pendiente");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");
            }
            return View(lista);
        }

        public ActionResult HistorialPagos()
        {
            IEnumerable<AsignaPlanCobro> lista = null;            
            try
            {
                IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();               
                lista = _ServiceAsignaPlanCobro.GetAsignaPlanCobroByEstado("Cancelado");
                ViewBag.listaMeses = "";
                ViewBag.listaResidentes = "";
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");
            }
            return View(lista);
        }

        public ActionResult historialPorMes(string mes)
        {
            IEnumerable<AsignaPlanCobro> lista = null;
            IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();
            if (string.IsNullOrEmpty(mes))
            {
                lista = _ServiceAsignaPlanCobro.GetAsignaPlanCobroByEstado("Cancelado");
            }
            else
            {
                lista = _ServiceAsignaPlanCobro.GetAsignaPlanCobroByMes(mes);
            }

            return PartialView("_PartialViewMes", lista);
        }

        public ActionResult historialPorResidencia(string filtro)
        {
            IEnumerable<AsignaPlanCobro> lista = null;
            IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();
            if (string.IsNullOrEmpty(filtro))
            {
                lista = _ServiceAsignaPlanCobro.GetAsignaPlanCobroByEstado("Cancelado");
            }
            else
            {
                lista = _ServiceAsignaPlanCobro.GetAsignaPlanCobroByResidente(filtro);
            }

            return PartialView("_PartialViewResidencia", lista);
        }


        private SelectList listaMeses(string mesSeleccionado = "")
        {
            IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();
            IEnumerable<String> lista = _ServiceAsignaPlanCobro.GetMeses();
            return new SelectList(lista, mesSeleccionado);
        }

        private SelectList listaEstados(string estadoSeleccionado = "")
        {
            IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();
            IEnumerable<String> lista = _ServiceAsignaPlanCobro.GetEstados();
            return new SelectList(lista, estadoSeleccionado);
        }

        private SelectList listaPlanesCobro(int planCobro = 0)
        {
            IServicePlanCobro _ServicePlanCobro = new ServicePlanCobro();
            IEnumerable<PlanCobro> lista = _ServicePlanCobro.GetPlanesCobros();
            return new SelectList(lista, "IdPlanCobro", "Descripcion", planCobro);
        }

        private SelectList listaResidencias(int residencia = 0)
        {
            IServiceResidencia _ServiceResidencia = new ServiceResidencia();
            IEnumerable<Residencia> lista = _ServiceResidencia.GetResidencias();
            return new SelectList(lista, "IdResidencia", "IdResidencia", residencia);
        }

        [CustomAuthorize("Administrador")]
        // GET: AsignaPlanCobro/Create
        public ActionResult Create()
        {
            ViewBag.PlanesCobro = listaPlanesCobro();
            ViewBag.Residencias = listaResidencias();
            ViewBag.Meses = listaMeses();
            ViewBag.Estados = listaEstados();
            TempData["Editar"] = 0;
            return View();
        }

        [HttpPost]
        [CustomAuthorize("Administrador")]
        public ActionResult Save(AsignaPlanCobro asignaPlan)
        {
            IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();
            try
            {            
                if (ModelState.IsValid)
                {
                    //Valido si es para editar no debe verificar el mes solo debe actualizar
                   if((int)TempData["Editar"] == 1)
                   {
                        AsignaPlanCobro oAsignaPlan = _ServiceAsignaPlanCobro.Save(asignaPlan);
                        TempData["NotificationMessage"] = Util.SweetAlertHelper.Mensaje("AsignaPlanCobro",
                                                    "Plan actualizado correctamente", Util.SweetAlertMessageType.success
                                                    );
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //validar que una residencia no tenga mas de un Plan de cobro asignado por mes                                         
                        if (_ServiceAsignaPlanCobro.ValidadMes(asignaPlan) == true)
                        {
                            TempData["NotificationMessage"] = Util.SweetAlertHelper.Mensaje("AsignaPlanCobro",
                                                        "Una Residencia solamente puede tener un Plan de cobro asignado por mes", Util.SweetAlertMessageType.error
                                                        );
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            AsignaPlanCobro oAsignaPlan = _ServiceAsignaPlanCobro.Save(asignaPlan);
                            TempData["NotificationMessage"] = Util.SweetAlertHelper.Mensaje("AsignaPlanCobro",
                                                        "Plan asignado correctamente", Util.SweetAlertMessageType.success
                                                        );
                            return RedirectToAction("Index");
                        }
                    }                                                                                                   
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Utils.Util.ValidateErrors(this);                    
                    ViewBag.IdPlan = listaPlanesCobro(asignaPlan.IdPlanCobro);
                    ViewBag.IdResidencia = listaResidencias(asignaPlan.IdResidencia);
                    ViewBag.Mes = listaMeses(asignaPlan.MesAsignado);
                    ViewBag.Estado = listaEstados(asignaPlan.Estado);
                    //Cargar la vista crear o actualizar
                    //Lógica para cargar vista correspondiente
                    if (asignaPlan.IdPlanCobro > 0 && asignaPlan.IdResidencia > 0)
                    {
                        return (ActionResult)View("Edit", asignaPlan);
                    }
                    else
                    {
                        return View("Create", asignaPlan);
                    }
                }               
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

        [CustomAuthorize("Administrador")]
        public ActionResult Edit(int? idPlan, int idResid, string mes)
        {
            IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();
            AsignaPlanCobro asignaPlan = null;

            try
            {
                
                asignaPlan = _ServiceAsignaPlanCobro.GetAsignaPlanCobroByID(Convert.ToInt32(idPlan), Convert.ToInt32(idResid),
                                                                            mes);
                if (asignaPlan == null)
                {
                    TempData["Message"] = "No existe la asignacion solicitada";
                    TempData["Redirect"] = "AsignaPlanCobro";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                //Listados
                ViewBag.IdPlanCobro = listaPlanesCobro(asignaPlan.IdPlanCobro);
                ViewBag.IdResidencia = listaResidencias(asignaPlan.IdResidencia);
                ViewBag.MesSeleccionado = listaMeses(asignaPlan.MesAsignado);
                ViewBag.EstadoAsignaPlan = listaEstados(asignaPlan.Estado);
                TempData["Editar"] = 1;
                return View(asignaPlan);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "AsignaPlanCobro";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        public ActionResult EditGestionDeuda(int? idPlan, int? idResid, string mes)
        {
            IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();
            AsignaPlanCobro asignaPlan = null;
            try
            {
                
                asignaPlan = _ServiceAsignaPlanCobro.GetAsignaPlanCobroByID(Convert.ToInt32(idPlan), Convert.ToInt32(idResid),mes);
                if (asignaPlan == null)
                {
                    TempData["Message"] = "No existe la asignacion solicitada";
                    TempData["Redirect"] = "AsignaPlanCobro";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                asignaPlan.Estado = "Cancelado";
                asignaPlan.Residencia = null;
                asignaPlan.PlanCobro = null;
                AsignaPlanCobro oAsignaPlan = _ServiceAsignaPlanCobro.Save(asignaPlan);
                TempData["NotificationMessage"] = Util.SweetAlertHelper.Mensaje("AsignaPlanCobro",
                                                        "Pago realizado correctamente", Util.SweetAlertMessageType.success);
                return RedirectToAction("GestionDeudas");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "AsignaPlanCobro";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }       

    }
}
