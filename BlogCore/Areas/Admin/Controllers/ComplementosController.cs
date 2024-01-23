using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ComplementosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;

        public ComplementosController(IContenedorTrabajo contenedorTrabajo , ApplicationDbContext context)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;
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
        public IActionResult Create(Complemento complemento)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Complemento.add(complemento);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(complemento);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Complemento complemento =new Complemento();
            complemento = _contenedorTrabajo.Complemento.Get(id);
            if(complemento==null)
            {
                return NotFound();
            }
            return View(complemento);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Complemento complemento)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Complemento.Update(complemento);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(complemento);
        }

        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Complemento.GetAll() });
        }
       
        
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _contenedorTrabajo.Complemento.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, Message = "Error borrando Complemento" });

            }
            _contenedorTrabajo.Complemento.Remove(objFromDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, Message = "Complemento Borrada Correctamente" });
        }



        #endregion
    }
}
