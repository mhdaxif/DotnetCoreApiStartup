using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
         public IEnumerable<User> GetList()
        {
            var users = _userService.GetUserList();
            return users; 
        }

        [HttpGet("{id}")]
        public User GetSingle(int id)
        {
            return _userService.GetSingle(id);
        }

        [HttpDelete("{id}")]
        public async Task<User> DeleteAsync (int id)
        {
            return await _userService.Delete(id);
        }        
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] User user)
        {
            var createdUser = await _userService.Create(user);
            return Ok(createdUser);
        }


        [Authorize]
        [HttpGet("list")]
        public IEnumerable<User> GetListSecure()
        {
            var users = _userService.GetUserList();
            return users;
        }
    } 
}