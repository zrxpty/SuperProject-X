using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BLL.Models.OutputModels
{
    public class AuthenticationOutputModel
    {
        public string Token { get; set; }

        public string? Email { get; set; }

        public string? Login { get; set; }

        public string? Role { get; set; }

    }
}
