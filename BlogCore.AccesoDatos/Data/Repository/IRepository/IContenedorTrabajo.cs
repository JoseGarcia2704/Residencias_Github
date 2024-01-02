using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IContenedorTrabajo :IDisposable
    {
        //aqui se debe de ir agregando los diferentes repositorios
        ICategoriaRepository Categoria {  get; }
        IArticuloRepository Articulo { get; }
        
        IProveedorRepository Proveedor { get; }
        void Save();
    }
}
