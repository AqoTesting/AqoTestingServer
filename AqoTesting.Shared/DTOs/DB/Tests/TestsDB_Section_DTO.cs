namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class TestsDB_Section_DTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TestsDB_Question_DTO[] Questions { get; set; }
        public bool Shuffle { get; set; }
    }
}
