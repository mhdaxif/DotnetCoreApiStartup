using MvcStartup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStartup.Services
{
    public class UserService : IUserService
    {
        public bool AuthenticateUser()
        {
            var rand = new Random();
            var number = rand.Next(100);

            return number % 2 == 0;
        }

        public IEnumerable<User> GetUserList()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Ali",
                },   new User
                {
                    Id = 2,
                    Name = "Faisal",
                },   new User
                {
                    Id = 3,
                    Name = "Jawad",
                },
            };
        }
    }
}
