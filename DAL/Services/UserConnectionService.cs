using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Services
{
    public class UserConnectionService
    {
        private readonly AppDbContext _dbContext;

        public UserConnectionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserConnection>> GetUserConnectionsForUserAsync(long userId)
        {
            return await _dbContext.UserConnections.Where(uc => uc.UserId == userId || uc.ConnectedUserId == userId).ToListAsync();
        }

        public async Task<bool> FavoriteUserForUser(long userId, long favoriteUserId)
        {
            try
            {
                UserConnection? userConnection = await _dbContext.UserConnections.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ConnectedUserId == favoriteUserId);
                if (userConnection == null)
                {
                    userConnection = new UserConnection
                    {
                        UserId = userId,
                        ConnectedUserId = favoriteUserId
                    };
                    _dbContext.UserConnections.Add(userConnection);
                }
                userConnection.IsFavorited = true;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UnfavoriteUserForUser(long userId, long favoriteUserId)
        {
            try
            {
                UserConnection? userConnection = await _dbContext.UserConnections.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ConnectedUserId == favoriteUserId);
                if (userConnection == null)
                {
                    userConnection = new UserConnection
                    {
                        UserId = userId,
                        ConnectedUserId = favoriteUserId
                    };
                    _dbContext.UserConnections.Add(userConnection);
                }
                userConnection.IsFavorited = false;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> BlockUserForUser(long userId, long blockedUserId)
        {
            try
            {
                UserConnection? userConnection = await _dbContext.UserConnections.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ConnectedUserId == blockedUserId);
                if (userConnection == null)
                {
                    userConnection = new UserConnection
                    {
                        UserId = userId,
                        ConnectedUserId = blockedUserId
                    };
                    _dbContext.UserConnections.Add(userConnection);
                }
                userConnection.IsBlocked = true;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UnblockUserForUser(long userId, long blockedUserId)
        {
            try
            {
                UserConnection? userConnection = await _dbContext.UserConnections.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ConnectedUserId == blockedUserId);
                if (userConnection == null)
                {
                    userConnection = new UserConnection
                    {
                        UserId = userId,
                        ConnectedUserId = blockedUserId
                    };
                    _dbContext.UserConnections.Add(userConnection);
                }
                userConnection.IsBlocked = false;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
