using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Jobbsy.Models;

namespace Jobbsy.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Jobbsy.Models.Company> Company { get; set; }
        public DbSet<Jobbsy.Models.Photo> Photo { get; set; }
        public DbSet<Jobbsy.Models.Technology> Technology { get; set; }
        public DbSet<Jobbsy.Models.Comment> Comment { get; set; }
    }
}
