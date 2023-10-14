using Microsoft.AspNetCore.Identity;
using server.Models;

namespace server.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetById(string id);
        Task<User> GetByEmail(string email);
        Task<IdentityResult> Add(User user);
        Task<IdentityResult> ResetUserPassword(User user, string token, string newPassword);
        Task<IdentityResult> ConfirmUser(User user, string token);
        Task<IdentityResult> Delete(User user);
        Task<string> GenerateConfirmationToken(User user);
        Task<string> GeneratePasswordResetToken(User user);
        Task<bool> CheckPassword(User user, string password);
        void Update(User user);
    }
}
