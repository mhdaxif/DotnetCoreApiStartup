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
        private AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Delete(int id)
        {
            var userToDelete = _context.Users.FirstOrDefault(x => x.Id == id);
            if (userToDelete == null)
            {
                return null;
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return userToDelete;
        }

        public User GetSingle(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }


        public IEnumerable<User> GetUserList()
        {
            return _context.Users.Include(x => x.Books);
        }
    }
}
