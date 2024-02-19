using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage ="El rfc es obligatorio")]
        public string Rfc {  get; set; }
        [Required(ErrorMessage = "La razon social es obligatorio")]
        public string razonSocial { get; set; }



    }
}
