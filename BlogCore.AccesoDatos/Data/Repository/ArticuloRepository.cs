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
    internal class ArticuloRepository : Repository<Articulo>, IArticuloRepository
    {
        private readonly ApplicationDbContext _db;

        public ArticuloRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

      

        public void Update(Articulo articulo)
        {
           var objetoDesdeDb=_db.Articulo.FirstOrDefault(s => s.Id == articulo.Id);
            objetoDesdeDb.Nombre=articulo.Nombre;
            objetoDesdeDb.Descripcion = articulo.Descripcion;
            objetoDesdeDb.UrlImagen=articulo.UrlImagen;
            objetoDesdeDb.UrlPDf=articulo.UrlPDf;
           // _db.SaveChanges();
        }

    }
}
