using Microsoft.AspNetCore.Identity;
using server.DTOs;
using server.Models;

namespace server.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Get(int id);
        Task<IdentityResult> Add(User user);
        void Update(User user);
        void Delete(User user);
    }
}
