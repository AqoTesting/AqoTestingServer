namespace AqoTesting.Shared.Enums
{
    public enum OperationErrorMessages
    {
        // 0
        #region Common
        NoError = 0,
        InvalidModel = 1,
        EntityNotFound = 3,
        #endregion

        // 100
        #region Auth
        WrongAuthData = 100,
        LoginAlreadyTaken = 101,
        EmailAlreadyTaken = 102,
        #endregion

        // 200
        #region Rooms
        RoomNotFound = 200,
        RoomAccessError = 201,

        // Нельзя создать комнату, такой домен уже занят
        DomainAlreadyTaken = 202,
        
        // Юзер не может добавить мембера по полям, потому что включена самостоятельная регистрация
        RoomRegistrationEnabled = 203,
        #endregion

        // 300
        #region Tests
        TestNotFound = 300,
        TestAccessError = 301,

        // Попытка удаления несуществующей секции или вопроса из несуществующей секции
        SectionNotFound = 302,
        // Попытка удаления несуществующего вопроса
        QuestionNotFound = 303,

        // При изменении информации теста или изменении секций, если секций больше нуля и (AttemptSectionsNumber, количество выдаваемых секций) больше, чем есть секций
        NotEnoughSections = 304,
        // При изменении секций, если (AttemptQuestionsNumber, количество выдаваемых вопросов) больше, чем есть вопросов
        NotEnoughQuestions = 305,

        // При создании или редактировании вопроса типа SingleChoice, передано 0 или больше 1 правильных ответов
        SingleChoiceWrongCorrectsCount = 306,

        // При создании или редактировании вопроса, в Option нет ни картинки, ни текста
        EmptyOption = 307,
        // При создании или редактировании вопроса, в нём нет ни картинки, ни текста
        EmptyQuestion = 308,

        // Мембер пытается начать тест, но у него кончлись попытки
        NoAttemptsLeft = 309,
        // IsActive == false и ( (дата деактивации не указана или уже настала) или (дата активации указана, но ещё не настала) )
        TestIsNotActive = 310,
        #endregion

        // 400
        #region Users
        UserNotFound = 400,
        #endregion

        // 500
        #region Members
        MemberNotFound = 500,
        MemberAccessError = 501,

        // При самостоятельной регистрации мембера, с такими полями уже зареган
        MemberAlreadyRegistered = 502,

        // При регистрации мембера, он не отправил IsRequired поле
        FieldNotPassed = 503,
        // При регистрации мембера, значение поля не соответствует регулярному выражению поля
        FieldRegexMismatch = 504,
        // При регистрации мембера, в поле типа Select передано значение, которого нет в списке значений поля
        FieldOptionNotInList = 505,

        // При ручном добавлении по полям, с такими полями уже зареган
        FieldsAlreadyExists = 506,

        // Неподтверждённый мембер пытается чёто сделать
        MemberIsNotApproved = 507,
        // Юзер пытается подтвердить уже подтверждённого мембера
        MemberIsApproved = 508,

        // Юзер пытается разрегистрировать незарегистрированного мембера
        MemberIsNotRegistered = 509,
        // Просто для симметрии
        MemberIsRegistered = 510,

        // У мембера нет активной попытки
        HasNoActiveAttempt = 511,
        // У мембера есть активная попытка (нельзя начать ещё одну)
        HasActiveAttempt = 512,
        #endregion

        // 600
        #region Attempts
        AttemptNotFound = 600,
        AttemptAccessError = 601,

        // MemberAPI Answer
        // В MemberAPI_CommonTestAnswer_DTO, поля, нужные для конкретного типа вопроса не переданы (null)
        AnswerNotPassed = 602,
        // SelectedOption больше или равно количетсву опций
        SelectedOptionOutOfRange = 603,
        // Количество переданных id опций не равно количеству опций в вопросе
        WrongOptionsCount = 604,
        // Переданы одинаковые id опций
        NonUniqueOption = 605,
        // Время на прохождение кончилось, а мембер попытался ответить, возвращается один раз, после этого попытка становится неактивной
        TimeIsUp = 606,
        #endregion
    }
}