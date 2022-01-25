using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models.DTO.Requests
{
    public class UserRequestDTO
    {
        [Required]
        [MinLength(1)]
        [MaxLength(150)]
        public string Username { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(150)]
        public string Password { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(150)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(150)]
        public string LastName { get; set; }
    }

    public class UserWithRoleRequestDTO : UserRequestDTO
    {
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Role { get; set; }
    }
}
