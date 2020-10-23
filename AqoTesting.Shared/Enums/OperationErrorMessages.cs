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
        RoomDoesntExists = 21,
        RoomAccessError = 22
    }
}