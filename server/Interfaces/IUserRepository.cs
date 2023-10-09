using Microsoft.AspNetCore.Identity;
using server.DTOs;
using server.Models;

namespace server.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetById(int id);

        Task<bool> EmailExists(string email);
        Task<IdentityResult> Add(User user);
        void Update(User user);
        void Delete(User user);


    }
}
