using System.ComponentModel.DataAnnotations.Schema;

namespace NyeriEvents.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int PhoneNumber { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
      
        public List<Event> Events { get; set; }= new List<Event>();

    }
}
