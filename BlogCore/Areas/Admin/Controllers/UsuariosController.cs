using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]

    public class UsuariosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            //opcion 1: obtener todos los usuarios
            //return View(_contenedorTrabajo.Usuario.GetAll());

            // Opción 2: obtener usuarios menos el autenticado
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            // Verificar si claimsIdentity y usuarioActual son diferentes de null
            if (claimsIdentity != null)
            {
                var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                // Verificar si usuarioActual no es nulo antes de acceder a su propiedad Value
                if (usuarioActual != null)
                {
                    return View(_contenedorTrabajo.Usuario.GetAll(u => u.Id != usuarioActual.Value));
                }
            }

            // Manejar el caso en que claimsIdentity o usuarioActual son nulos
            // Puedes devolver una vista de error o realizar otra acción adecuada.
            return RedirectToAction("Error");
        }


        [HttpGet]
        public IActionResult Bloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.BloquearUsuario(id);
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public IActionResult Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
            return RedirectToAction(nameof(Index));

        }
    }
}
