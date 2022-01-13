using Airbnb_PWEB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Airbnb_PWEB.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Property> Properties { get; set; } 

        public DbSet<PropertyImage> PropertyImages { get; set; }    

        public DbSet<Reservation> Reservations { get; set; }    

        public DbSet<Evaluation> Evaluation { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Company> Companies { get; set;}

        public DbSet<ClientEvaluation> ClientEvaluations { get; set; }
    }
}
