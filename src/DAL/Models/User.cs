using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Username { get; set; }

        [MaxLength(150)]
        public string FirstName { get; set; }

        [MaxLength(150)]
        public string LastName { get; set; }
    }
}
