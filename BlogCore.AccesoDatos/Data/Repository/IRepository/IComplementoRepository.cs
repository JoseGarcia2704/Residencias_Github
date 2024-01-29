﻿using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IComplementoRepository :IRepository<Complemento>
    {
        IEnumerable<SelectListItem> GetListaComplemento();
        void Update(Complemento complemento);
    }
}
