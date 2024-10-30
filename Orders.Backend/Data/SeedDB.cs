using Orders.Backend.UnitOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Enums;
using System.Net;
using System.Reflection.Metadata;

namespace Orders.Backend.Data
{
    public class SeedDB
    {
        //Propiedad privada para usarla en toda la sollucion
        private readonly DataContext _context;

        private readonly IUsersUnitOfWork _usersUnitOfWork;

        //Inyectamos el contexto para acceder a la base de datos
        public SeedDB(DataContext context, IUsersUnitOfWork usersUnitOfWork)
        {
            _context = context;
            _usersUnitOfWork = usersUnitOfWork;
        }

        public async Task SeedAsync()
        {
            //Asegurarse de que hay Base de datos, crea un update-database por codigo
            await _context.Database.EnsureCreatedAsync();

            //Metodo para crear paises
            await CheckCountriesAsync();
            //Metodo para crear categorias
            await CheckCategoriesAsync();

            //Chequear roles
            await CheckRolesAsync();

            //Crear un usuario admin para poder acceder al sistema
            await CheckUsersAsync("1010", "Jorge", "Alcántara", "jar@yopmail.com", "322 311 4620", "Direccion", UserType.Admin);
        }

        private async Task<User> CheckUsersAsync(string document, string firstName, string lastName, string email, string phoneNumber, string address, UserType userType)
        {
            var user = await _usersUnitOfWork.GetUserAsync(email);

            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phoneNumber,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };
                await _usersUnitOfWork.AddUserAsync(user, "123456");
                await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());
            }
            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
            await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
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
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States =
                    [
                        new State()
                        {
                            Name = "Antioquia",
                                Cities =
                                [
                                    new() { Name = "Medellín" },
                                    new() { Name = "Itagüí" },
                                    new() { Name = "Envigado" },
                                    new() { Name = "Bello" },
                                    new() { Name = "Rionegro" },
                                ]
                        },
                        new State()
                        {
                            Name = "Bogotá",
                                Cities =
                                [
                                    new() { Name = "Usaquen" },
                                    new() { Name = "Champinero" },
                                    new() { Name = "Santa fe" },
                                    new() { Name = "Useme" },
                                    new() { Name = "Bosa" },
                                ]
                        },
                    ]
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States =
                    [
                        new State()
                        {
                            Name = "Florida",
                            Cities =
                            [
                                new() { Name = "Orlando" },
                                new() { Name = "Miami" },
                                new() { Name = "Tampa" },
                                new() { Name = "Fort Lauderdale" },
                                new() { Name = "Key West" },
                            ]
                        },
                        new State()
                        {
                            Name = "Texas",
                            Cities =
                            [
                                new() { Name = "Houston" },
                                new() { Name = "San Antonio" },
                                new() { Name = "Dallas" },
                                new() { Name = "Austin" },
                                new() { Name = "El Paso" },
                            ]
                        },
                    ]
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}