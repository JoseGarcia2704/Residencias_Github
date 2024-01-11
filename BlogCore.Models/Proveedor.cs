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
        public string Solicitante { get; set; }


        [Display(Name ="Moneda")]
        public string Moneda { get; set; }


        [Display(Name = "Monto")]
        public int Monto { get; set; }


        [Display(Name = "Folio")]
        public string Folio { get; set; }


        [Display(Name = "Estatus")]
        public string Estatus { get; set; }


        [Display(Name = "Fecha de pago")]
        public string fechaPago { get; set; }


        [Display(Name = "Nombre del proveedor")]
        public string nombreProveedor { get; set; }


        public string Notas { get; set; }


        public string comentariosSeguimiento { get; set; }



        public string Complemento { get; set; }


        [DataType(DataType.ImageUrl)]
        [Display(Name = "Pdf")]
        public string PdfUrl { get; set; }

       
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Xml")]
        public string XmlUrl { get; set; }
    }
}
