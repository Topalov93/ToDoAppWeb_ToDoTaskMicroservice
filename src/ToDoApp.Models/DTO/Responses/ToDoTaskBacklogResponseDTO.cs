using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models.DTO.Responses
{
    public class ToDoTaskBacklogResponseDTO
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool IsCompleted { get; set; }
    }
}
