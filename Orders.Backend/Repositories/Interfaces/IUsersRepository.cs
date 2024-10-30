using Microsoft.AspNetCore.Identity;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        //Buscar el usuario por email
        Task<User> GetUserAsync(string email);

        //Adiciona un usuario con su password y regresa si fue existosa la operacion
        Task<IdentityResult> AddUserAsync(User user, string password);

        //Chequea si existe el rol, si no existe lo crea
        Task CheckRoleAsync(string roleName);

        //Asigna ese rol al usuario
        Task AddUserToRoleAsync(User user, string roleName);

        //Revisa si ese usuario pertenece a ese rol
        Task<bool> IsUserInRoleAsync(User user, string roleName);

        //Para poder hacer Login
        Task<SignInResult> LoginAsync(LoginDTO model);

        //Para Logout
        Task LogoutAsync();
    }
}