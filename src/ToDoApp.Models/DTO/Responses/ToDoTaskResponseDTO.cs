using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models.DTO.Responses
{
    public class ToDoTaskResponseDTO
    {
        public int Id { get; set; }

        public int ToDoListId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime AddedOn { get; set; }

        public int UserId { get; set; }

        public DateTime? EditedOn { get; set; }

        public int? EditedBy { get; set; }
    }
}
