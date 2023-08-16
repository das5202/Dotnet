namespace JwtAuth.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public Users(int id, string password)
        {
            Id = id;
            Password = password;
        }
    }
}
