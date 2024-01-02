using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
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
            objetoDesdeDb.Solicitante=proveedor.Solicitante;
            objetoDesdeDb.Moneda=proveedor.Moneda;
            objetoDesdeDb.Monto=proveedor.Monto;
            objetoDesdeDb.Folio=proveedor.Folio;
            objetoDesdeDb.Estatus=proveedor.Estatus;
            objetoDesdeDb.nombreProveedor=proveedor.nombreProveedor;
            objetoDesdeDb.Notas=proveedor.Notas;
            objetoDesdeDb.comentariosSeguimiento = proveedor.comentariosSeguimiento;
            objetoDesdeDb.Complemento=proveedor.Complemento;
            objetoDesdeDb.XmlUrl=proveedor.XmlUrl;
            objetoDesdeDb.PdfUrl=proveedor.PdfUrl;
           // _db.SaveChanges();
        }

    }
}
