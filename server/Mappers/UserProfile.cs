using AutoMapper;
using Microsoft.AspNetCore.Identity;
using server.DTOs;
using server.Models;

namespace server.Mappers
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<RegistrationDTO, User>();
        }
    }
}