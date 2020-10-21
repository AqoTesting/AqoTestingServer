using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.Enums
{
    public enum OperationErrorMessages
    {
        NoError = 0,
        InvalidModel = 1,

        // Auth
        WrongAuthData = 10,
        LoginAlreadyTaken = 11,
        EmailAlreadyTaken = 12,
    }
}