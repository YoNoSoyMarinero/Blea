using Microsoft.AspNetCore.Identity;
using server.Interfaces;
using server.Models;

namespace server.Repository
{
    /**
     * This is a class that wraps the Indentities userManager and Encapsulates all of its functionalities
     */
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> userManager;

        public UserRepository(UserManager<User> userManager) 
        { 
            this.userManager = userManager; 
        }

        public async Task<IdentityResult> Add(User user)
        {
            return await userManager.CreateAsync(user, user.Password);
        }

        public async Task<IdentityResult> Delete(User user)
        {
            return await userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> ResetUserPassword(User user, string token, string newPassword)
        {
            return await userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<IdentityResult> ConfirmUser(User user, string token)
        {
            return await userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<User> GetById(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await userManager.FindByNameAsync(username);
        }

        public async Task<string> GenerateConfirmationToken(User user)
        {
            return await userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetToken(User user)
        {
            return await userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
