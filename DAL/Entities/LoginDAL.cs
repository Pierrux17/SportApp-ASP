using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class LoginDAL
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; }
    }
}
