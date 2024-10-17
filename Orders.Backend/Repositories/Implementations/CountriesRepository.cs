using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations
{
    //El CountriesRepository hereda de la clase GenericRepositori pero tambien implementa los metodos del ICountriesRepository porque son diferentes
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly DataContext _context;

        public CountriesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<Country>> GetAsync(int id)
        {
            //Usamos Include para incluir los estados por relacion directa
            //Usamos ThenInclude para incluir cada ciudad contenida en cada estado de cada pais
            var country = await _context.Countries
            .Include(c => c.States!)
            .ThenInclude(s => s.Cities)
            .FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return new ActionResponse<Country>
                {
                    WasSuccess = false,
                    Message = "País no existe"
                };
            }
            return new ActionResponse<Country>
            {
                WasSuccess = true,
                Result = country
            };
        }

        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync()
        {
            //Utilizamos Include como inner join para unir los estados de cada pais
            var countries = await _context.Countries
                .OrderBy(x => x.Name)
                .Include(c => c.States)
            .ToListAsync();
            return new ActionResponse<IEnumerable<Country>>
            {
                WasSuccess = true,
                Result = countries
            };
        }

        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Countries
            .Include(c => c.States)
            .AsQueryable();
            return new ActionResponse<IEnumerable<Country>>
            {
                WasSuccess = true,
                Result = await queryable
            .OrderBy(x => x.Name)
            .Paginate(pagination)
            .ToListAsync()
            };
        }
    }
}