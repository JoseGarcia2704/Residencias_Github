using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Slider
    {
        [Key]
        public int idSlider { get; set; }
        
        [Required(ErrorMessage = "Ingrese un nombre para el slider")]
        [Display(Name = "Nombre del Articulo")]
        public String Nombre { get; set; }

        
        [Display(Name = "Estado")]
        public bool Estado { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string UrlImagen { get; set; }

       
    }
}
