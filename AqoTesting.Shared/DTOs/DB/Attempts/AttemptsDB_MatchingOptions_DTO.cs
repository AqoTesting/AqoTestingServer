using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.DB.Attempts
{
    public class AttemptsDB_MatchingOptions_DTO
    {
        public Dictionary<AttemptsDB_PositionalOption_DTO, AttemptsDB_PositionalOption_DTO> Correct { get; set; }
        public Dictionary<AttemptsDB_PositionalOption_DTO, AttemptsDB_PositionalOption_DTO> Answer { get; set; }
    }
}
