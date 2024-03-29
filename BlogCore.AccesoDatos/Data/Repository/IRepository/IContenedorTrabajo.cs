﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IContenedorTrabajo :IDisposable
    {
        //aqui se debe de ir agregando los diferentes repositorios
        IArticuloRepository Articulo { get; }
        
        IProveedorRepository Proveedor { get; }

        IComplementoRepository Complemento { get; }
        ISliderRepository Slider { get; }
        IUsuarioRepository Usuario { get; }


        void Save();
    }
}
