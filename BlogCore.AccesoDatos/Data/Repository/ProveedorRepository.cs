using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BlogCore.AccesoDatos.Data.Repository
{
    internal class ProveedorRepository : Repository<Proveedor>, IProveedorRepository
    {
        private readonly ApplicationDbContext _db;

        public ProveedorRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Update(Proveedor proveedor)
        {
           var objetoDesdeDb=_db.Proveedor.FirstOrDefault(s => s.OrdenCompra == proveedor.OrdenCompra);

            //no se usara por el momento
            // objetoDesdeDb.FechaRegistro=proveedor.FechaRegistro;
            // objetoDesdeDb.fechaPago=proveedor.fechaPago;
            objetoDesdeDb.OrdenCompra = proveedor.OrdenCompra;
            objetoDesdeDb.statusComplemento = proveedor.statusComplemento;
            objetoDesdeDb.Folio = proveedor.Folio;
            objetoDesdeDb.Monto = proveedor.Monto;
            objetoDesdeDb.Notas = proveedor.Notas;
            objetoDesdeDb.Estatus = proveedor.Estatus;
            objetoDesdeDb.UUIDF = proveedor.UUIDF;
            objetoDesdeDb.XmlUrl = proveedor.XmlUrl;
            objetoDesdeDb.PdfUrl = proveedor.PdfUrl;
            objetoDesdeDb.metodoPago = objetoDesdeDb.metodoPago;
            objetoDesdeDb.Solicitante=proveedor.Solicitante;
            objetoDesdeDb.comentariosSeguimiento = proveedor.comentariosSeguimiento;
            objetoDesdeDb.nombreProveedor = proveedor.nombreProveedor;
            objetoDesdeDb.Moneda=proveedor.Moneda;
            objetoDesdeDb.FechaRegistro = proveedor.FechaRegistro;
            objetoDesdeDb.fechaPago = proveedor.fechaPago;
            objetoDesdeDb.fechaProximaPago = proveedor.fechaProximaPago;
            objetoDesdeDb.fechaFactura = proveedor.fechaFactura;
            objetoDesdeDb.idComplementoFK = proveedor.idComplementoFK;
            objetoDesdeDb.idUsuarioFK = proveedor.idUsuarioFK;
           // _db.SaveChanges();
        }

    }
}
