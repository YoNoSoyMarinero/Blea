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
            var result = await userManager.CreateAsync(user, user.Password);
            return result;
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetById(int id)
        {
            return await userManager.FindByIdAsync(id.ToString());
        }

        public async Task<bool> EmailExists(string email)
        {
            return await userManager.FindByEmailAsync(email) != null;
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
