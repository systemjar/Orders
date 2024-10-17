using Orders.Shared.DTOs;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitOfWork.Interfaces
{
    namespace Orders.Backend.UnitsOfWork.Interfaces
    {
        public interface IGenericUnitOfWork<T> where T : class
        {
            Task<ActionResponse<T>> GetAsync(int id);

            Task<ActionResponse<IEnumerable<T>>> GetAsync();

            //Para obtener una lista con paginacion
            Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination);

            //Para obtener una lista con paginacion necesito saber cuantas pagins tiene la lista
            Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

            Task<ActionResponse<T>> AddAsync(T model);

            Task<ActionResponse<T>> UpdateAsync(T model);

            Task<ActionResponse<T>> DeleteAsync(int id);
        }
    }
}