using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        //private readonly ApplicationDbContext _context;
        public CategoriasController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
            //_context = context;
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
        public IActionResult Create(Categoria categoria)
        {
            if(ModelState.IsValid)
            {
                _contenedorTrabajo.Categoria.add(categoria);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }
       
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Categoria categoria = new Categoria();
            categoria =_contenedorTrabajo.Categoria.Get(id);
            if(categoria == null)
            {
                return NotFound();
            }


            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Categoria.Update(categoria);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }


        #region 
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new {data=_contenedorTrabajo.Categoria.GetAll()});

        }

        [HttpDelete]
        public IActionResult Delete(int id) {
            var objFromDb = _contenedorTrabajo.Categoria.Get(id);
            if(objFromDb == null)
            {
                return Json(new { success = false,Message="Error borrando Categoria" });

            }
            _contenedorTrabajo.Categoria.Remove(objFromDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, Message = "Categoria Borrada Correctamente" });
        }
        #endregion

    }
}
