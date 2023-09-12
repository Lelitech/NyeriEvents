using System.ComponentModel.DataAnnotations;

namespace NyeriEvents.Requests
{
    public class AddUser
    {
        [Required]
        public string Name { get; set; }=string.Empty;
        [Required]
        public string Email { get; set; }= string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
