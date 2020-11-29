using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.MemberAPI.Attempts;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Attempts.Options;
using AqoTesting.Shared.DTOs.DB.Attempts.OptionsData;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace AqoTesting.Core.Utils
{
    public class AttemptUtils
    {
        public static (bool, OperationErrorMessages, object) ApplyAnswer(Dictionary<string, AttemptsDB_Section_DTO> sections, string sectionId, string questionId, MemberAPI_CommonTestAnswer_DTO testAnswerDTO)
        {
            if (!sections.ContainsKey(sectionId))
                return (false, OperationErrorMessages.SectionNotFound, null);

            var questions = sections[sectionId].Questions;

            if (!questions.ContainsKey(questionId))
                return (false, OperationErrorMessages.QuestionNotFound, null);

            var question = questions[questionId];

            if (question.Type == QuestionTypes.SingleChoice)
            {
                var selectedOption = testAnswerDTO.SelectedOption;
                if (selectedOption != null)
                {
                    var optionsContainer = BsonSerializer.Deserialize<AttemptsDB_ChoiceOptions_Container>(question.Options);

                    if (selectedOption >= optionsContainer.Options.Length)
                        return (false, OperationErrorMessages.SelectedOptionOutOfRange, null);

                    for (var i = 0; i < optionsContainer.Options.Length; i++)
                        optionsContainer.Options[i].Chosen = i == selectedOption;

                    question.Options = optionsContainer.ToBsonDocument();
                }
            }
            else if (question.Type == QuestionTypes.MultipleChoice)
            {
                var selectedOptions = testAnswerDTO.SelectedOptions;
                if (selectedOptions != null)
                {
                    var optionsContainer = BsonSerializer.Deserialize<AttemptsDB_ChoiceOptions_Container>(question.Options);
                    var optionsCount = optionsContainer.Options.Length;

                    var uniques = new HashSet<int>();
                    for (var i = 0; i < selectedOptions.Length; i++)
                    {
                        if (selectedOptions[i] >= optionsCount)
                            return (false, OperationErrorMessages.SelectedOptionOutOfRange, null);

                        if (!uniques.Add(selectedOptions[i]))
                            return (false, OperationErrorMessages.NonUniqueOption, new CommonAPI_Error_DTO { ErrorSubject = selectedOptions[i] });
                    }

                    for (var i = 0; i < optionsCount; i++)
                        optionsContainer.Options[i].Chosen = selectedOptions.Contains(i);

                    question.Options = optionsContainer.ToBsonDocument();
                }
            }
            else if(question.Type == QuestionTypes.Matching)
            {
                var leftSequence = testAnswerDTO.LeftSequence;
                var rightSequence = testAnswerDTO.RightSequence;
                if (leftSequence != null && rightSequence != null)
                {
                    var optionsContainer = BsonSerializer.Deserialize<AttemptsDB_MatchingOptions_Container>(question.Options);
                    var optionsCount = optionsContainer.LeftAnswerSequence.Length;

                    if (leftSequence.Length != optionsCount)
                        return (false, OperationErrorMessages.WrongOptionsCount, new CommonAPI_Error_DTO { ErrorSubject = "left" });

                    if (rightSequence.Length != optionsCount)
                        return (false, OperationErrorMessages.WrongOptionsCount, new CommonAPI_Error_DTO { ErrorSubject = "right" });

                    var leftUniques = new HashSet<int>();
                    var rightUniques = new HashSet<int>();
                    for (var i = 0; i < leftSequence.Length; i++)
                    {
                        if (leftSequence[i] >= optionsCount)
                            return (false, OperationErrorMessages.SelectedOptionOutOfRange, new CommonAPI_Error_DTO { ErrorSubject = new object[] { "left", leftSequence[i] } });
                        if (rightSequence[i] >= optionsCount)
                            return (false, OperationErrorMessages.SelectedOptionOutOfRange, new CommonAPI_Error_DTO { ErrorSubject = new object[] { "right", rightSequence[i] } });

                        if (!leftUniques.Add(leftSequence[i]))
                            return (false, OperationErrorMessages.NonUniqueOption, new CommonAPI_Error_DTO { ErrorSubject = new object[] { "left", leftSequence[i] } });
                        if (!rightUniques.Add(rightSequence[i]))
                            return (false, OperationErrorMessages.NonUniqueOption, new CommonAPI_Error_DTO { ErrorSubject = new object[] { "right", rightSequence[i] } });
                    }

                    var tempLeftSequence = new AttemptsDB_PositionalOption[optionsCount];
                    var tempRightSequence = new AttemptsDB_PositionalOption[optionsCount];
                    for (var i = 0; i < optionsCount; i++)
                    {
                        tempLeftSequence[i] = optionsContainer.LeftAnswerSequence[leftSequence[i]];
                        tempRightSequence[i] = optionsContainer.RightAnswerSequence[rightSequence[i]];
                    }
                    optionsContainer.LeftAnswerSequence = tempLeftSequence;
                    optionsContainer.RightAnswerSequence = tempRightSequence;

                    question.Options = optionsContainer.ToBsonDocument();
                }
            }
            else if(question.Type == QuestionTypes.Sequence)
            {
                var sequence = testAnswerDTO.Sequence;
                if (sequence != null)
                {
                    var optionsContainer = BsonSerializer.Deserialize<AttemptsDB_SequenceOptions_Container>(question.Options);
                    var optionsCount = optionsContainer.AnswerSequence.Length;

                    if (sequence.Length != optionsCount)
                        return (false, OperationErrorMessages.WrongOptionsCount, null);

                    var uniques = new HashSet<int>();
                    for (var i = 0; i < sequence.Length; i++)
                    {
                        if (sequence[i] >= optionsCount)
                            return (false, OperationErrorMessages.SelectedOptionOutOfRange, new CommonAPI_Error_DTO { ErrorSubject = sequence[i] });

                        if (!uniques.Add(sequence[i]))
                            return (false, OperationErrorMessages.NonUniqueOption, new CommonAPI_Error_DTO { ErrorSubject = sequence[i] });
                    }

                    var temptOptions = new AttemptsDB_PositionalOption[optionsCount];
                    for (var i = 0; i < optionsCount; i++)
                        temptOptions[i] = optionsContainer.AnswerSequence[sequence[i]];
                    optionsContainer.AnswerSequence = temptOptions;

                    question.Options = optionsContainer.ToBsonDocument();
                }
            }

            question.Touched = true;
            question.BlurTime += testAnswerDTO.BlurTimeAddition;
            question.TotalTime += testAnswerDTO.TotalTimeAddition;

            sections[sectionId].Questions[questionId] = question;

            return (true, OperationErrorMessages.NoError, sections);
        }
    }
}
