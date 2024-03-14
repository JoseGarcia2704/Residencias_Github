using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;


        public ComplementosController(IContenedorTrabajo contenedorTrabajo , ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;

        }

        [HttpGet]
        public IActionResult Index()
        {
            var usuario = _userManager.GetUserAsync(User).Result;
            var rfcUsuario = usuario.Rfc;
            IEnumerable<Complemento> complementos;

            if (User.IsInRole(CNT.Admin))
            {
                complementos = _contenedorTrabajo.Complemento.GetAll();
            }
            else
            {
                complementos = _contenedorTrabajo.Complemento.GetAll(filter: c => c.Rfc == rfcUsuario);
            }

            var rfcList = complementos.Select(c => c.Rfc).Distinct().ToList();

            return View(rfcList);
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

            if (!System.IO.File.Exists(rutaPdf))
            {
                return NotFound(); // O manejar de acuerdo a tus necesidades
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(rutaPdf);
            string fileName = "Complemento.pdf";
            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = true,  // Abrir en otra pestaña
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            return File(fileBytes, "application/pdf");
        }





        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            var usuario = _userManager.GetUserAsync(User).Result;
            var rfcUsuario = usuario.Rfc;
            IEnumerable<Complemento> complementos;

            if (User.IsInRole(CNT.Admin))
            {
                complementos = _contenedorTrabajo.Complemento.GetAll();
            }
            else
            {
                complementos = _contenedorTrabajo.Complemento.GetAll(filter: c => c.Rfc == rfcUsuario);
            }

            return Json(new { data = complementos });
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
