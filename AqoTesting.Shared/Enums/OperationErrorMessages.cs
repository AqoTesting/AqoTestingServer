namespace AqoTesting.Shared.Enums
{
    public enum OperationErrorMessages
    {
        NoError = 0,
        InvalidModel = 1,
        EntityNotFound = 3,
        NothingChanged = 4,

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

        //Users
        UserNotFound = 40,

        //Members
        MemberNotFound = 50,
        FieldNotPassed = 51,
        FieldRegexMissmatch = 52,
        FieldOptionNotInList = 53,
        MemberNotIsChecked = 54,
        MemberIsChecked = 55,
    }
}