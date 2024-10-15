﻿using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Repositories.Interfaces;
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
            .Include(c => c.Cities)
            .ToListAsync();
            return new ActionResponse<IEnumerable<State>>
            {
                WasSuccess = true,
                Result = states
            };
        }
    }
}