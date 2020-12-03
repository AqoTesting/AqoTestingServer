using AqoTesting.Shared.DTOs.API.CommonAPI;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.Enums;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace AqoTesting.Core.Utils
{
    public static class TestUtils
    {
        public static (bool, OperationErrorMessages, object) ValidateSections(Dictionary<string, UserAPI_PostTestSectionDTO> sections)
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
                        return (false, OperationErrorMessages.EmptyQuestion, new CommonAPI_ErrorDTO { ErrorSubject = new string[] { section.Key, question.Key } });

                    switch (question.Value.Type)
                    {
                        case QuestionTypes.SingleChoice:
                            correctsCount = 0;
                            foreach (var option in question.Value.Options)
                                if (option.Text == null && option.ImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_ErrorDTO { ErrorSubject = new object[] { section.Key, question.Key, option } });

                                else if (option.IsCorrect)
                                    correctsCount++;

                            if (correctsCount != 1)
                                return (false, OperationErrorMessages.SingleChoiceWrongCorrectsCount, new CommonAPI_ErrorDTO { ErrorSubject = new string[] { section.Key, question.Key } });

                            break;

                        case QuestionTypes.MultipleChoice:
                            foreach (var option in question.Value.Options)
                                if (option.Text == null && option.ImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_ErrorDTO { ErrorSubject = new object[] { section.Key, question.Key, option } });

                            break;

                        case QuestionTypes.Matching:
                            foreach (var option in question.Value.Options)
                                if (option.LeftText == null && option.LeftImageUrl == null || option.RightText == null && option.RightImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_ErrorDTO { ErrorSubject = new object[] { section.Key, question.Key, option } });

                            break;

                        case QuestionTypes.Sequence:
                            foreach (var option in question.Value.Options)
                                if (option.Text == null && option.ImageUrl == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_ErrorDTO { ErrorSubject = new object[] { section.Key, question.Key, option } });

                            break;

                        case QuestionTypes.FillIn:
                            foreach(var option in question.Value.Options)
                                if(option.IsBlank && option.CorrectTexts.Length == 0 || !option.IsBlank && option.Text == null)
                                    return (false, OperationErrorMessages.EmptyOption, new CommonAPI_ErrorDTO { ErrorSubject = new object[] { section.Key, question.Key, option } });

                            break;
                    }
                }
            }

            return (true, OperationErrorMessages.NoError, null);
        }

        public static (bool, OperationErrorMessages, object) MergeSections(Dictionary<string, TestsDB_SectionDTO> dbSections, Dictionary<string, UserAPI_PostTestSectionDTO> updateSections)
        {
            foreach (var updateSection in updateSections)
            {
                if (updateSection.Value.Deleted)
                    if (!dbSections.ContainsKey(updateSection.Key))
                        return (false, OperationErrorMessages.SectionNotFound, new CommonAPI_ErrorDTO { ErrorSubject = updateSection.Key });
                    else
                        dbSections.Remove(updateSection.Key);

                else
                {
                    foreach (var updateQuestion in updateSection.Value.Questions)
                        if (updateQuestion.Value.Deleted)
                            if (!dbSections.ContainsKey(updateSection.Key))
                                return (false, OperationErrorMessages.SectionNotFound, new CommonAPI_ErrorDTO { ErrorSubject = updateSection.Key });
                            else if (!dbSections[updateSection.Key].Questions.ContainsKey(updateQuestion.Key))
                                return (false, OperationErrorMessages.QuestionNotFound, new CommonAPI_ErrorDTO { ErrorSubject = new string[] { updateSection.Key, updateQuestion.Key } });

                            else
                            {
                                dbSections[updateSection.Key].Questions.Remove(updateQuestion.Key);
                                updateSection.Value.Questions.Remove(updateQuestion.Key);
                            }

                    if (dbSections.ContainsKey(updateSection.Key))
                    {
                        var oldQuestions = dbSections[updateSection.Key].Questions.ToDictionary(x => x.Key, x => x.Value);

                        dbSections[updateSection.Key] = Mapper.Map<TestsDB_SectionDTO>(updateSection.Value);

                        foreach(var question in dbSections[updateSection.Key].Questions)
                        {
                            if (oldQuestions.ContainsKey(question.Key))
                                oldQuestions[question.Key] = question.Value;
                            else
                                oldQuestions.Add(question.Key, question.Value);
                        }
                        dbSections[updateSection.Key].Questions = oldQuestions;
                    }
                    else
                        dbSections.Add(updateSection.Key, Mapper.Map<TestsDB_SectionDTO>(updateSection.Value));

                    if (dbSections[updateSection.Key].Questions.Count > 0 && dbSections[updateSection.Key].Questions.Count < dbSections[updateSection.Key].AttemptQuestionsNumber)
                        return (false, OperationErrorMessages.NotEnoughQuestions, new CommonAPI_ErrorDTO { ErrorSubject = updateSection.Key });
                }                
            }

            return (true, OperationErrorMessages.NoError, dbSections);
        }
    }
}
