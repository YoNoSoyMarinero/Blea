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

        public async Task<IdentityResult> Add(RegistrationDTO registrationDTO)
        {
            User user = new User {
                FirstName = registrationDTO.FirstName,
                LastName = registrationDTO.LastName,
                Email = registrationDTO.Email,
                UserName = registrationDTO.Username
            };
            var result = await userManager.CreateAsync(user, registrationDTO.Password);
            return result;
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Get(int id)
        {
            return await userManager.FindByIdAsync(id.ToString());
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
