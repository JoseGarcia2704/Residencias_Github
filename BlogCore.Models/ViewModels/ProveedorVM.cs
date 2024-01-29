using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models.ViewModels
{
    public class ProveedorVM
    {
        public Proveedor Proveedor { get; set; }
        public IEnumerable<SelectListItem> ListaComplemento { get; set; }
        //public IEnumerable<SelectListItem> ListaUsuario { get; set; }

    }
}
