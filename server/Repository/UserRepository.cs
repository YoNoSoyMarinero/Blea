using Microsoft.AspNetCore.Identity;
using server.DTOs;
using server.Interfaces;
using server.Models;

namespace server.Repository
{
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

        public async Task<User> GetById(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateConfirmationToken(User user)
        {
            return await userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        
        public async Task<IdentityResult> ConfirmUser(User user, string token)
        {
            return await userManager.ConfirmEmailAsync(user, token);
        }
        
        public async Task<bool> CheckPassword(User user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }
    }
}
