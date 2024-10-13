using Microsoft.EntityFrameworkCore;
using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
    public class DataContext : DbContext
    {
        //Esto es para conectarse a la base de datos
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        //El DbSet se pone en plural del nombre de la tabla
        public DbSet<Country> Countries { get; set; }

        public DbSet<Category> Categories { get; set; }

        //Creamos indices para evitar que se dupliquen los datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Indice unico de Paises
            modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();

            //Indice unico de Categorias
            modelBuilder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
        }
    }
}