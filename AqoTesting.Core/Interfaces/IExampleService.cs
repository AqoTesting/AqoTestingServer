using AqoTesting.Core.DTOs.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AqoTesting.Core.Interfaces
{
    public interface IExampleService
    {
        Task<MeowDTO> GetMeow();
    }
}
