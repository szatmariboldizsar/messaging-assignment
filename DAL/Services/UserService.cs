using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Services
{
    public class UserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<User>> GetAllUsersForUserAsync(long userId)
        {
            return await _dbContext.Users.Where(u => u.Id != userId).ToListAsync();
        }

        public bool IsUsernameUnique(string username)
        {
            return !_dbContext.Users.Where(u => u.Username == username).Any();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
