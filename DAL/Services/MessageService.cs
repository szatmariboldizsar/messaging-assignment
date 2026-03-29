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

        public MessageService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Message>> GetMessagesForUsersAsync(long userId1, long userId2)
        {
            //return await _dbContext.Messages.Where(m => m.FromUserId == userId1 ||
            //                                            m.FromUserId == userId2 &&
            //                                            m.ToUserId == userId1 ||
            //                                            m.ToUserId == userId2).OrderBy(m => m.DateSent).ToListAsync();

            return new List<Message> { new Message() { FromUserId = 123, Content = "Test message From" },
                                        new Message() { Content = "Test message To" },
                                        new Message() { FromUserId = 123, Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer pulvinar tellus quis purus volutpat, eu vehicula dui tempor. Nulla bibendum tincidunt molestie. Proin egestas tellus sagittis dolor vestibulum dapibus. Duis risus dui, congue et commodo eget, commodo in lorem. Fusce lacinia tempus dictum. Integer egestas rhoncus faucibus. Duis sit amet vulputate sem. Cras interdum leo vel nulla semper mattis. Pellentesque neque sem, finibus ac imperdiet vel, fermentum non augue. Vestibulum tempor nisl vel velit elementum suscipit. Aenean malesuada eu lectus rutrum dapibus. Nunc vel lacus enim. Aliquam interdum porttitor commodo." },
                                        new Message() { Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer pulvinar tellus quis purus volutpat, eu vehicula dui tempor. Nulla bibendum tincidunt molestie. Proin egestas tellus sagittis dolor vestibulum dapibus. Duis risus dui, congue et commodo eget, commodo in lorem. Fusce lacinia tempus dictum. Integer egestas rhoncus faucibus. Duis sit amet vulputate sem. Cras interdum leo vel nulla semper mattis. Pellentesque neque sem, finibus ac imperdiet vel, fermentum non augue. Vestibulum tempor nisl vel velit elementum suscipit. Aenean malesuada eu lectus rutrum dapibus. Nunc vel lacus enim. Aliquam interdum porttitor commodo." }
                                     };
        }
    }
}
