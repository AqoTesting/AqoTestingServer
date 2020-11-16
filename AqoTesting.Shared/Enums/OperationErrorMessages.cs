namespace AqoTesting.Shared.Enums
{
    public enum OperationErrorMessages
    {
        NoError = 0,
        InvalidModel = 1,
        EntityNotFound = 3,

        // Auth
        WrongAuthData = 100,
        LoginAlreadyTaken = 101,
        EmailAlreadyTaken = 102,

        // Rooms
        DomainAlreadyTaken = 200,
        RoomNotFound = 201,
        RoomAccessError = 202,
        RoomRegistrationEnabled = 203,

        // Tests
        TestNotFound = 300,

        // Users
        UserNotFound = 400,

        // Members
        MemberNotFound = 500,
        MemberAlreadyExists = 501,
        MemberAccessError = 502,

        FieldNotPassed = 503,
        FieldRegexMissmatch = 504,
        FieldOptionNotInList = 505,
        FieldsAlreadyExists = 506,

        MemberIsNotApproved = 507,
        MemberIsApproved = 508,

        MemberIsNotRegistered = 509,
        MemberIsRegistered = 510,

        RegistrationDisabled = 511,
        RegistrationEnabled = 512,
    }
}