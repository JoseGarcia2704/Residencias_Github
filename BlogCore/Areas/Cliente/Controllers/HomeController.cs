using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogCore.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;

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

      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}