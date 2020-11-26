﻿using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.Enums;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace AqoTesting.Core.Utils
{
    public static class TestsUtils
    {
        public static (bool, OperationErrorMessages, object) ValidateSections(Dictionary<string, UserAPI_PostSection_DTO> sections)
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

        public static (bool, OperationErrorMessages, object) MergeSections(Dictionary<string, TestsDB_Section_DTO> dbSections, Dictionary<string, UserAPI_PostSection_DTO> updateSections)
        {
            foreach (var updateSection in updateSections)
            {
                if (updateSection.Value.Deleted)
                    if (!dbSections.ContainsKey(updateSection.Key))
                        return (false, OperationErrorMessages.SectionNotFound, new CommonAPI_Error_DTO { ErrorSubject = updateSection.Key });
                    else
                        dbSections.Remove(updateSection.Key);

                else
                {
                    foreach (var updateQuestion in updateSection.Value.Questions)
                        if (updateQuestion.Value.Deleted)
                            if (!dbSections.ContainsKey(updateSection.Key))
                                return (false, OperationErrorMessages.SectionNotFound, new CommonAPI_Error_DTO { ErrorSubject = updateSection.Key });
                            else if (!dbSections[updateSection.Key].Questions.ContainsKey(updateQuestion.Key))
                                return (false, OperationErrorMessages.QuestionNotFound, new CommonAPI_Error_DTO { ErrorSubject = new string[] { updateSection.Key, updateQuestion.Key } });

                            else
                            {
                                dbSections[updateSection.Key].Questions.Remove(updateQuestion.Key);
                                updateSection.Value.Questions.Remove(updateQuestion.Key);
                            }

                    if (dbSections.ContainsKey(updateSection.Key))
                    {
                        var oldQuestions = dbSections[updateSection.Key].Questions.ToDictionary(x => x.Key, x => x.Value);
                        dbSections[updateSection.Key] = Mapper.Map<TestsDB_Section_DTO>(updateSection.Value);
                        dbSections[updateSection.Key].Questions = dbSections[updateSection.Key].Questions.Concat(oldQuestions).ToDictionary(x => x.Key, x => x.Value);
                    }
                    else
                        dbSections.Add(updateSection.Key, Mapper.Map<TestsDB_Section_DTO>(updateSection.Value));

                    if (dbSections[updateSection.Key].Questions.Count < dbSections[updateSection.Key].AttemptQuestionsNumber)
                        return (false, OperationErrorMessages.NotEnoughQuestions, new CommonAPI_Error_DTO { ErrorSubject = updateSection.Key });
                }                
            }

            return (true, OperationErrorMessages.NoError, dbSections);
        }
    }
}