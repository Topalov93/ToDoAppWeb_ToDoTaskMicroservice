using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models.DTO.Requests
{
    public class ToDoListRequestDTO
    {
        [Required]
        [MinLength(1)]
        [MaxLength(150)]
        public string Title { get; set; }
    }
}
