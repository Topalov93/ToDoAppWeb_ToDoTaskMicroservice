using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models.DTO.Requests
{
    public class ToDoTaskCreateRequestDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public string UserUsername { get; set; }

        public string UserFirstname { get; set; }

        public string UserLastname { get; set; }
    }

    public class ToDoTaskEditRequestDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
