namespace AqoTesting.Shared.Enums
{
    public enum OperationErrorMessages
    {
        NoError = 0,
        InvalidModel = 1,
        EntityNotFound = 3,

        // Auth
        WrongAuthData = 10,
        LoginAlreadyTaken = 11,
        EmailAlreadyTaken = 12,

        // Rooms
        DomainAlreadyTaken = 20,
        RoomNotFound = 21,
        RoomAccessError = 22,

        // Tests
        TestNotFound = 30,

        // Users
        UserNotFound = 40,

        // Members
        MemberNotFound = 50,
        MemberAlreadyExists = 51,

        FieldNotPassed = 52,
        FieldRegexMissmatch = 53,
        FieldOptionNotInList = 54,
        FieldsAlreadyExists = 55,

        MemberIsNotApproved = 56,
        MemberIsApproved = 57,

        RegistrationDisabled = 58,
        RegistrationEnabled = 59,
    }
}