using AqoTesting.Shared.DTOs.DB.Attempts.Options;

namespace AqoTesting.Shared.DTOs.DB.Attempts.OptionsData
{
    public class AttemptsDB_SequenceOptions_Container
    {
        public AttemptsDB_PositionalOption[]? CorrectOptions { get; set; }
        public AttemptsDB_PositionalOption[] AnswerOptions { get; set; }
    }
}
