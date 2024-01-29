using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        //trabajar con subida de archivos
        private readonly IWebHostEnvironment _hostingEnvironment;
        public SlidersController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironment)
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
          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
               
                    //Nuevo Slider
                    String nombreArchivo=Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    {
                        ModelState.AddModelError(string.Empty, "Solo se permiten archivos con extensión .jpg, .jpeg o .png");
                        return View();
                    }

                    using (var fileStreams= new FileStream(Path.Combine(subidas, nombreArchivo + extension),FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    slider.UrlImagen=@"\imagenes\sliders\"+nombreArchivo+extension;
                    _contenedorTrabajo.Slider.add(slider);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
           
            if (id != null)
            {
                var slider = _contenedorTrabajo.Slider.Get(id.GetValueOrDefault());
                return View(slider);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var sliderDesdeDb = _contenedorTrabajo.Slider.Get(slider.idSlider);

                if (archivos.Count() > 0)
                {
                    //Nueva imagen para el slider
                    String nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    if (extension != ".jpeg" && extension != ".png")
                    {
                        ModelState.AddModelError(string.Empty, "Solo se permiten archivos con extensión .jpg, .jpeg o .png");
                        return View();
                    }

                    var  rutaImagen=Path.Combine(rutaPrincipal, sliderDesdeDb.UrlImagen.TrimStart('\\'));
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }


                    //Nuevamente subir el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;
                  
                    _contenedorTrabajo.Slider.Update(slider);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Aqui seria cuando la imagen ya existe y se conserva
                    slider.UrlImagen = sliderDesdeDb.UrlImagen;
                    
                }
                _contenedorTrabajo.Slider.Update(slider);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        #region 
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Slider.GetAll()});
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var sliderDesdeDb= _contenedorTrabajo.Slider.Get(id);
            
            if (sliderDesdeDb == null)
            {
                return Json(new { success = false, Message = "Error borrando slider" });

            }

            
            _contenedorTrabajo.Slider.Remove(sliderDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, Message = "Slider Borrada Correctamente" });
        }
        #endregion
    }
}
