using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitOfWork.Interfaces
{
    public interface IStatesUnitOfWork
    {
        //Se definen los metodos a sobreescribir por ser diferentes para los estados
        Task<ActionResponse<State>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<State>>> GetAsync();
    }
}