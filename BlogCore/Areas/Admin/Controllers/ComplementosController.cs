using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Usuario,Admin")]
    [Area("Admin")]
    public class ComplementosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public ComplementosController(IContenedorTrabajo contenedorTrabajo , ApplicationDbContext context, IWebHostEnvironment hostingEnvironment )
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;
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
        [HttpGet]
        public IActionResult VerPdf(int id)
        {
            var complemento = _contenedorTrabajo.Complemento.Get(id);

            if (complemento == null)
            {
                return NotFound(); // O manejar de acuerdo a tus necesidades
            }
            var rutaPdf = _hostingEnvironment.WebRootPath + "\\" + complemento.PdfUrl.TrimStart('\\');
            //var rutaPdf = Path.Combine(_hostingEnvironment.WebRootPath, complemento.PdfUrl.TrimStart('\\'));

            if (!System.IO.File.Exists(rutaPdf))
            {
                return NotFound(); // O manejar de acuerdo a tus necesidades
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(rutaPdf);
            string fileName = "Complemento.pdf";

            // Cambiar el ContentDisposition para abrir en otra pestaña
            var contentDisposition = new ContentDisposition
            {
                FileName = fileName,
                Inline = false,  // Cambiar a true para abrir en línea
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            return File(fileBytes, "application/pdf");
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
            // Eliminar el archivo físico
            var rutaPdf = Path.Combine(_hostingEnvironment.WebRootPath, objFromDb.PdfUrl.TrimStart('\\'));
            if (System.IO.File.Exists(rutaPdf))
            {
                System.IO.File.Delete(rutaPdf);
            }
            var rutaXml = Path.Combine(_hostingEnvironment.WebRootPath, objFromDb.XmlUrl.TrimStart('\\'));
            if (System.IO.File.Exists(rutaXml))
            {
                System.IO.File.Delete(rutaXml);
            }
            _contenedorTrabajo.Complemento.Remove(objFromDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, Message = "Complemento Borrada Correctamente" });
        }



        #endregion
    }
}
