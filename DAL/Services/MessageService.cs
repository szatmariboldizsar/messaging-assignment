using DAL.DTOs;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Services
{
    public class MessageService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserService _userService;

        public MessageService(AppDbContext dbContext, UserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public async Task<bool> CreateMessage(Message message)
        {
            try
            {
                _dbContext.Messages.Add(message);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateMessage(Message message)
        {
            try
            {
                _dbContext.Messages.Update(message);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Message>> GetMessagesForUsersAsync(long userId1, long userId2)
        {
            return await _dbContext.Messages.Where(m => (m.FromUserId == userId1 && m.ToUserId == userId2) || (m.FromUserId == userId2 && m.ToUserId == userId1)).OrderBy(m => m.DateSent).ToListAsync();
        }

        public async Task<Message?> GetLastMessageForUsers(long userId1, long userId2)
        {
            return await _dbContext.Messages.OrderByDescending(m => m.DateSent).FirstOrDefaultAsync(m => (m.FromUserId == userId1 && m.ToUserId == userId2) || (m.FromUserId == userId2 && m.ToUserId == userId1));
        }

        public async Task<List<UserWithMessage>> GetLastMessagesForUser(long userId)
        {
            List<UserWithMessage> lastMessages = new List<UserWithMessage>();
            List<User> users = await _userService.GetAllUsersForUserAsync(userId);

            foreach (User user in users)
            {
                UserWithMessage userWithMessage = new UserWithMessage(user, await GetLastMessageForUsers(userId, user.Id));
                lastMessages.Add(userWithMessage);
            }

            return lastMessages;
        }
    }
}
