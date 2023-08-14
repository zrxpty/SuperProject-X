using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Data.Models
{
    public class Roles
    {
        public int Id { get; set; }

        public int UserAccountId { get; set; }
        [ForeignKey("UserAccountId")]
        public UserAccount UserAccount { get; set; }

        public string? Role { get; set; }
    }
}