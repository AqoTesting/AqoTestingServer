using AqoTesting.Shared.DTOs.API;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IExampleService
    {
        Task<MeowDTO> GetMeow();
    }
}
