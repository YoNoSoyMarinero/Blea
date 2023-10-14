using Microsoft.AspNetCore.Identity;
using server.DTOs;
using server.Models;

namespace server.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetById(string id);
        Task<User> GetByEmail(string email);
        Task<IdentityResult> Add(User user);
        Task<string> GenerateConfirmationToken(User user);
        Task<IdentityResult> ConfirmUser(User user, string token);
        Task<bool> CheckPassword(User user, string password);
        void Update(User user);
        Task<IdentityResult> Delete(User user);
    }
}
