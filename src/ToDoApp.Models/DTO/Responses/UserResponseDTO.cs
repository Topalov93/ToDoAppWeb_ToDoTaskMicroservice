using System;

namespace ToDoApp.Models.DTO.Responses
{
    public class UserResponseDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public DateTime AddedOn { get; set; }

        public int AddedBy { get; set; }

        public DateTime? EditedOn { get; set; }

        public int? EditedBy { get; set; }
    }
}
