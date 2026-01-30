using ApplicationCore.Services;
using Infraestructure.Models;
using Infraestructure.Utils;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Security;
using Web.Utils;
using Log = Web.Utils.Log;

namespace Web.Controllers
{
    public class ReporteController : Controller
    {
        // GET: Reporte
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IngresosMensualesCatalogo()
        {
            IEnumerable<AsignaPlanCobro> lista = null;
            try
            {

                IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();
                lista = _ServiceAsignaPlanCobro.GetAsignaPlanCobroByEstado("Cancelado");
                return View(lista);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        public ActionResult CreatePdfIngresosMensualesCatalogo()
        {
            //Ejemplos IText7 https://kb.itextpdf.com/home/it7kb/examples
            IEnumerable<AsignaPlanCobro> lista = null;
            try
            {
                // Extraer informacion
                IServiceAsignaPlanCobro _ServiceAsignaPlanCobro = new ServiceAsignaPlanCobro();
                lista = _ServiceAsignaPlanCobro.GetAsignaPlanCobroByEstado("Cancelado");

                // Crear stream para almacenar en memoria el reporte 
                MemoryStream ms = new MemoryStream();
                //Initialize writer
                PdfWriter writer = new PdfWriter(ms);

                //Initialize document
                PdfDocument pdfDoc = new PdfDocument(writer);
                Document doc = new Document(pdfDoc);

                Paragraph header = new Paragraph("Reporte de Ingresos mensuales")
                                   .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                                   .SetFontSize(14)
                                   .SetFontColor(ColorConstants.BLUE);
                doc.Add(header);


                // Crear tabla con 5 columnas 
                Table table = new Table(5, true);


                // los Encabezados
                table.AddHeaderCell("Plan Cobro");
                table.AddHeaderCell("Usuario");
                table.AddHeaderCell("Mes");
                table.AddHeaderCell("Monto pagado");
                table.AddHeaderCell("Estado");
                table.AddFooterCell("Total Ingresos");

                foreach (var item in lista)
                {

                    // Agregar datos a las celdas
                    table.AddCell(new Paragraph(item.PlanCobro.Descripcion));
                    table.AddCell(new Paragraph(item.Residencia.Usuario.Nombre));
                    table.AddCell(new Paragraph(item.MesAsignado));
                    table.AddCell(new Paragraph(item.PlanCobro.MontoTotal.ToString()));
                    table.AddCell(new Paragraph(item.Estado));
                    //table.add
                }
                doc.Add(table);


                // Colocar número de páginas
                int numberOfPages = pdfDoc.GetNumberOfPages();
                for (int i = 1; i <= numberOfPages; i++)
                {

                    // Write aligned text to the specified by parameters point
                    doc.ShowTextAligned(new Paragraph(String.Format("pag {0} of {1}", i, numberOfPages)),
                            559, 826, i, TextAlignment.RIGHT, VerticalAlignment.TOP, 0);
                }


                //Close document
                doc.Close();
                // Retorna un File
                return File(ms.ToArray(), "application/pdf", "reporteIngresosMensuales.pdf");

            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }

        }
    }
}