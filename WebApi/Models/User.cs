using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get;  set; }
        public string UserName { get;  set; }

        public ICollection<Book> Books { get; set; }
    }
}
