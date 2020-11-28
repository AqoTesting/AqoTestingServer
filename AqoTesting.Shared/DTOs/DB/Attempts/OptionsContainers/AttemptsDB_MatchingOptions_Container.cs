using AqoTesting.Shared.DTOs.DB.Attempts.Options;

namespace AqoTesting.Shared.DTOs.DB.Attempts.OptionsData
{
    public class AttemptsDB_MatchingOptions_Container
    {
        public AttemptsDB_PositionalOption[] LeftCorrectSequence { get; set; }
        public AttemptsDB_PositionalOption[] RightCorrectSequence { get; set; }

        public AttemptsDB_PositionalOption[] LeftAnswerSequence { get; set; }
        public AttemptsDB_PositionalOption[] RightAnswerSequence { get; set; }
    }
}
