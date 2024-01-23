using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Usuario
    {
        [Key]
        public int idUsuario { get; set; }

        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [Display(Name = "Rfc")]
        public string Rfc { get; set; }


        [Required]
        public int idRolFK { get; set; }

        [ForeignKey("idRolFK")]
        public rolUsuario rolUsuario { get; set; }

    }
}
