using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckListAPI.Dtos
{
    public class LoginDto
    {
        public string? Username { set; get; }
        public string? Password { set; get; }
    }
}