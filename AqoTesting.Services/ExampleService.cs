using AqoTesting.Core.DTOs.API;
using AqoTesting.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AqoTesting.Services
{
    public class ExampleService : ServiceBase, IExampleService
    {
        public async Task<MeowDTO> GetMeow()
        {
            return new MeowDTO { Text = "мяу..." };
        }
    }
}
