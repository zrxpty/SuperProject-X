using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Data.Models
{
    public class Profile
    {
        public int Id { get; set; }

        public int UserAccountId { get; set; }
        [ForeignKey("UserAccountId")]
        public UserAccount UserAccount { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? BirthDay { get; set; }

        public string? Sex { get; set; }
        public string? City { get; set; }
    }
}