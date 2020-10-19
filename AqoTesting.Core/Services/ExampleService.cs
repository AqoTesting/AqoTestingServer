using AqoTesting.Shared.DTOs.API;
using AqoTesting.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AqoTesting.Core.Services
{
    public class ExampleService : ServiceBase, IExampleService
    {
        public async Task<MeowDTO> GetMeow()
        {
            return new MeowDTO { Text = "мяу..." };
        }
    }
}
