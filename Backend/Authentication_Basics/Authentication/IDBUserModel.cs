using System.Collections.Generic;

namespace API.Authentication
{
    public interface IDBUserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LanguageCode { get; set; }
        public string Email { get; set; }
        public List<string> Permissions { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class DBUserModel : IDBUserModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new List<string>();
        public bool IsEnabled { get; set; }
    }
}