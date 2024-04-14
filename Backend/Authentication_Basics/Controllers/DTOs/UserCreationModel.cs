using API.Authentication;
using System.Collections.Generic;

namespace API.Controllers.DTOs
{
    public class UserCreationModel
    {
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }
}
