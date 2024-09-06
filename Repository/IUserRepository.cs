using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckListAPI.Models;

namespace CheckListAPI.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(User user);
    }
}