namespace AqoTesting.Shared.Enums
{
    public enum OperationErrorMessages
    {
        #region Common
        NoError = 0,
        InvalidModel = 1,
        EntityNotFound = 3,
        #endregion

        #region Auth
        WrongAuthData = 100,
        LoginAlreadyTaken = 101,
        EmailAlreadyTaken = 102,
        #endregion

        #region Rooms
        RoomNotFound = 200,
        RoomAccessError = 201,

        DomainAlreadyTaken = 202,
        
        // Юзер не может добавить мембера по полям, потому что включена самостоятельная регистрация
        RoomRegistrationEnabled = 203,
        #endregion

        #region Tests
        TestNotFound = 300,
        TestAccessError = 301,

        SectionNotFound = 302,
        QuestionNotFound = 303,

        NotEnoughSections = 304,
        NotEnoughQuestions = 305,

        // В SingleChoice 0 или больше 1 правильных ответов, в MultipleChoice - меньше 2
        ChoiceWrongCorrectsCount = 306,

        // В Option нет ни картинки, ни текста
        EmptyOption = 307,
        EmptyQuestion = 308,
        #endregion

        #region Users
        UserNotFound = 400,
        #endregion

        #region Members
        MemberNotFound = 500,
        MemberAccessError = 501,

        MemberAlreadyExists = 502,

        FieldNotPassed = 503,
        FieldRegexMismatch = 504,
        FieldOptionNotInList = 505,

        // Мембер с такими полями уже зареган
        FieldsAlreadyExists = 506,

        MemberIsNotApproved = 507,
        MemberIsApproved = 508,

        MemberIsNotRegistered = 509,
        MemberIsRegistered = 510,
        #endregion

        #region Attempts
        AttemptNotFound = 600,
        AttemptAccessError = 601,
        #endregion
    }
}