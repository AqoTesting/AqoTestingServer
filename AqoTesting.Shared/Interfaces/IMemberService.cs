using AqoTesting.Shared.DTOs.API.Members;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.Enums;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IMemberService
    {
        Task<Member> GetMemberByAuthData(SignInMemberDTO authData);

        Task<(OperationErrorMessages, string)> MemberAuth(SignUpMemberDTO signUpMemberDTO);
    }
}
