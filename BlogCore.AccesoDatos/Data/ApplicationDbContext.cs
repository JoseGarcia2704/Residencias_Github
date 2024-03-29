﻿using BlogCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //Se tiene que migrar cada modelo que se cree
        public DbSet<Articulo> Articulo {  get; set; }
        public DbSet<Proveedor> Proveedor { get; set;}
        public DbSet<Complemento> Complemento { get; set; }
        public DbSet<Slider> Slider { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

    }
}