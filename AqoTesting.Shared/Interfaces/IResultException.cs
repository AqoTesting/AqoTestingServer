using AqoTesting.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.Interfaces
{
    public interface IResultException
    {
        OperationErrorMessages ErrorMessageCode { get; set; }
    }
}
