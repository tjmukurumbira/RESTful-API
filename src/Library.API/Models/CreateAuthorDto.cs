using System;

namespace Library.API.Models
{
    public class CreateAuthorDto
    { 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Genre { get; set; }
    }
}