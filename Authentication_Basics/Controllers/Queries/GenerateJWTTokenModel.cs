namespace Authentication_Basics.Controllers.Queries
{
    public class GenerateJWTTokenModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
