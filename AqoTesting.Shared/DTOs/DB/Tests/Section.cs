namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class Section
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Question[] Questions { get; set; }
        public bool Shuffle { get; set; }
    }
}
