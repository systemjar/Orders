using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
    public class SeedDB
    {
        //Propiedad privada para usarla en toda la sollucion
        private readonly DataContext _context;

        //Inyectamos el contexto para acceder a la base de datos
        public SeedDB(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            //Asegurarse de que hay Base de datos, crea un update-database por codigo
            await _context.Database.EnsureCreatedAsync();

            //Metodo para crear paises
            await CheckCountriesAsync();
            //Metodo para crear categorias
            await CheckCategoriesAsync();
        }

        private async Task CheckCategoriesAsync()
        {
            //Si no hay ninguna categoria
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Deportes" });
                _context.Categories.Add(new Category { Name = "Hogar" });
                _context.Categories.Add(new Category { Name = "Jardin" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCountriesAsync()
        {
            //Si no hay ningun pais
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country { Name = "Guatemala" });
                _context.Countries.Add(new Country { Name = "Honduras" });
                _context.Countries.Add(new Country { Name = "Nicaragua" });
                await _context.SaveChangesAsync();
            }
        }
    }
}