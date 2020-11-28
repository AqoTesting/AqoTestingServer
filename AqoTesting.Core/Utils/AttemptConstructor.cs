using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Tests.OptionsContainers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AqoTesting.Core.Utils
{
    public class AttemptConstructor
    {
        public static Dictionary<string, TestsDB_Section_DTO> SelectSections(TestsDB_Test_DTO test)
        {
            var random = new Random();

            if (test.AttemptSectionsNumber == 0)
                test.AttemptSectionsNumber = test.Sections.Count();

            var testSections = test.Sections.ToArray()
                .OrderBy(x => random.Next())
                    .Take(test.AttemptSectionsNumber).ToArray();

            for (var i = 0; i < test.AttemptSectionsNumber; i++)
            {
                var testSection = testSections[i];

                if (testSection.Value.AttemptQuestionsNumber == 0)
                    testSection.Value.AttemptQuestionsNumber = testSection.Value.Questions.Count();

                testSections[i].Value.Questions = testSection.Value.Questions.OrderBy(x => random.Next())
                    .Take(testSection.Value.AttemptQuestionsNumber)
                        .ToDictionary(x => x.Key, x => x.Value);

                if (testSections[i].Value.Shuffle)
                    testSections[i].Value.Questions = ShuffleQuestions(testSections[i].Value.Questions);
            }

            test.Sections = testSections.ToDictionary(x => x.Key, x => x.Value);

            if (test.Shuffle)
                test.Sections = ShuffleSections(test.Sections);

            return test.Sections;
        }

        private static Dictionary<string, TestsDB_Section_DTO> ShuffleSections(Dictionary<string, TestsDB_Section_DTO> sections)
        {
            var random = new Random();

            var shuffledWeights = sections.Select(section => section.Value.Weight)
                .OrderBy(x => random.Next())
                    .ToArray();

            var sectionsPairs = sections.ToArray();
            for (var i = 0; i < sectionsPairs.Length; i++)
                sectionsPairs[i].Value.Weight = shuffledWeights[i];

            return sectionsPairs.ToDictionary(x => x.Key, x => x.Value);
        }

        private static Dictionary<string, TestsDB_Question_DTO> ShuffleQuestions(Dictionary<string, TestsDB_Question_DTO> questions)
        {
            var random = new Random();

            var shuffledWeights = questions.Select(question => question.Value.Weight)
                .OrderBy(x => random.Next())
                    .ToArray();

            var questionsPairs = questions.ToArray();
            for (var i = 0; i < questionsPairs.Length; i++)
                questionsPairs[i].Value.Weight = shuffledWeights[i];

            return questionsPairs.ToDictionary(x => x.Key, x => x.Value);
        }

        public static T[] ShuffleArray<T>(T[] array)
        {
            var random = new Random();
            return array.OrderBy(x => random.Next()).ToArray();
        }
    }
}
