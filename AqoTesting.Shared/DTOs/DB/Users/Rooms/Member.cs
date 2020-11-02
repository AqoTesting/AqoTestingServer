namespace AqoTesting.Shared.DTOs.DB.Users.Rooms
{
    public class Member
    {
        public string Token { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public Attempt[] Attempts { get; set; }
        public object UserData { get; set; }
    }
}
