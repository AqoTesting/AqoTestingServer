using AqoTesting.Shared.DTOs.API.CommonAPI;
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
        public static (bool, OperationErrorMessages, object) ApplyAnswer(Dictionary<string, AttemptsDB_SectionDTO> sections, string sectionId, string questionId, MemberAPI_CommonTestAnswerDTO testAnswerDTO)
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
                    var optionsContainer = BsonSerializer.Deserialize<AttemptsDB_ChoiceOptionsContainer>(question.Options);

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
                    var optionsContainer = BsonSerializer.Deserialize<AttemptsDB_ChoiceOptionsContainer>(question.Options);
                    var optionsCount = optionsContainer.Options.Length;

                    var uniques = new HashSet<int>();
                    for (var i = 0; i < selectedOptions.Length; i++)
                    {
                        if (selectedOptions[i] >= optionsCount)
                            return (false, OperationErrorMessages.SelectedOptionOutOfRange, null);

                        if (!uniques.Add(selectedOptions[i]))
                            return (false, OperationErrorMessages.NonUniqueOption, new CommonAPI_ErrorDTO { ErrorSubject = selectedOptions[i] });
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
                    var optionsContainer = BsonSerializer.Deserialize<AttemptsDB_MatchingOptionsContainer>(question.Options);
                    var optionsCount = optionsContainer.LeftSequence.Length;

                    if (leftSequence.Length != optionsCount)
                        return (false, OperationErrorMessages.WrongOptionsCount, new CommonAPI_ErrorDTO { ErrorSubject = "left" });

                    if (rightSequence.Length != optionsCount)
                        return (false, OperationErrorMessages.WrongOptionsCount, new CommonAPI_ErrorDTO { ErrorSubject = "right" });

                    var leftUniques = new HashSet<int>();
                    var rightUniques = new HashSet<int>();
                    for (var i = 0; i < leftSequence.Length; i++)
                    {
                        if (leftSequence[i] >= optionsCount)
                            return (false, OperationErrorMessages.SelectedOptionOutOfRange, new CommonAPI_ErrorDTO { ErrorSubject = new object[] { "left", leftSequence[i] } });
                        if (rightSequence[i] >= optionsCount)
                            return (false, OperationErrorMessages.SelectedOptionOutOfRange, new CommonAPI_ErrorDTO { ErrorSubject = new object[] { "right", rightSequence[i] } });

                        if (!leftUniques.Add(leftSequence[i]))
                            return (false, OperationErrorMessages.NonUniqueOption, new CommonAPI_ErrorDTO { ErrorSubject = new object[] { "left", leftSequence[i] } });
                        if (!rightUniques.Add(rightSequence[i]))
                            return (false, OperationErrorMessages.NonUniqueOption, new CommonAPI_ErrorDTO { ErrorSubject = new object[] { "right", rightSequence[i] } });
                    }

                    var tempLeftSequence = new AttemptsDB_PositionalOption[optionsCount];
                    var tempRightSequence = new AttemptsDB_PositionalOption[optionsCount];
                    for (var i = 0; i < optionsCount; i++)
                    {
                        tempLeftSequence[i] = optionsContainer.LeftSequence[leftSequence[i]];
                        tempRightSequence[i] = optionsContainer.RightSequence[rightSequence[i]];
                    }
                    optionsContainer.LeftSequence = tempLeftSequence;
                    optionsContainer.RightSequence = tempRightSequence;

                    question.Options = optionsContainer.ToBsonDocument();
                }
            }
            else if(question.Type == QuestionTypes.Sequence)
            {
                var sequence = testAnswerDTO.Sequence;
                if (sequence != null)
                {
                    var optionsContainer = BsonSerializer.Deserialize<AttemptsDB_SequenceOptionsContainer>(question.Options);
                    var optionsCount = optionsContainer.Sequence.Length;

                    if (sequence.Length != optionsCount)
                        return (false, OperationErrorMessages.WrongOptionsCount, null);

                    var uniques = new HashSet<int>();
                    for (var i = 0; i < sequence.Length; i++)
                    {
                        if (sequence[i] >= optionsCount)
                            return (false, OperationErrorMessages.SelectedOptionOutOfRange, new CommonAPI_ErrorDTO { ErrorSubject = sequence[i] });

                        if (!uniques.Add(sequence[i]))
                            return (false, OperationErrorMessages.NonUniqueOption, new CommonAPI_ErrorDTO { ErrorSubject = sequence[i] });
                    }

                    var temptOptions = new AttemptsDB_PositionalOption[optionsCount];
                    for (var i = 0; i < optionsCount; i++)
                        temptOptions[i] = optionsContainer.Sequence[sequence[i]];
                    optionsContainer.Sequence = temptOptions;

                    question.Options = optionsContainer.ToBsonDocument();
                }
            }

            question.Touched = true;
            question.BlurTime += testAnswerDTO.BlurTimeAddition;
            question.TotalTime += testAnswerDTO.TotalTimeAddition;

            sections[sectionId].Questions[questionId] = question;

            return (true, OperationErrorMessages.NoError, sections);
        }

        public static (int, int, int, float, float) CalculateScore(Dictionary<string, AttemptsDB_SectionDTO> sections)
        {
            var maxPoints = 0;
            var correctPoints = 0;
            var penalPoints = 0;

            foreach(var section in sections)
                foreach(var question in section.Value.Questions)
                {
                    maxPoints += question.Value.Cost;

                    if((float)question.Value.BlurTime / question.Value.TotalTime >= 0.5)
                        penalPoints += question.Value.Cost;

                    if(question.Value.Type == QuestionTypes.SingleChoice || question.Value.Type == QuestionTypes.MultipleChoice)
                    {
                        var options = BsonSerializer.Deserialize<AttemptsDB_ChoiceOptionsContainer>(question.Value.Options).Options;

                        var correct = true;
                        foreach(var option in options)
                            if(option.Chosen != option.IsCorrect)
                            {
                                correct = false;
                                break;
                            }

                        if(correct)
                            correctPoints += question.Value.Cost;
                    }
                    else if(question.Value.Type == QuestionTypes.Matching)
                    {
                        var sequences = BsonSerializer.Deserialize<AttemptsDB_MatchingOptionsContainer>(question.Value.Options);
                        var (leftSequence, rightSequence) = (sequences.LeftSequence, sequences.RightSequence);

                        var correct = true;
                        for(var i = 0; i < sequences.LeftSequence.Length; i++)
                            if(leftSequence[i].CorrectIndex != rightSequence[i].CorrectIndex)
                            {
                                correct = false;
                                break;
                            }

                        if(correct)
                            correctPoints += question.Value.Cost;
                    }
                    else if(question.Value.Type == QuestionTypes.Sequence)
                    {
                        var sequence = BsonSerializer.Deserialize<AttemptsDB_SequenceOptionsContainer>(question.Value.Options).Sequence;

                        var correct = true;
                        for(var i = 0; i < sequence.Length; i++)
                            if(sequence[i].CorrectIndex != i)
                            {
                                correct = false;
                                break;
                            }

                        if(correct)
                            correctPoints += question.Value.Cost;
                    }
                }

            var correctRatio = (float)correctPoints / maxPoints;
            var penalRatio = (float)penalPoints / maxPoints;

            return (maxPoints, correctPoints, penalPoints, correctRatio, penalRatio);
        }
    }
}
