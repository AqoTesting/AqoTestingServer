using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace AqoTesting.Shared.Models
{
    public class ResultResponse<TData>
    {
        public bool Succeeded { get; set; } = true;
        public OperationErrorMessages ErrorMessageCode { get; set; } = OperationErrorMessages.NoError;
        public TData Data { get; set; } = default;

        public IActionResult GetObjectResult(ControllerBase? controller = null)
        {
            /*switch (ErrorMessageCode)
            {
                case OperationDetailsErrorMessages.NotAuthorized: return new UnauthorizedResult(); Тут обработка ошибки авторизации, например

            }*/
            return new OkObjectResult(this);
        }
    }

    public static class ResultResponceExtension
    {
        public static IActionResult ResultResponse<TData>(this ControllerBase controller, OperationDetails? operationDetails = null, TData data = default)
        {

            var result = new ResultResponse<TData>() { Data = data };

            if (operationDetails != null)
            {
                result.Succeeded = operationDetails.Succedeed;
                result.ErrorMessageCode = operationDetails.ErrorMessageCode;
            }
            else
            {
                result.Succeeded = true;
                result.ErrorMessageCode = OperationErrorMessages.NoError;
            }
            return result.GetObjectResult(controller);
        }

        public static IActionResult ResultResponse<TData>(this ControllerBase controller, OperationErrorMessages messageCode, TData data = default)
        {
            return ObjectResultResponse<TData>(messageCode, data);
        }

        public static IActionResult ObjectResultResponse<TData>(OperationErrorMessages messageCode, TData data = default)
        {
            var result = new ResultResponse<TData>() { Data = data };

            result.ErrorMessageCode = messageCode;
            result.Succeeded = false;

            switch (messageCode)
            {
                case OperationErrorMessages.NoError:
                    result.Succeeded = true;
                    break;
            }

            return result.GetObjectResult();
        }

    }
}
