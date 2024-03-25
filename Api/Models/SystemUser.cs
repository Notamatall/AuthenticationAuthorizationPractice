namespace Api.Models
{
    public class SystemUser
    {
        public int Id { get; protected set; }
        public virtual string Username { get; protected set; }
        public virtual string Password { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string Email { get; protected set; }

        public static SystemUser Create(string username, string password, string firstName, string email)
        {
            return new SystemUser()
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                Email = email
            };
        }
    }
}
