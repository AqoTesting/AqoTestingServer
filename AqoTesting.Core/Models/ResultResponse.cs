using AqoTesting.Core.Enums;
using AqoTesting.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace AqoTesting.Core.Models
{
    public class ResultResponse<DataType>
    {
        public bool Succeeded { get; set; } = true;
        public OperationErrorMessages ErrorMessageCode { get; set; }  = OperationErrorMessages.NoError;
        public DataType Data { get; set; } = default;
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
        public static IActionResult ResultResponse<DatType>(this ControllerBase controller, OperationDetails? operationDetails = null, DatType data = default)
        {

            var result = new ResultResponse<DatType>() { Data = data };

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

        public static IActionResult ResultResponse<DatType>(this ControllerBase controller, OperationErrorMessages messageCode, DatType data = default)
        {
            return ObjectResultResponse<DatType>(messageCode, data);
        }

        public static IActionResult ObjectResultResponse<DatType>(OperationErrorMessages messageCode, DatType data = default)
        {
            var result = new ResultResponse<DatType>() { Data = data };

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
