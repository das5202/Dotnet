namespace JwtAuth.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Users(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
