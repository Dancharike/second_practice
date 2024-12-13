namespace AS_practice.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } // administrator, lecturer or student
        public int RoleSpecificId { get; set; }
    }
}