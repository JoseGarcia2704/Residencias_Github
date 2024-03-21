using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Elfie.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using BlogCore.Utilidades;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Usuario,Admin")]
    [Area("Admin")]

    public class ProveedoresController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        //trabajar con subida de archivos
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProveedoresController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;

        }

        [HttpGet]
        public IActionResult Index()
        {
            var usuario = _userManager.GetUserAsync(User).Result;
            var rfcUsuario = usuario.Rfc;
            IEnumerable<Proveedor> proveedores;

            if (User.IsInRole(CNT.Admin))
            {
                proveedores = _contenedorTrabajo.Proveedor.GetAll(includeProperties: "Complemento");
            }
            else
            {
                proveedores = _contenedorTrabajo.Proveedor.GetAll(filter: p => p.Rfc.Trim().ToUpper() == rfcUsuario.Trim().ToUpper());
            }
           


            return View(proveedores);
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
            var usuario = _userManager.GetUserAsync(User).Result;
            var razonsocial = usuario.razonSocial;
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

                    if (extension1 != ".pdf" || extension2 != ".xml")
                    {
                        var message = "Los formatos de archivos no corresponden";
                        TempData["AlertMessage"] = message;
                        return View(proveVM);
                    }

                    // Crea el nombre del archivo
                    using (var fileStream1 = new FileStream(Path.Combine(subida1, nombreArchivo1 + extension1), FileMode.Create))
                    using (var fileStream2 = new FileStream(Path.Combine(subida2, nombreArchivo2 + extension2), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream1);
                        archivos[1].CopyTo(fileStream2);
                    }
                    // Renombrar los archivos, sobrescribiendo si existen
                    string rutaArchivoXml = Path.Combine(subida1, nombreArchivo1);
                    string rutaArchivoPdf = Path.Combine(subida2, nombreArchivo2);
                    
                   



                    string xmlPath = Path.Combine(subida2, nombreArchivo2 + extension2);
                    var xmlData = XDocument.Load(xmlPath);
                    string rfc = xmlData.Root.Element("{http://www.sat.gob.mx/cfd/4}Emisor").Attribute("Rfc").Value;
                    string tipo = xmlData.Root.Attribute("TipoDeComprobante").Value;
                    string totalStr = xmlData.Root.Attribute("Total").Value;
                    string folio = xmlData.Root.Attribute("Folio").Value;
                    string fecha = xmlData.Root.Attribute("Fecha").Value;
                    DateTime fechaConvertida;

                    if (DateTime.TryParse(fecha, out fechaConvertida))
                    {
                        // La conversión fue exitosa, ahora puedes usar fechaConvertida como DateTime
                        // Por ejemplo, puedes mostrarla en un formato específico
                        Console.WriteLine(fechaConvertida.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        // La conversión falló, manejar el error según sea necesario
                        Console.WriteLine("No se pudo convertir la fecha.");
                    }

                    // Modificar el nombre del archivo con RFC y folio
                    string nombreArchivoXml = $"{rfc.Replace(" ", "")}-{folio}{extension2}";
                    string nombreArchivoPdf = $"{rfc.Replace(" ", "")}-{folio}{extension1}";

                    // Renombrar los archivos
                    System.IO.File.Move(Path.Combine(subida1, nombreArchivo1 + extension1), Path.Combine(subida1, nombreArchivoPdf));
                    System.IO.File.Move(Path.Combine(subida2, nombreArchivo2 + extension2), Path.Combine(subida2, nombreArchivoXml));

                    // Utilizar las rutas renombradas al eliminar los archivos
                    string rutaArchivoPdfRenombrado = Path.Combine(subida1, nombreArchivoPdf);
                    string rutaArchivoXmlRenombrado = Path.Combine(subida2, nombreArchivoXml);

                    
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
                        var uuidfExistente = _contenedorTrabajo.Proveedor.GetAll().Any(p => p.UUIDF == uuidf);
                        if (uuidfExistente)
                        {

                            EliminarArchivos(rutaArchivoXmlRenombrado, rutaArchivoPdfRenombrado);
                            var message = $"Ya existe un registro de esta factura ";
                            TempData["AlertMessage"] = message;
                            return View(proveVM);

                        }
                        var usuarioI = _userManager.GetUserAsync(User).Result;
                        var rfcusuario = usuarioI.Rfc;

                        if (rfcusuario != rfc && !User.IsInRole(CNT.Admin))
                        {
                            EliminarArchivos(rutaArchivoXmlRenombrado, rutaArchivoPdfRenombrado);
                            var message = "La factura no le pertenece al usuario logeado";
                            TempData["AlertMessage"] = message;
                            return View(proveVM);
                        }
                        //if (fechaConvertida.Year != DateTime.Now.Year)
                        //{
                        //    EliminarArchivos(rutaArchivoXmlRenombrado, rutaArchivoPdfRenombrado);
                        //    var message = "La factura debe ser del ano actual.";
                        //    TempData["AlertMessage"] = message;
                        //    return View(proveVM);
                        //}


                        var fechaActual = DateTime.Now;

                        // Obtener todas las fechas de pago y statusComplemento del proveedor con el RFC del usuario logeado
                        var fechasPagos = _contenedorTrabajo.Proveedor.GetAll().Where(factura => factura.Rfc == rfcusuario).Select(factura => new { factura.fechaPago, factura.statusComplemento }).ToList();

                        // Verificar si ya es 10 del siguiente mes
                        if (fechaActual.Day >= 10)
                        {
                            foreach (var factura in fechasPagos)
                            {
                                var fechaLimitePago = factura.fechaPago.AddMonths(1);
                                fechaLimitePago = new DateTime(fechaLimitePago.Year, fechaLimitePago.Month, 10);

                                if (factura.statusComplemento != "C" && fechaActual >= fechaLimitePago)
                                {
                                    EliminarArchivos(rutaArchivoXmlRenombrado, rutaArchivoPdfRenombrado);
                                    var message = $"No se puede subir una nueva factura si la fecha limite de pago ya ha pasado y tiene facturas sin complemento ";
                                    TempData["AlertMessage"] = message;
                                    return View(proveVM);
                                }
                            }
                        }







                        string metodopago = xmlData.Root.Attribute("MetodoPago").Value;

                        // Mover los archivos a las carpetas finales
                        System.IO.File.Move(Path.Combine(subida1, nombreArchivoXml), Path.Combine(subidaFinalXml, nombreArchivoXml));
                        System.IO.File.Move(Path.Combine(subida2, nombreArchivoPdf), Path.Combine(subidaFinalPdf, nombreArchivoPdf));
                        
                        if (User.IsInRole(CNT.Usuario))
                        {
                            proveVM.Proveedor.comentariosSeguimiento = "Na";

                        }
                        
                        var statusAX = "2";
                      
                        var statusBase = "Na";
                        switch (statusAX)
                        {
                            case "2":
                                statusBase = "Revision";
                                break;
                            case "3":
                                 statusBase = "Cancelado";
                                break;
                            case "4":
                                statusBase = "Validada";
                                break;
                            case "5":
                                statusBase = "Pagada";
                                break;
                            // Puedes agregar más casos según tus necesidades
                            default:
                                break;
                        }
                                proveVM.Proveedor.Moneda = moneda;
                        proveVM.Proveedor.Folio = folio;
                        proveVM.Proveedor.Monto = total;
                        proveVM.Proveedor.Estatus =statusBase;
                        proveVM.Proveedor.idComplementoFK = 1;
                        proveVM.Proveedor.metodoPago = metodopago;
                        if (metodopago=="PPD")
                        {
                            proveVM.Proveedor.statusComplemento = "S";

                        }
                        else
                        {
                            proveVM.Proveedor.statusComplemento = "NA";
                        }
                        proveVM.Proveedor.Solicitante = "Solicitante_Ejemplo";
                        proveVM.Proveedor.nombreProveedor = razonsocial;
                        proveVM.Proveedor.UUIDF = uuidf;
                        proveVM.Proveedor.FechaRegistro = DateTime.Now;
                        proveVM.Proveedor.fechaPago = DateTime.Now.AddMonths(-1);


                        proveVM.Proveedor.fechaProximaPago = DateTime.Now;




                        proveVM.Proveedor.fechaFactura = fechaConvertida;
                        proveVM.Proveedor.XmlUrl = @"\Documentos\" + year + @"\" + month + @"\" + rfc + @"\" + nombreArchivoXml;
                        proveVM.Proveedor.PdfUrl = @"\Documentos\" + year + @"\" + month + @"\" + rfc + @"\" + nombreArchivoPdf;
                        proveVM.Proveedor.Rfc = rfc;
                        _contenedorTrabajo.Proveedor.add(proveVM.Proveedor);
                        
                       

                    }
                    else if (tipo == "P")
                    {
                        var usuarioI = _userManager.GetUserAsync(User).Result;
                        var rfcusuario = usuarioI.Rfc;
                        if (rfcusuario != rfc && !User.IsInRole(CNT.Admin))
                        {
                            EliminarArchivos(rutaArchivoXmlRenombrado, rutaArchivoPdfRenombrado);
                            var message = "La factura no le pertenece al usuario logeado";
                            TempData["AlertMessage"] = message;
                            return View(proveVM);
                        }
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
                                    //verifica que no suban el mismo complemento 
                                    var uuidfExistente = _contenedorTrabajo.Complemento.GetAll().Any(p => p.UUIDC == uuid);
                                    if (uuidfExistente)
                                    {
                                        EliminarArchivos(rutaArchivoXmlRenombrado, rutaArchivoPdfRenombrado);
                                        ModelState.AddModelError(string.Empty, "Ya existe un registro de esta factura");
                                        return View(proveVM);
                                    }

                                    // Guardar los IdDocumento en una lista
                                    var idDocumentos = xmlData.Descendants(pago20 + "DoctoRelacionado")
                                        .Select(docto => (string)docto.Attribute("IdDocumento"))
                                        .ToList();

                                    // Guardar los IdDocumento en una lista
                                    var folioc = xmlData.Descendants(pago20 + "DoctoRelacionado")
                                        .Select(docto => (string)docto.Attribute("Folio"))
                                        .ToList();

                                    //Manejar la logica de si algun elemento de la lista esta en los registros de proveedores cambie el StatusComplemento a C
                                    var proveedoresConIdDocumento = _contenedorTrabajo.Proveedor.GetAll().Where(p => idDocumentos.Contains(p.UUIDF)).ToList();

                                    // Validar si todos los elementos de idDocumentos están en proveedoresConIdDocumento
                                    if (idDocumentos.All(id => proveedoresConIdDocumento.Any(p => p.UUIDF == id)))
                                    {
                                        foreach (var proveedor in proveedoresConIdDocumento)
                                        {
                                            proveedor.statusComplemento = "C";
                                            proveedor.Estatus = "Complemento";
                                            _contenedorTrabajo.Proveedor.Update(proveedor);

                                        }
                                    }
                                    else
                                    {
                                        // No todos los elementos de idDocumentos están en proveedoresConIdDocumento
                                        EliminarArchivos(rutaArchivoXmlRenombrado, rutaArchivoPdfRenombrado);
                                        // Obtener los folios no encontrados
                                        var foliosNoEncontrados = idDocumentos
                                            .Where(id => !proveedoresConIdDocumento.Any(p => p.UUIDF == id))
                                            .Select(id => folioc[idDocumentos.IndexOf(id)])
                                            .ToList();

                                        // Construir el mensaje de error con los folios no encontrados
                                        var message = $"No se han subido todas las facturas que respalda este complemento. Los siguientes folios no se encontraron en la base de datos: {string.Join(", ", foliosNoEncontrados)}";
                                        TempData["AlertMessage"] = message;
                                        return View(proveVM);

                                    }
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
                                    proveVM.Complemento.Rfc = rfc;
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

                    var messagesu = "Factura subida correctamente";
                    TempData["SuccessMessage"] = messagesu;
                    // Mostrar la alerta de éxito antes de redirigir al índice
                    TempData["ShowSuccessAlert"] = true;
                }

                //aqui truene si no sube los dos archivos
                var message_ar = "Suba la factura en los formatos Pdf y Xml correspondientemente ";
                TempData["AlertMessage"] = message_ar;
            }
            //return RedirectToAction(nameof(Index));
            return View(proveVM);
        }



        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
                proveVM.Proveedor.Notas = proveedorExistente.Notas;
                proveVM.Proveedor.Monto = proveedorExistente.Monto;
                proveVM.Proveedor.Estatus = proveedorExistente.Estatus;
                proveVM.Proveedor.UUIDF=proveedorExistente.UUIDF;
                proveVM.Proveedor.PdfUrl = proveedorExistente.PdfUrl;
                proveVM.Proveedor.XmlUrl = proveedorExistente.XmlUrl;
                proveVM.Proveedor.metodoPago = proveedorExistente.metodoPago;
                proveVM.Proveedor.Solicitante = proveedorExistente.Solicitante;
                proveVM.Proveedor.nombreProveedor = proveedorExistente.nombreProveedor;
                proveVM.Proveedor.Moneda = proveedorExistente.Moneda;
                proveVM.Proveedor.Rfc = proveedorExistente.Rfc;
                proveVM.Proveedor.idComplementoFK= proveedorExistente.idComplementoFK;


                // Actualizar otras propiedades del proveedor
                proveVM.Proveedor.FechaRegistro = DateTime.Now;
                proveVM.Proveedor.fechaPago = DateTime.Now;
                proveVM.Proveedor.fechaProximaPago = DateTime.Now;
                proveVM.Proveedor.fechaFactura =proveedorExistente.fechaFactura;
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
            var usuario = _userManager.GetUserAsync(User).Result;
            var rfcUsuario = usuario.Rfc;
            IEnumerable<Proveedor> proveedores;

            if (User.IsInRole(CNT.Admin))
            {
                proveedores = _contenedorTrabajo.Proveedor.GetAll(includeProperties: "Complemento");
            }
            else
            {
                proveedores = _contenedorTrabajo.Proveedor.GetAll(filter: p => p.Rfc == rfcUsuario, includeProperties: "Complemento");
            }

            return Json(new { data = proveedores });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var proveedorExistente = _contenedorTrabajo.Proveedor.Get(id);

            if (proveedorExistente == null)
            {
                return Json(new { success = false, message = "Factura no encontrado" });
            }

            if (proveedorExistente.Estatus != "Revision")
            {
                return Json(new { success = false, message = "No se puede eliminar la factura en el actual estatus" });
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


        private void EliminarArchivo(string rutaArchivoXmlRenombrado, string archivoUrl)
        {
            if (!string.IsNullOrEmpty(archivoUrl))
            {
                var filePath = Path.Combine(rutaArchivoXmlRenombrado, archivoUrl.TrimStart('\\'));

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
        // Método para eliminar archivos en caso de algun error
        private void EliminarArchivos(string rutaArchivoXmlRenombrado, string rutaArchivoPdfRenombrado)
        {
            string rutaXml = Path.Combine(rutaArchivoXmlRenombrado);
            string rutaPdf = Path.Combine(rutaArchivoPdfRenombrado);

            if (System.IO.File.Exists(rutaXml))
            {
                System.IO.File.Delete(rutaXml);
            }

            if (System.IO.File.Exists(rutaPdf))
            {
                System.IO.File.Delete(rutaPdf);
            }
        }

        #endregion
    }
}
