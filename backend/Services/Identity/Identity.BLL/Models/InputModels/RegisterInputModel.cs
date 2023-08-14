using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BLL.Models.InputModels
{
    public class RegisterInputModel
    {
        public string Email { get; set; }

        public string? Login { get; set; }

        public string Password { get; set; }
    }
}
