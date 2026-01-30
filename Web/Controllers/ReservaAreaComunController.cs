using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Security;
using Web.Util;
using Web.Utils;

namespace Web.Controllers
{
    public class ReservaAreaComunController : Controller
    {
        // GET: ReservaAreaComun
        [CustomAuthorize("Residente")]
        public ActionResult IndexResidente()
        {
            IEnumerable<ReservaAreaComun> lista = null;
            try
            {
                Usuario user = Session["User"] as Usuario;
                IServiceResidencia _ServiceResidencia = new ServiceResidencia();
                Residencia res = _ServiceResidencia.GetResidenciaByUsuario(user.IdUsuario);
                IServiceReservaAreaComun _ServiceReservaAreaComun = new ServiceReservaAreaComun();
                lista = _ServiceReservaAreaComun.GetReservaByResidencia(res.IdResidencia);
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
        public ActionResult IndexAdmin()
        {
            IEnumerable<ReservaAreaComun> lista = null;
            try
            {
                IServiceReservaAreaComun _ServiceReservaAreaComun = new ServiceReservaAreaComun();
                lista = _ServiceReservaAreaComun.GetReservas();
                ViewBag.Estados = _ServiceReservaAreaComun.GetEstados();
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");
            }
            return View(lista);       
        }

        [CustomAuthorize("Administrador", "Residente")]
        public PartialViewResult reservasByEstado(string estadoSeleccionado)
        {
            IEnumerable<ReservaAreaComun> lista = null;
            IServiceReservaAreaComun _ServiceReservaAreaComun = new ServiceReservaAreaComun();
            if (estadoSeleccionado != null)
            {
                if (estadoSeleccionado == "")
                {
                    lista = _ServiceReservaAreaComun.GetReservas();
                }
                else
                {
                    lista = _ServiceReservaAreaComun.GetReservaByEstado(estadoSeleccionado);
                }
            }
            return PartialView("_PartialViewReserva", lista);
        }

        [CustomAuthorize("Administrador", "Residente")]
        public ActionResult Create()
        {           
            try
            {
                if (TempData.ContainsKey("NotificationMessage"))
                {
                    ViewBag.NotificationMessage = TempData["NotificationMessage"];
                }

                //ViewBag.Residencia = residencia;
                ViewBag.AreasComunes = listaAreasComunes();
                ViewBag.Horarios = listaHorarios();
                ViewBag.Estados = listaEstados();
            }
            catch (Exception)
            {

                throw;
            }
            
            return View();
        }

        private SelectList listaAreasComunes(string areaSeleccionada = "")
        {
            IServiceReservaAreaComun _ServiceReservaAreaComun = new ServiceReservaAreaComun();
            IEnumerable<String> lista = _ServiceReservaAreaComun.GetAreas();
            return new SelectList(lista, areaSeleccionada);
        }

        private SelectList listaHorarios(string horarioSeleccionado = "")
        {
            IServiceReservaAreaComun _ServiceReservaAreaComun = new ServiceReservaAreaComun();
            IEnumerable<String> lista = _ServiceReservaAreaComun.GetHorarios();
            return new SelectList(lista, horarioSeleccionado);
        }

        private SelectList listaEstados(String estadoSeleccionado = "")
        {
            IServiceReservaAreaComun _ServiceReservaAreaComun = new ServiceReservaAreaComun();
            IEnumerable<String> lista = _ServiceReservaAreaComun.GetEstados();
            return new SelectList(lista, estadoSeleccionado);
        }

        [HttpPost]
        [CustomAuthorize("Administrador", "Residente")]
        public ActionResult Save(ReservaAreaComun reserva)
        {
            IServiceReservaAreaComun _ServiceReservaAreaComun = new ServiceReservaAreaComun();
            IServiceUsuario _ServiceUsuario = new ServiceUsuario();
            try
            {
                
                
                if (Session["User"] != null)
                {
                    int idUsuario = ((Usuario)Session["User"]).IdUsuario;
                    Usuario oUsuario = _ServiceUsuario.GetUsuarioByID(idUsuario);
                    if (oUsuario.Rol == "Residente")
                    {
                        //Se validad si hay disponibilidad para guardar la reserva
                        if (_ServiceReservaAreaComun.VerificaDisponibilidad(reserva))
                        {

                            IServiceResidencia _ServiceResidencia = new ServiceResidencia();
                            Residencia residencia = _ServiceResidencia.GetResidenciaByUsuario(oUsuario.IdUsuario);
                            reserva.IdResidencia = residencia.IdResidencia;
                            reserva.Estado = "En proceso";
                            //reserva.Residencia = residencia;
                            ReservaAreaComun oReservaAreaComun = _ServiceReservaAreaComun.Save(reserva);
                            TempData["NotificationMessage"] = Util.SweetAlertHelper.Mensaje("Reserva Area Comun", "Reserva guardada satisfactoriamente!", SweetAlertMessageType.success);
                            return RedirectToAction("Create");
                        }
                        else
                        {
                            TempData["NotificationMessage"] = Util.SweetAlertHelper.Mensaje("Reserva Area Comun", "No hay disponibilidad para la Fecha y el Horario indicado", SweetAlertMessageType.error);
                            return RedirectToAction("Create");
                        }
                    }
                    else
                    {
                        ReservaAreaComun oReservaAreaComun = _ServiceReservaAreaComun.Save(reserva);                        
                        TempData["NotificationMessage"] = Util.SweetAlertHelper.Mensaje("Reserva Area Comun", "Reserva actualizada satisfactoriamente!", SweetAlertMessageType.success);
                        return RedirectToAction("IndexAdmin");
                    }                                     
                }
                else
                {
                    TempData["NotificationMessage"] = Util.SweetAlertHelper.Mensaje("Reserva Area Comun", "Debe iniciar sesion", SweetAlertMessageType.success);
                    return RedirectToAction("Create");
                }               
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "ReservaAreaComun";
                TempData["Redirect-Action"] = "IndexAdmin";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize("Administrador")]
        public ActionResult Edit(int? idReserva)
        {
            IServiceReservaAreaComun _ServiceReservaAreaComun = new ServiceReservaAreaComun();
            ReservaAreaComun reserva = null;

            try
            {
                // Si va null
                if (idReserva == null)
                {
                    return RedirectToAction("IndexAdmin");
                }

                reserva = _ServiceReservaAreaComun.GetReservaById(Convert.ToInt32(idReserva));
                ViewBag.FechaSolicitud = reserva.FechaSolicitud;
                ViewBag.FechaReservacion = reserva.FechaReserva;
                if (reserva == null)
                {
                    TempData["Message"] = "No existe la reserva solicitada";
                    TempData["Redirect"] = "ReservaAreaComun";
                    TempData["Redirect-Action"] = "IndexAdmin";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                //Listados
                ViewBag.EstadoSeleccionado = listaEstados(reserva.Estado);               
                return View(reserva);
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