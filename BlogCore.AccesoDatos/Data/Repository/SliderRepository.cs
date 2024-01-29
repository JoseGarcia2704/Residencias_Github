using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;

namespace BlogCore.AccesoDatos.Data.Repository
{
    internal class SliderRepository : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _db;

        public SliderRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

      

        public void Update(Slider slider)
        {
           var objetoDesdeDb=_db.Slider.FirstOrDefault(s => s.idSlider == slider.idSlider);
            objetoDesdeDb.Nombre=slider.Nombre;
            objetoDesdeDb.Estado = slider.Estado;
            objetoDesdeDb.UrlImagen=slider.UrlImagen;
            _db.SaveChanges();
        }

       

    }
}
