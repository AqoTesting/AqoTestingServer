using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DTOs.BDModels
{
    public class SectionWithQuestions : Section
    {
#pragma warning disable CS8618 // Поле, не допускающее значение NULL, не инициализировано. Рекомендуется объявить его как допускающее значение NULL.
        public Question[] Questions { get; set; }
#pragma warning restore CS8618 // Поле, не допускающее значение NULL, не инициализировано. Рекомендуется объявить его как допускающее значение NULL.
    }
}
