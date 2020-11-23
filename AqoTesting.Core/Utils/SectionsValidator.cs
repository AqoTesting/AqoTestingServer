using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
using AqoTesting.Shared.Enums;
using System.Collections.Generic;

namespace AqoTesting.Core.Utils
{
    public static class SectionsValidator
    {
        public static (bool, OperationErrorMessages, object) Validate(Dictionary<string, UserAPI_PostSection_DTO> sections)
        {
            int correctsCount;

            foreach (var section in sections)
            {
                if (section.Value.Deleted)
                    continue;

                foreach (var question in section.Value.Questions)
                {
                    if (question.Value.Deleted)
                        continue;

                    if (question.Value.Text == null && question.Value.ImageUrl == null)
                        return (false, OperationErrorMessages.EmptyQuestion, new CommonAPI_Error_DTO { ErrorSubject = new string[] { section.Key, question.Key } });

                    switch (question.Value.Type)
                    {
                        case QuestionTypes.SingleChoice:
                            correctsCount = 0;
                            foreach (var option in question.Value.Options)
                                if (option.Text == null && option.ImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Key, question.Key, option } });

                                else if (option.IsCorrect)
                                    correctsCount++;

                            if (correctsCount != 1)
                                return (false, OperationErrorMessages.ChoiceWrongCorrectsCount, new CommonAPI_Error_DTO { ErrorSubject = new string[] { section.Key, question.Key } });

                            break;

                        case QuestionTypes.MultipleChoice:
                            correctsCount = 0;
                            foreach (var option in question.Value.Options)
                                if (option.Text == null && option.ImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Key, question.Key, option } });

                                else if (option.IsCorrect) correctsCount++;

                            if (correctsCount < 2)
                                return (false, OperationErrorMessages.ChoiceWrongCorrectsCount, new CommonAPI_Error_DTO { ErrorSubject = new string[] { section.Key, question.Key } });

                            break;

                        case QuestionTypes.Matching:
                            foreach (var option in question.Value.Options)
                                if (option.LeftText == null && option.LeftImageUrl == null || option.RightText == null && option.RightImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Key, question.Key, option } });

                            break;

                        case QuestionTypes.Sequence:
                            foreach (var option in question.Value.Options)
                                if (option.Text == null && option.ImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Key, question.Key, option } });

                            break;
                    }
                }
            }

            return (true, OperationErrorMessages.NoError, null);
        }
    }
}
