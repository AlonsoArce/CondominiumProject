using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
    public class InformacionController : Controller
    {
        //private MyContext db = new MyContext();

        // GET: Informacion
        [CustomAuthorize("Administrador", "Residente")]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.NotificationMessage = TempData["mensaje"];
            }
            IEnumerable<Informacion> lista = null;
            try
            {
                IServiceInformacion _ServiceInformacion = new ServiceInformacion();
                lista = _ServiceInformacion.GetInformacion();
                ViewBag.tittle = "Lista Informacion";
                IServiceTipoInformacion _ServiceTipoInformacion = new ServiceTipoInformacion();
                ViewBag.listaTipos = _ServiceTipoInformacion.GetTipoInformacion();
                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");
            }
            //return View(db.Informacion.ToList());
        }

        [CustomAuthorize("Administrador", "Residente")]
        public PartialViewResult infoByTipo(int? id)
        {
            IEnumerable<Informacion> lista = null;
            IServiceInformacion _ServiceInformacion = new ServiceInformacion();
            if (id != null)
            {
                if (id == 0)
                {
                    lista = _ServiceInformacion.GetInformacion();
                }
                else
                {
                    lista = _ServiceInformacion.GetInfoByTipo((int)id);
                }
            }
            return PartialView("_PartialViewInfo", lista);
        }

        [CustomAuthorize("Administrador")]
        //Action para generar vista del administrador
        public ActionResult IndexAdmin()
        {
            IEnumerable<Informacion> lista = null;
            try
            {
                IServiceInformacion _ServiceInformacion = new ServiceInformacion();
                lista = _ServiceInformacion.GetInformacion();
                ViewBag.tittle = "Lista Informacion";

                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos!" + ex.Message;
                return RedirectToAction("Default", "Error");
            }            
        }

        private SelectList listaTiposInfo(int tipoInfo = 0)
        {
            IServiceTipoInformacion _ServiceTipoInfo = new ServiceTipoInformacion();
            IEnumerable<TipoInformacion> lista = _ServiceTipoInfo.GetTipoInformacion();
            return new SelectList(lista, "IdTipo", "Descripcion", tipoInfo);
        }

        [CustomAuthorize("Administrador")]
        // GET: Informacion/Create
        public ActionResult Create()
        {
            ViewBag.tipoInfo = listaTiposInfo();
            return View();
        }

        [CustomAuthorize("Administrador")]
        // GET: Informacion/Edit/5
        public ActionResult Edit(int? id)
        {
            ServiceInformacion _ServiceInformacion = new ServiceInformacion();
            Informacion info = null;

            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Index");
                }

                info = _ServiceInformacion.GetInformacionByID(Convert.ToInt32(id));
                if (info == null)
                {
                    TempData["Message"] = "No existe el libro solicitado";
                    TempData["Redirect"] = "Libro";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                //Listados
                ViewBag.IdTipoInformacion = listaTiposInfo(info.IdTipoInformacion);              
                return View(info);
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
        [CustomAuthorize("Administrador")]
        public ActionResult Save(Informacion informacion, HttpPostedFileBase ImageFile)
        {
            //Gestión de archivos
            MemoryStream target = new MemoryStream();
            //Servicio Libro
            IServiceInformacion _ServiceInformacion = new ServiceInformacion();
            try
            {
                //Insertar la imagen
                if (informacion.Imagen == null)
                {
                    if (ImageFile != null)
                    {
                        ImageFile.InputStream.CopyTo(target);
                        informacion.Imagen = target.ToArray();
                        ModelState.Remove("Imagen");
                    }
                }
                if (ModelState.IsValid)
                {
                    Informacion oInformacion = _ServiceInformacion.Save(informacion);
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Utils.Util.ValidateErrors(this);
                    //ViewBag.idAutor = listAutores(libro.IdAutor);
                   // ViewBag.idCategoria = listaCategorias(libro.Categoria);
                    //Cargar la vista crear o actualizar
                    //Lógica para cargar vista correspondiente
                    if (informacion.IdInformacion > 0)
                    {
                        return (ActionResult)View("Edit", informacion);
                    }
                    else
                    {
                        return View("Create", informacion);
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
