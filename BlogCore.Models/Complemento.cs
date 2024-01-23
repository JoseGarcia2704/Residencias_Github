using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
   
    public class Complemento
    {
        [Key]
        public int idComplemento { get; set; }

        [Display(Name = "UUIDC")]
        public string UUIDC { get; set; }

        [Display(Name = "Monto")]
        public float Monto { get; set; }

        [Display(Name = "saldoInsoluto")]
        public float saldoInsoluto { get; set; }
    }
}
