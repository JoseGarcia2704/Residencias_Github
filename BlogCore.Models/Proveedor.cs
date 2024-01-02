using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlogCore.Models
{
    public class Proveedor
    {
        [Key]
        public int OrdenCompra { get; set; }

        [Display(Name = "Fecha de registro")]
        public string FechaRegistro { get; set; }


        [Display(Name ="Nombre del solicitante")]
        [Required(ErrorMessage = "El nombre del socilitante es obligatorio")]
        public string Solicitante { get; set; }


        [Display(Name ="Moneda")]
        [Required(ErrorMessage = "La moneda es obligatoria")]
        public string Moneda { get; set; }


        [Display(Name = "Monto")]
        [Required(ErrorMessage = "El monto es obligatorio")]
        public int Monto { get; set; }


        [Display(Name = "Folio")]
        [Required(ErrorMessage = "El folio es obligatoria")]
        public string Folio { get; set; }


        [Display(Name = "Estatus")]
        [Required(ErrorMessage = "El estatus es obligatoria")]
        public string Estatus { get; set; }


        [Display(Name = "Fecha de pago")]
        public string fechaPago { get; set; }


        [Display(Name = "Nombre del proveedor")]
        [Required(ErrorMessage = "El nombre del proveedor es obligatorio")]
        public string nombreProveedor { get; set; }


        public string Notas { get; set; }


        public string comentariosSeguimiento { get; set; }



        [Required(ErrorMessage = "El complemento de pago es obligatoria")]
        public string Complemento { get; set; }


        [DataType(DataType.ImageUrl)]
        [Display(Name = "Pdf")]
        public string PdfUrl { get; set; }

       
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Xml")]
        public string XmlUrl { get; set; }
    }
}
