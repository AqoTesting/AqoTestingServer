using AqoTesting.Core.Enums;
using AqoTesting.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace AqoTesting.Core.Models
{
    public class ResultResponse
    {
        public bool Succeeded { get; set; } = true;
        public OperationErrorMessages ErrorMessageCode { get; set; }  = OperationErrorMessages.NoError;
        public object? Data { get; set; }
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
        public static IActionResult ResultResponse(this ControllerBase controller, OperationDetails? operationDetails = null, dynamic? data = null)
        {

            var result = new ResultResponse() { Data = data };

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

        public static IActionResult ResultResponse(this ControllerBase controller, OperationErrorMessages messageCode, dynamic? data = null)
        {
            return ObjectResultResponse(messageCode, data);
        }

        public static IActionResult ObjectResultResponse(OperationErrorMessages messageCode, dynamic? data = null)
        {
            var result = new ResultResponse() { Data = data };

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
