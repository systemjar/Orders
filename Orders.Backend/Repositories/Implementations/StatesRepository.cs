using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations
{
    public class StatesRepository : GenericRepository<State>, IStatesRepository
    {
        private readonly DataContext _context;

        public StatesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<State>> GetAsync(int id)
        {
            //Usamos Include para incluir los estados por relacion directa
            //Usamos ThenInclude para incluir cada ciudad contenida en cada estado de cada pais
            var state = await _context.States
            .Include(c => c.Cities)
            .FirstOrDefaultAsync(c => c.Id == id);
            if (state == null)
            {
                return new ActionResponse<State>
                {
                    WasSuccess = false,
                    Message = "Estado no existe"
                };
            }
            return new ActionResponse<State>
            {
                WasSuccess = true,
                Result = state
            };
        }

        public override async Task<ActionResponse<IEnumerable<State>>> GetAsync()
        {
            //Utilizamos Include como inner join para unir los estados de cada estado
            var states = await _context.States
                .OrderBy(s => s.Name)
                .Include(c => c.Cities)
                .ToListAsync();
            return new ActionResponse<IEnumerable<State>>
            {
                WasSuccess = true,
                Result = states
            };
        }

        public override async Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.States
            .Include(x => x.Cities)
            .Where(x => x.Country!.Id == pagination.Id)
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<State>>
            {
                WasSuccess = true,
                Result = await queryable
            .OrderBy(x => x.Name)
            .Paginate(pagination)
            .ToListAsync()
            };
        }

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.States
            .Where(x => x.Country!.Id == pagination.Id)
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);

            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }
    }
}