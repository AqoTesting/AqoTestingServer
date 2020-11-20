using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Questions;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Core.Utils
{
    public static class SectionsValidator
    {
        public static (bool, OperationErrorMessages, object) Validate(UserAPI_Section_DTO[] sections)
        {
            HashSet<int> uniqueSectionIds = new HashSet<int>();
            HashSet<int> uniqueQuestionIds;
            foreach (var section in sections)
            {
                if (!uniqueSectionIds.Add(section.Id))
                    return (false, OperationErrorMessages.NonUniqueSectionId, new CommonAPI_Error_DTO { ErrorSubject = section.Id });

                uniqueQuestionIds = new HashSet<int>();
                foreach (var question in section.Questions)
                {
                    if (!uniqueQuestionIds.Add(question.Id))
                        return (false, OperationErrorMessages.NonUniqueQuestionId, new CommonAPI_Error_DTO { ErrorSubject = question.Id });

                    switch (question.Type)
                    {
                        case QuestionTypes.SingleChoice:
                            var correctsCount = 0;
                            foreach (var option in question.Options)
                            {
                                if (option.IsCorrect == null)
                                    return (false, OperationErrorMessages.OptionTypeMismatch, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Id, question.Id, option } });
                                if (option.Text == null && option.ImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Id, question.Id, option } });

                                if (option.IsCorrect.Value) correctsCount++;
                            }
                            if (correctsCount != 1)
                                return (false, OperationErrorMessages.SingleChoiceWrongCorrectsCount, new CommonAPI_Error_DTO { ErrorSubject = new int[] { section.Id, question.Id } });

                            break;

                        case QuestionTypes.MultipleChoice:
                            foreach (var option in question.Options)
                            {
                                if (option.IsCorrect == null)
                                    return (false, OperationErrorMessages.OptionTypeMismatch, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Id, question.Id, option } });
                                if (option.Text == null && option.ImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_Error_DTO { ErrorSubject = new object[] { section.Id, question.Id, option } });
                            }

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
            }

            return (true, OperationErrorMessages.NoError, null);
        }
    }
}
