using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SamiApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamiApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        } 
        public DbSet<Client> clients { get; set; }
        public DbSet<homePage> homePages { get; set; }
        public DbSet<Login> logins { get; set; }
        public DbSet<Register> registers { get; set; }
    }
           
}
