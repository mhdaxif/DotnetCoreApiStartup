using WebApi.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUserList();
        Task<User> Create(User user);
        Task<User> Delete(int id);
        User GetSingle(int id);
    }
}