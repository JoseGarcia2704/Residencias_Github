using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Elfie.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml.Linq;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Usuario,Admin")]
    [Area("Admin")]

    public class ProveedoresController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        //trabajar con subida de archivos
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ProveedoresController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProveedorVM proveVM = new ProveedorVM()
            {
                Complemento = new BlogCore.Models.Complemento(),
                Proveedor = new BlogCore.Models.Proveedor(),
            ListaComplemento = _contenedorTrabajo.Complemento.GetListaComplemento()

            };
            return View(proveVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProveedorVM proveVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                if (proveVM.Proveedor.idProveedor == 0 && archivos.Count == 2)
                {
                    string nombreArchivo1 = Guid.NewGuid().ToString();
                    string nombreArchivo2 = Guid.NewGuid().ToString();
                    var subida1 = Path.Combine(rutaPrincipal, @"Documentos");
                    var subida2 = Path.Combine(rutaPrincipal, @"Documentos");

                    var extension1 = Path.GetExtension(archivos[0].FileName);
                    var extension2 = Path.GetExtension(archivos[1].FileName);

                    // Verifica las extensiones de los archivos
                    if (extension1 != ".xml" || extension2 != ".pdf")
                    {
                        ModelState.AddModelError(string.Empty, "Los formatos de archivos no corresponden");
                        return View(proveVM);
                    }

                    // Crea el nombre del archivo
                    using (var fileStream1 = new FileStream(Path.Combine(subida1, nombreArchivo1 + extension1), FileMode.Create))
                    using (var fileStream2 = new FileStream(Path.Combine(subida2, nombreArchivo2 + extension2), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream1);
                        archivos[1].CopyTo(fileStream2);
                    }

                    string xmlPath = Path.Combine(subida1, nombreArchivo1 + extension1);
                    var xmlData = XDocument.Load(xmlPath);
                    string rfc = xmlData.Root.Element("{http://www.sat.gob.mx/cfd/4}Emisor").Attribute("Rfc").Value;
                    string tipo = xmlData.Root.Attribute("TipoDeComprobante").Value;
                    string totalStr = xmlData.Root.Attribute("Total").Value;
                    string folio = xmlData.Root.Attribute("Folio").Value;
                    // Modificar el nombre del archivo con RFC y folio
                    string nombreArchivoXml = $"{rfc.Replace(" ", "")}-{folio}{extension1}";
                    string nombreArchivoPdf = $"{rfc.Replace(" ", "")}-{folio}{extension2}";

                    // Renombrar los archivos
                    System.IO.File.Move(Path.Combine(subida1, nombreArchivo1 + extension1), Path.Combine(subida1, nombreArchivoXml));
                    System.IO.File.Move(Path.Combine(subida2, nombreArchivo2 + extension2), Path.Combine(subida2, nombreArchivoPdf));

                    // Obtener año y mes actuales
                    DateTime now = DateTime.Now;
                    string year = now.Year.ToString();
                    string month = now.Month.ToString("00");

                    // Agregar la carpeta del RFC
                    string subidaFinalXml = Path.Combine(subida1, year, month, rfc);
                    string subidaFinalPdf = Path.Combine(subida2, year, month, rfc);

                    if (!Directory.Exists(subidaFinalXml))
                    {
                        Directory.CreateDirectory(subidaFinalXml);
                    }

                    if (!Directory.Exists(subidaFinalPdf))
                    {
                        Directory.CreateDirectory(subidaFinalPdf);
                    }

                   

                    float total;
                    if (!float.TryParse(totalStr, out total))
                    {
                        throw new Exception("No se pudo convertir el valor de 'Total' a un número válido.");
                    }
                    if (tipo == "I")
                    {
                        string moneda = xmlData.Root.Attribute("Moneda").Value;
                        string uuidf = xmlData.Root
                                            .Element("{http://www.sat.gob.mx/cfd/4}Complemento")
                                            .Element("{http://www.sat.gob.mx/TimbreFiscalDigital}TimbreFiscalDigital")
                                            .Attribute("UUID").Value;
                        string formapago = xmlData.Root.Attribute("FormaPago").Value;
                        // Mover los archivos a las carpetas finales
                        System.IO.File.Move(Path.Combine(subida1, nombreArchivoXml), Path.Combine(subidaFinalXml, nombreArchivoXml));
                        System.IO.File.Move(Path.Combine(subida2, nombreArchivoPdf), Path.Combine(subidaFinalPdf, nombreArchivoPdf));

                        proveVM.Proveedor.Moneda = moneda;
                        proveVM.Proveedor.Folio = folio;
                        proveVM.Proveedor.Monto = total;
                        proveVM.Proveedor.idComplementoFK = 7;
                        proveVM.Proveedor.UUIDF = uuidf;
                        proveVM.Proveedor.metodoPago = formapago;
                        proveVM.Proveedor.FechaRegistro = DateTime.Now;
                        proveVM.Proveedor.fechaPago = DateTime.Now;
                        proveVM.Proveedor.fechaProximaPago = DateTime.Now;
                        proveVM.Proveedor.fechaFactura = DateTime.Now;
                        proveVM.Proveedor.XmlUrl = @"\Documentos\" + year + @"\" + month + @"\" + rfc + @"\" + nombreArchivoXml;
                        proveVM.Proveedor.PdfUrl = @"\Documentos\" + year + @"\" + month + @"\" + rfc + @"\" + nombreArchivoPdf;

                        _contenedorTrabajo.Proveedor.add(proveVM.Proveedor);

                    }
                    else if (tipo == "P")
                    {
                        XNamespace pago20 = "http://www.sat.gob.mx/Pagos20";

                        // Obtener el elemento Pagos
                        var pagos = xmlData.Descendants(pago20 + "Pagos").FirstOrDefault();

                        if (pagos != null)
                        {
                            // Recuperar el MontoTotalPagos
                            var montoTotalPagosAttribute = pagos.Element(pago20 + "Totales")?.Attribute("MontoTotalPagos");
                            if (montoTotalPagosAttribute != null && float.TryParse(montoTotalPagosAttribute.Value, out float montoTotalPagos))
                            {
                                // Obtener el UUID
                                var timbreFiscalDigital = xmlData.Descendants()
                                    .FirstOrDefault(e => e.Name.LocalName == "TimbreFiscalDigital" && e.Name.NamespaceName == "http://www.sat.gob.mx/TimbreFiscalDigital");
                                if (timbreFiscalDigital != null)
                                {
                                    string uuid = (string)timbreFiscalDigital.Attribute("UUID");

                                    // Guardar los IdDocumento en una lista
                                    var idDocumentos = xmlData.Descendants(pago20 + "DoctoRelacionado")
                                        .Select(docto => (string)docto.Attribute("IdDocumento"))
                                        .ToList();

                                    // Calcular la suma de todos los ImpSaldoInsoluto
                                    float sumaImpSaldoInsoluto = xmlData.Descendants(pago20 + "DoctoRelacionado")
                                        .Sum(docto => (float?)docto.Attribute("ImpSaldoInsoluto") ?? 0);


                                    var nuevoComplemento = new BlogCore.Models.Complemento();
                                    proveVM.Complemento = nuevoComplemento;
                                    // Crear la ruta de la carpeta
                                    string rfcFolder = Path.Combine(rutaPrincipal, @"Documentos\Cfdi_Pagos", year, month, rfc);
                                    if (!Directory.Exists(rfcFolder))
                                    {
                                        Directory.CreateDirectory(rfcFolder);
                                    }
                                    System.IO.File.Move(Path.Combine(subida1, nombreArchivoXml), Path.Combine(rfcFolder, nombreArchivoXml));
                                    System.IO.File.Move(Path.Combine(subida2, nombreArchivoPdf), Path.Combine(rfcFolder, nombreArchivoPdf));


                                    proveVM.Complemento.UUIDC = uuid;
                                    proveVM.Complemento.Monto = montoTotalPagos;
                                    proveVM.Complemento.XmlUrl = @"\Documentos\Cfdi_Pagos\" + year + @"\" + month + @"\" + rfc + @"\" + nombreArchivoXml;
                                    proveVM.Complemento.PdfUrl = @"\Documentos\Cfdi_Pagos\" + year + @"\" + month + @"\" + rfc + @"\" + nombreArchivoPdf;

                                    if (sumaImpSaldoInsoluto == 0)
                                    {
                                        proveVM.Complemento.saldoInsoluto = 0;
                                    }
                                    else
                                    {
                                        proveVM.Complemento.saldoInsoluto = 1;
                                    }

                                    _contenedorTrabajo.Complemento.add(proveVM.Complemento);
                                }
                                else
                                {
                                    // Manejar el caso donde no se encuentra el elemento TimbreFiscalDigital
                                    ModelState.AddModelError(string.Empty, "No se encontró el elemento TimbreFiscalDigital en el XML.");
                                }
                            }
                            else
                            {
                                // Manejar el caso donde no se puede obtener el MontoTotalPagos
                                ModelState.AddModelError(string.Empty, "No se pudo obtener el MontoTotalPagos del XML.");
                            }
                        }
                        else
                        {
                            // Manejar el caso donde no se encuentra el elemento Pagos
                            ModelState.AddModelError(string.Empty, "No se encontró el elemento Pagos en el XML.");
                        }
                    }

                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
            }

            return View(proveVM);
        }




        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ProveedorVM proveVM = new ProveedorVM()
            {
                Proveedor = new BlogCore.Models.Proveedor()
            };
            if (id != null)
            {
                proveVM.Proveedor = _contenedorTrabajo.Proveedor.Get(id.GetValueOrDefault());
            }
            return View(proveVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProveedorVM proveVM)
        {
            if (ModelState.IsValid)
            {
                // Obtener el proveedor existente de la base de datos
                Proveedor proveedorExistente = _contenedorTrabajo.Proveedor.Get(proveVM.Proveedor.idProveedor);

                // Copiar los valores que no se están editando desde el proveedor existente al proveedor recibido
                proveVM.Proveedor.OrdenCompra = proveedorExistente.OrdenCompra;
                proveVM.Proveedor.statusComplemento = proveedorExistente.statusComplemento;
                proveVM.Proveedor.Folio = proveedorExistente.Folio;
                proveVM.Proveedor.Monto = proveedorExistente.Monto;
                proveVM.Proveedor.Estatus = proveedorExistente.Estatus;
                proveVM.Proveedor.UUIDF=proveedorExistente.UUIDF;
                proveVM.Proveedor.PdfUrl = proveedorExistente.PdfUrl;
                proveVM.Proveedor.XmlUrl = proveedorExistente.XmlUrl;
                proveVM.Proveedor.metodoPago = proveedorExistente.metodoPago;
                proveVM.Proveedor.Solicitante = proveedorExistente.Solicitante;
                proveVM.Proveedor.comentariosSeguimiento = proveedorExistente.comentariosSeguimiento;
                proveVM.Proveedor.nombreProveedor = proveedorExistente.nombreProveedor;
                proveVM.Proveedor.Moneda = proveedorExistente.Moneda;
               proveVM.Proveedor.idComplementoFK= proveedorExistente.idComplementoFK;


                // Actualizar otras propiedades del proveedor
                proveVM.Proveedor.FechaRegistro = DateTime.Now;
                proveVM.Proveedor.fechaPago = DateTime.Now;
                proveVM.Proveedor.fechaProximaPago = DateTime.Now;
                proveVM.Proveedor.fechaFactura = DateTime.Now;
                // Actualizar el proveedor en la base de datos
                _contenedorTrabajo.Proveedor.Update(proveVM.Proveedor);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(proveVM);
        }




        #region 
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Proveedor.GetAll(includeProperties: "Complemento") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var proveedorExistente = _contenedorTrabajo.Proveedor.Get(id);

            if (proveedorExistente == null)
            {
                return Json(new { success = false, message = "Proveedor no encontrado" });
            }

            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;

            // Elimina los archivos asociados al proveedor
            EliminarArchivo(rutaDirectorioPrincipal, proveedorExistente.XmlUrl);
            EliminarArchivo(rutaDirectorioPrincipal, proveedorExistente.PdfUrl);

            try
            {
                // Elimina el proveedor de la base de datos
                _contenedorTrabajo.Proveedor.Remove(proveedorExistente);
                _contenedorTrabajo.Save();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al borrar proveedor: {ex.Message}" });
            }

            return Json(new { success = true, message = "Proveedor borrado correctamente" });
        }

        private void EliminarArchivo(string rutaDirectorioPrincipal, string archivoUrl)
        {
            if (!string.IsNullOrEmpty(archivoUrl))
            {
                var filePath = Path.Combine(rutaDirectorioPrincipal, archivoUrl.TrimStart('\\'));

                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                        // Agrega registros aquí para verificar que el archivo se haya eliminado
                    }
                    catch (Exception ex)
                    {
                        // Agrega registros para la excepción
                    }
                }
            }
        }




        #endregion
    }
}
