using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlogCore.Areas.Admin.Controllers
{
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
        public IActionResult AbrirPdf(int id)
        {
            var articulo = _contenedorTrabajo.Articulo.Get(id);

            if (articulo == null)
            {
                return NotFound(); // O manejar de acuerdo a tus necesidades
            }

            var rutaPdf = Path.Combine(_hostingEnvironment.WebRootPath, articulo.UrlImagen.TrimStart('\\'));

            if (!System.IO.File.Exists(rutaPdf))
            {
                return NotFound(); // O manejar de acuerdo a tus necesidades
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(rutaPdf);
            string fileName = "Anuncio.pdf";

            return File(fileBytes, "application/pdf", fileName);
        }






        [HttpGet]
        public IActionResult Create()
        {
            ArticuloVM artivm = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                 ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
                 
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
                    String nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"Documentos\Anuncios");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    if (extension != ".pdf" )
                    {
                        ModelState.AddModelError(string.Empty, "Solo se permiten archivos con extensión .pdf");
                        artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
                        return View(artiVM);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    artiVM.Articulo.UrlImagen = @"Documentos\Anuncios\" + nombreArchivo + extension;
                    artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();
                    _contenedorTrabajo.Articulo.add(artiVM.Articulo);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(artiVM);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM artivm = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()

            };
            if (id != null)
            {
                artivm.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
            }
            return View(artivm);
        }


        public IActionResult Edit(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(artiVM.Articulo.Id);

                if (archivos.Count() > 0)
                {
                    // Nueva imagen para el artículo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"Documentos\Anuncios");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    if (extension != ".pdf")
                    {
                        ModelState.AddModelError(string.Empty, "Solo se permiten archivos con extensión .pdf");
                        artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
                        return View(artiVM);
                    }

                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    // Nuevamente subir el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    artiVM.Articulo.UrlImagen = Path.Combine("Documentos", "Anuncios", nombreArchivo + extension);
                    artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Aquí sería cuando la imagen ya existe y se conserva
                    artiVM.Articulo.UrlImagen = articuloDesdeDb.UrlImagen;
                }

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
            return Json(new { data = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria") });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var articuloDesdeDb= _contenedorTrabajo.Articulo.Get(id);
            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
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
