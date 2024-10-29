using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Interfaces
{
    public interface ICountriesRepository
    {
        //Sobre escribimos los metodos GetAsync() porque necesitamos que el Country traiga de regreso el estado
        Task<ActionResponse<Country>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<Country>>> GetAsync();

        Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}