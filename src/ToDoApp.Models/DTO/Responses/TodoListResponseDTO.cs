using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models.DTO.Responses
{
    public class TodoListResponseDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime AddedOn { get; set; }

        public int AddedBy { get; set; }

        public DateTime? EditedOn { get; set; }

        public int? EditedBy { get; set; }
    }
}
