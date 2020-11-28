using AqoTesting.Shared.DTOs.DB.Attempts.Options;

namespace AqoTesting.Shared.DTOs.DB.Attempts.OptionsData
{
    public class AttemptsDB_MatchingOptions_Container
    {
        public AttemptsDB_PositionalOption[] LeftCorrectOptions { get; set; }
        public AttemptsDB_PositionalOption[] RightCorrectOptions { get; set; }

        public AttemptsDB_PositionalOption[] LeftAnswerOptions { get; set; }
        public AttemptsDB_PositionalOption[] RightAnswerOptions { get; set; }
    }
}
