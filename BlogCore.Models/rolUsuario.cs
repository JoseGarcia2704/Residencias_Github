using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class rolUsuario
    {
        [Key]
        public int idRol { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
    }
}
