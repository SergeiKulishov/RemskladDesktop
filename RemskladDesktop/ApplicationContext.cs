using System;
using Microsoft.EntityFrameworkCore;


namespace RemskladDesktop

{
    public class ApplicationContext : DbContext
    {
        public DbSet<Datum> Datums { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=remskladapi;Trusted_Connection=True;");
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=greatsteve");

        }
    }
}
