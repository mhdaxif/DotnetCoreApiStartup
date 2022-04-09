using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Db;
using WebApi.Models;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private AppDbContext context;

        public UserService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<User> Create(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Delete(int id)
        {
            var userToDelete = context.Users.FirstOrDefault(x => x.Id == id);
            if (userToDelete == null)
            {
                return null;
            }

            context.Users.Remove(userToDelete);
            await context.SaveChangesAsync();

            return userToDelete;
        }

        public User GetSingle(int id)
        {
            return context.Users.FirstOrDefault(x => x.Id == id);
        }


        public IEnumerable<User> GetUserList()
        {
            return context.Users.Include(x => x.Books);
        }

        public async Task<bool> UpdateAsync(int id, User user)
        {
            //var userToDelete = context.Users.FirstOrDefault(x => x.Id == id);
               
            context.Users.Update(user);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
