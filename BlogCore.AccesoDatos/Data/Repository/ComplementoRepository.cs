using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    internal class ComplementoRepository : Repository<Complemento>, IComplementoRepository
    {
        private readonly ApplicationDbContext _db;

        public ComplementoRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetListaCategorias()
        {
        return _db.Complemento.Select(i=>new SelectListItem()
            {
            Text = i.UUIDC,
            Value = i.idComplemento.ToString()
            }
          );
        }


        public void Update(Complemento complemento)
        {
            var objetoDesdeDb = _db.Complemento.FirstOrDefault(s => s.idComplemento == complemento.idComplemento);


            objetoDesdeDb.UUIDC = complemento.UUIDC;
            objetoDesdeDb.Monto = complemento.Monto;
            objetoDesdeDb.saldoInsoluto = complemento.saldoInsoluto;

            _db.SaveChanges();
        }


    }
}
