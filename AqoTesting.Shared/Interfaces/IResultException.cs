using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.Interfaces
{
    public interface IResultException
    {
        OperationErrorMessages ErrorMessageCode { get; set; }
    }
}
