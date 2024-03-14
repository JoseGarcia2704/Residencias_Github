using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlogCore.Areas.Admin.Controllers
{

    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        //trabajar con subida de archivos
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironment)
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
            ArticuloVM artivm = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                 
            };
            return View(artivm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (artiVM.Articulo.Id == 0)
                {
                    //Nuevo Articulo
                    if (archivos.Count != 2)
                    {
                        var message = "Se requieren cargar dos archivos.";
                        TempData["AlertMessage"] = message;
                        return View(artiVM);
                    }

                    var extension1 = Path.GetExtension(archivos[0].FileName);
                    var extension2 = Path.GetExtension(archivos[1].FileName);

                    if ((extension1 != ".jpg" && extension1 != ".jpeg" && extension1 != ".png") || (extension2 != ".pdf"))
                    {
                        var message = "El formato de archivos no corresponde";
                        TempData["AlertMessage"] = message;
                        return View(artiVM);
                    }

                    String nombreArchivo1 = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff");
                    String nombreArchivo2 = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff");


                    var subida1 = Path.Combine(rutaPrincipal, @"Documentos\Anuncios", nombreArchivo1 + extension1);
                    var subida2 = Path.Combine(rutaPrincipal, @"Documentos\Anuncios", nombreArchivo2 + extension2);

                    using (var fileStream1 = new FileStream(subida1, FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream1);
                    }

                    using (var fileStream2 = new FileStream(subida2, FileMode.Create))
                    {
                        archivos[1].CopyTo(fileStream2);
                    }

                    artiVM.Articulo.UrlImagen = @"Documentos\Anuncios\" + nombreArchivo1 + extension1;
                    artiVM.Articulo.UrlPDf= @"Documentos\Anuncios\" + nombreArchivo2 + extension2;
                    artiVM.Articulo.Descripcion = "NA";
                    artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();
                    _contenedorTrabajo.Articulo.add(artiVM.Articulo);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(artiVM);
        }



        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM artivm = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),

            };
            if (id != null)
            {
                artivm.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
            }
            return View(artivm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(artiVM.Articulo.Id);

                if (archivos.Count == 2)
                {
                    var extension1 = Path.GetExtension(archivos[0].FileName);
                    var extension2 = Path.GetExtension(archivos[1].FileName);

                    if ((extension1 != ".jpg" && extension1 != ".jpeg" && extension1 != ".png") || (extension2 != ".pdf"))
                    {
                        var message = "El formato de archivos no corresponde";
                        TempData["AlertMessage"] = message;
                        return View(artiVM);
                    }

                    String nombreArchivo1 = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff");
                    String nombreArchivo2 = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff");

                    var subida1 = Path.Combine(rutaPrincipal, @"Documentos\Anuncios", nombreArchivo1 + extension1);
                    var subida2 = Path.Combine(rutaPrincipal, @"Documentos\Anuncios", nombreArchivo2 + extension2);

                    using (var fileStream1 = new FileStream(subida1, FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream1);
                    }

                    using (var fileStream2 = new FileStream(subida2, FileMode.Create))
                    {
                        archivos[1].CopyTo(fileStream2);
                    }

                    string rutaImagenAntigua = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));
                    string rutaPdfAntiguo = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlPDf.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagenAntigua))
                    {
                        System.IO.File.Delete(rutaImagenAntigua);
                    }

                    if (System.IO.File.Exists(rutaPdfAntiguo))
                    {
                        System.IO.File.Delete(rutaPdfAntiguo);
                    }

                    artiVM.Articulo.UrlImagen = @"Documentos\Anuncios\" + nombreArchivo1 + extension1;
                    artiVM.Articulo.UrlPDf = @"Documentos\Anuncios\" + nombreArchivo2 + extension2;
                }
                else
                {
                    // Mantener las URL existentes si no se suben archivos nuevos
                    artiVM.Articulo.UrlImagen = articuloDesdeDb.UrlImagen ?? string.Empty;
                    artiVM.Articulo.UrlPDf = articuloDesdeDb.UrlPDf ?? string.Empty;
                }

                artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();
                artiVM.Articulo.Descripcion = "NA";
                _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(artiVM);
        }




        #region 
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Articulo.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var articuloDesdeDb= _contenedorTrabajo.Articulo.Get(id);
            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));
            var rutaPdf= Path.Combine(rutaDirectorioPrincipal,articuloDesdeDb.UrlPDf.TrimStart('\\'));
            if (System.IO.File.Exists(rutaImagen))
            {                System.IO.File.Delete(rutaImagen);
            }
            if (System.IO.File.Exists(rutaPdf))
            {
                System.IO.File.Delete(rutaPdf);
            }
            if (articuloDesdeDb == null)
            {
                return Json(new { success = false, Message = "Error borrando articulo" });

            }

            
            _contenedorTrabajo.Articulo.Remove(articuloDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, Message = "Articulo Borrada Correctamente" });
        }
        #endregion
    }
}
