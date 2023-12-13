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


        [Required(ErrorMessage = "El nombre del socilitante es obligatorio")]
        public string Solicitante { get; set; }


        [Required(ErrorMessage = "La moneda es obligatoria")]
        public string Moneda { get; set; }



        [Required(ErrorMessage = "El monto es obligatorio")]
        public int Monto { get; set; }



        [Required(ErrorMessage = "El folio es obligatoria")]
        public string Folio { get; set; }



        [Required(ErrorMessage = "El estatus es obligatoria")]
        public string Estatus { get; set; }



        [Required(ErrorMessage = "La Fecha programada de pago es obligatoria")]
        public string fechaPago { get; set; }



        [Required(ErrorMessage = "El nombre del proveedor es obligatorio")]
        public string nombreProveedor { get; set; }


        public string Notas { get; set; }


        public string comentariosSeguimiento { get; set; }



        [Required(ErrorMessage = "El complemento es obligatoria")]
        public string Complemento { get; set; }

        /*
        // Propiedad para el archivo PDF
        [DataType(DataType.Upload)]
        [RegularExpression(@"^.+\.(pdf)$", ErrorMessage = "Por favor, seleccione un archivo PDF válido.")]
        [Display(Name = "Archivo PDF")]
        public IFormFile ArchivoPDF { get; set; }

        // Propiedad para el archivo XML
        [DataType(DataType.Upload)]
        [RegularExpression(@"^.+\.(xml)$", ErrorMessage = "Por favor, seleccione un archivo XML válido.")]
        [Display(Name = "Archivo XML")]
        public IFormFile ArchivoXML { get; set; }


        */
    }
}
