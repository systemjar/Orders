﻿using Orders.Shared.Entities;

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
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Antioquia",
                                Cities = new List<City>()
                                {
                                    new City() { Name = "Medellín" },
                                    new City() { Name = "Itagüí" },
                                    new City() { Name = "Envigado" },
                                    new City() { Name = "Bello" },
                                    new City() { Name = "Rionegro" },
                                }
                        },
                        new State()
                        {
                            Name = "Bogotá",
                                Cities = new List<City>()
                                {
                                    new City() { Name = "Usaquen" },
                                    new City() { Name = "Champinero" },
                                    new City() { Name = "Santa fe" },
                                    new City() { Name = "Useme" },
                                    new City() { Name = "Bosa" },
                                }
                        },
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Florida",
                            Cities = new List<City>()
                            {
                                new City() { Name = "Orlando" },
                                new City() { Name = "Miami" },
                                new City() { Name = "Tampa" },
                                new City() { Name = "Fort Lauderdale" },
                                new City() { Name = "Key West" },
                            }
                        },
                        new State()
                        {
                            Name = "Texas",
                            Cities = new List<City>()
                            {
                                new City() { Name = "Houston" },
                                new City() { Name = "San Antonio" },
                                new City() { Name = "Dallas" },
                                new City() { Name = "Austin" },
                                new City() { Name = "El Paso" },
                            }
                        },
                    }
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}