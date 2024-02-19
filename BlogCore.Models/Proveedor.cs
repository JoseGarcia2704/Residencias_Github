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
        public int idProveedor { get; set; }


        [Display(Name = "ordenCompra")]
        public int OrdenCompra { get; set; }


        [Display(Name = "statusComplemento")]
        public string statusComplemento { get; set; }


        [Display(Name = "Folio")]
        public string Folio { get; set; }


        [Display(Name = "Monto")]
        public float Monto { get; set; }

        public string Notas { get; set; }


        [Display(Name = "Estatus")]
        public string Estatus { get; set; }

        [Display(Name = "UUIDF")]
        public string UUIDF { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Pdf")]
        public string PdfUrl { get; set; }


        [DataType(DataType.ImageUrl)]
        [Display(Name = "Xml")]
        public string XmlUrl { get; set; }


        [Display(Name = "metodoPago")]
        public string metodoPago { get; set; }


        [Display(Name = "Nombre del solicitante")]
        public string Solicitante { get; set; }


        public string comentariosSeguimiento { get; set; }


        [Display(Name = "Nombre del proveedor")]
        public string nombreProveedor { get; set; }


        [Display(Name = "Moneda")]
        public string Moneda { get; set; }


        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }


        [Display(Name = "Fecha de pago")]
        public DateTime fechaPago { get; set; }


        [Display(Name = "fecha Proxima Pago")]
        public DateTime fechaProximaPago { get; set; }


        [Display(Name = "fecha de Factura")]
        public DateTime fechaFactura { get; set; }



        [Required]
        public int idComplementoFK { get; set; }

        [ForeignKey("idComplementoFK")]
        public Complemento Complemento { get; set; }


        
     





    }
}
