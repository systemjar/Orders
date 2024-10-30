using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        //Esto es para conectarse a la base de datos
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        //El DbSet se pone en plural del nombre de la tabla
        public DbSet<City> Cities { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<State> States { get; set; }

        //Creamos indices para evitar que se dupliquen los datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Indice compuesto para ciudades
            modelBuilder.Entity<City>().HasIndex(x => new { x.StateId, x.Name }).IsUnique();

            //Indice unico de Categorias
            modelBuilder.Entity<Category>().HasIndex(x => x.Name).IsUnique();

            //Indice unico de Paises
            modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();

            //Indice compuesto para ciudades
            modelBuilder.Entity<State>().HasIndex(x => new { x.CountryId, x.Name }).IsUnique();

            //Desabilitamos el metodo de borrado en cascada
            DisableCascadingDelete(modelBuilder);
        }

        //Implementacion del metodo para eliminar el borrado en cascada
        private void DisableCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}