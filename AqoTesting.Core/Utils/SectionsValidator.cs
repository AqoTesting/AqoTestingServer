using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
using AqoTesting.Shared.Enums;
using System.Collections.Generic;

namespace AqoTesting.Core.Utils
{
    public static class SectionsValidator
    {
        public static (bool, OperationErrorMessages, object) Validate(UserAPI_GetSection_DTO[] sections)
        {
            /*HashSet<int> uniqueSectionIds = new HashSet<int>();
            HashSet<int> uniqueQuestionIds;
            foreach (var section in sections)
            {
                    return (false, OperationErrorMessages.NonUniqueSectionId, new CommonAPI_Error_DTO { ErrorSubject = section.Id });

                foreach (var question in section.Questions)
                {

                    int correctsCount;
                    switch (question.Type)
                    {
                        case QuestionTypes.SingleChoice:
                            if (question.Shuffle == null)
                                return (false, OperationErrorMessages.TypeMismatch, new CommonAPI_Error_DTO { ErrorSubject = new int[] { section.Id, question.Id } });

                            correctsCount = 0;
                            foreach (var option in question.Options)
                            {
                                if (option.IsCorrect == null)
                                    return (false, OperationErrorMessages.TypeMismatch, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Id, question.Id, option } });
                                if (option.Text == null && option.ImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Id, question.Id, option } });

                                if (option.IsCorrect.Value) correctsCount++;
                            }
                            if (correctsCount != 1)
                                return (false, OperationErrorMessages.ChoiceWrongCorrectsCount, new CommonAPI_Error_DTO { ErrorSubject = new int[] { section.Id, question.Id } });

                            break;

                        case QuestionTypes.MultipleChoice:
                            if (question.Shuffle == null)
                                return (false, OperationErrorMessages.TypeMismatch, new CommonAPI_Error_DTO { ErrorSubject = new int[] { section.Id, question.Id } });

                            correctsCount = 0;
                            foreach (var option in question.Options)
                            {
                                if (option.IsCorrect == null)
                                    return (false, OperationErrorMessages.TypeMismatch, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Id, question.Id, option } });
                                if (option.Text == null && option.ImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Id, question.Id, option } });

                                if (option.IsCorrect.Value) correctsCount++;
                            }
                            if (correctsCount < 2)
                                return (false, OperationErrorMessages.ChoiceWrongCorrectsCount, new CommonAPI_Error_DTO { ErrorSubject = new int[] { section.Id, question.Id } });

                            break;

                        case QuestionTypes.Matching:
                            foreach (var option in question.Options)
                            {
                                if (option.LeftText == null && option.LeftImageUrl == null || option.RightText == null && option.RightImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Id, question.Id, option } });
                            }

                            break;

                        case QuestionTypes.Sequence:
                            foreach (var option in question.Options)
                            {
                                if (option.Text == null && option.ImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Id, question.Id, option } });
                            }

                            break;
                    }
                }
            }*/

            return (true, OperationErrorMessages.NoError, null);
        }
    }
}
