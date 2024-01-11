using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Elfie.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogCore.Areas.Admin.Controllers
{
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
                Proveedor = new BlogCore.Models.Proveedor()
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

                if (proveVM.Proveedor.OrdenCompra == 0 && archivos.Count == 2)
                {
                    // Nueva proveedor con dos archivos
                    string nombreArchivo1 = Guid.NewGuid().ToString();
                    string nombreArchivo2 = Guid.NewGuid().ToString();
                    var subida1 = Path.Combine(rutaPrincipal, @"Documentos\Xml");
                    var subida2 = Path.Combine(rutaPrincipal, @"Documentos\Pdf");

                    var extension1 = Path.GetExtension(archivos[0].FileName);
                    var extension2 = Path.GetExtension(archivos[1].FileName);

                    // Verifica las extensiones de los archivos
                    if (extension1 != ".xml" || extension2 != ".pdf")
                    {
                        ModelState.AddModelError(string.Empty, "Los formatos de archivos no corresponden");
                        return View(proveVM);
                    }


                    using (var fileStream1 = new FileStream(Path.Combine(subida1, nombreArchivo1 + extension1), FileMode.Create))
                    using (var fileStream2 = new FileStream(Path.Combine(subida2, nombreArchivo2 + extension2), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream1);
                        archivos[1].CopyTo(fileStream2);
                    }
                    proveVM.Proveedor.FechaRegistro = DateTime.Now.ToString();
                    proveVM.Proveedor.fechaPago = DateTime.Now.ToString();
                    proveVM.Proveedor.XmlUrl = @"\Documentos\Xml\" + nombreArchivo1 + extension1;
                    proveVM.Proveedor.PdfUrl = @"\Documentos\Pdf\" + nombreArchivo2 + extension2;

                    _contenedorTrabajo.Proveedor.add(proveVM.Proveedor);
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
                Proveedor proveedorExistente = _contenedorTrabajo.Proveedor.Get(proveVM.Proveedor.OrdenCompra);

                // Copiar los valores que no se están editando desde el proveedor existente al proveedor recibido
                proveVM.Proveedor.Solicitante = proveedorExistente.Solicitante;
                proveVM.Proveedor.Moneda = proveedorExistente.Moneda;
                proveVM.Proveedor.Monto = proveedorExistente.Monto;
                proveVM.Proveedor.Folio = proveedorExistente.Folio;
                proveVM.Proveedor.Estatus = proveedorExistente.Estatus;
                proveVM.Proveedor.nombreProveedor = proveedorExistente.nombreProveedor;
                proveVM.Proveedor.comentariosSeguimiento = proveedorExistente.comentariosSeguimiento;
                proveVM.Proveedor.PdfUrl = proveedorExistente.PdfUrl;
                proveVM.Proveedor.XmlUrl = proveedorExistente.XmlUrl;
                proveVM.Proveedor.Complemento = proveedorExistente.Complemento;

                // Actualizar otras propiedades del proveedor
                proveVM.Proveedor.FechaRegistro = DateTime.Now.ToString();
                proveVM.Proveedor.fechaPago = DateTime.Now.ToString();

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
            return Json(new { data = _contenedorTrabajo.Proveedor.GetAll() });
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

            // Elimina el proveedor de la base de datos
            _contenedorTrabajo.Proveedor.Remove(proveedorExistente);
            _contenedorTrabajo.Save();

            return Json(new { success = true, message = "Proveedor borrado correctamente" });
        }

        private void EliminarArchivo(string rutaDirectorioPrincipal, string archivoUrl)
        {
            if (!string.IsNullOrEmpty(archivoUrl))
            {
                var filePath = Path.Combine(rutaDirectorioPrincipal, archivoUrl.TrimStart('\\'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }


        #endregion
    }
}
