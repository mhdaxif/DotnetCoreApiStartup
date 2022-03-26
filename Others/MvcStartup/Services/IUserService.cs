using MvcStartup.Models;
using System.Collections;
using System.Collections.Generic;

namespace MvcStartup.Services
{
    public interface IUserService
    {
        bool AuthenticateUser();
        IEnumerable<User> GetUserList();
    }
}