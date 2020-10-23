using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.Utils
{
    public class OperationDetails
    {
        public bool Succedeed = true;
        public string Property = string.Empty;
        public OperationErrorMessages ErrorMessageCode = OperationErrorMessages.NoError;

        public static OperationDetails Successfully()
        {
            return new OperationDetails { Succedeed = true };
        }

        public static OperationDetails WithError(OperationErrorMessages messageCode, string property = "")
        {
            return new OperationDetails
            {
                Succedeed = false,
                ErrorMessageCode = messageCode,
                Property = property
            };
        }
    }
}
