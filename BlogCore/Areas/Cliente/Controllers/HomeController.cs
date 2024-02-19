using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Diagnostics;

namespace BlogCore.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public HomeController(IContenedorTrabajo contenedorTrabajo,  IWebHostEnvironment hostingEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;

        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Slider=_contenedorTrabajo.Slider.GetAll(),
                ListaArticulos = _contenedorTrabajo.Articulo.GetAll()
                
            };

            //esta linea es para poder saber si estamos en el home o no
            ViewBag.IsHome = true;

            return View(homeVM);
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

            // Cambiar el ContentDisposition para abrir en otra pestaña
            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = false,  // Abrir en otra pestaña
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            return File(fileBytes, "application/pdf");
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}