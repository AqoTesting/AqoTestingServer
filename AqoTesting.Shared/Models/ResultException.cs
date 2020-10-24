using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.Models
{
    public class ResultException : Exception, IResultException
    {
        public OperationErrorMessages ErrorMessageCode { get; set; }
        public ResultException(OperationErrorMessages OperationErrorMessages)
        {
            ErrorMessageCode = OperationErrorMessages;
        }
    }
}
