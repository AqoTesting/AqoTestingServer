using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.Members;
using AqoTesting.Shared.DTOs.API.Members.Rooms;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AutoMapper;
using MongoDB.Bson;

namespace AqoTesting.Core.Services
{
    public class MemberService : ServiceBase, IMemberService
    {
        IMemberRepository _memberRepository;
        IRoomRepository _roomRepository;

        public MemberService(IRoomRepository roomRespository, IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
            _roomRepository = roomRespository;
        }

        public async Task<Member> GetMemberByAuthData(SignInMemberDTO authData)
        {
            var member = await _memberRepository.GetMemberByAuthData(authData.Login, Sha256.Compute(authData.Password));

            return member;
        }

        public async Task<(OperationErrorMessages, string)> MemberAuth(SignUpMemberDTO signUpMemberDTO)
        {
            var room = await _roomRepository.GetRoomById(ObjectId.Parse(signUpMemberDTO.RoomId));
            if (room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var roomFields = room.Fields;
            var newMemberFields = signUpMemberDTO.Fields;

            foreach (var roomField in roomFields)
            {
                if (newMemberFields.ContainsKey(roomField.Name))
                {
                    var newMemberFieldValue = newMemberFields[roomField.Name];

                    if (roomField.Type == FieldType.Input)
                    {
                        var mask = roomField.Data["Mask"];
                        if (mask.IsString)
                        {
                            var regex = new Regex(mask.AsString);
                            if (!regex.IsMatch(newMemberFieldValue))
                                return (OperationErrorMessages.FieldRegexMissmatch, roomField.Name);
                        }
                    }
                    else if (roomField.Type == FieldType.Select)
                    {
                        var options = roomField.Data["Options"];
                        if (options.IsBsonArray && !options.AsBsonArray.Select(item => item.AsString).ToArray().Contains(newMemberFieldValue))
                            return (OperationErrorMessages.FieldOptionNotInList, roomField.Name);
                    }
                }
                else if (roomField.IsRequired)
                    return (OperationErrorMessages.FieldNotPassed, roomField.Name);
            }

            var memberByFields = await _memberRepository.GetMemberByFields(room.Id, newMemberFields);

            //var fieldsExists = memberByFields != null;
            //switch (room.IsApproveManually ? 100 : 0 + (room.IsRegistrationEnabled ? 10 : 0) + (fieldsExists ? 1 : 0))
            //{
            //    case 000:
            //        return (OperationErrorMessages.MemberNotFound, null);

            //    case 001:

            //}

            return (OperationErrorMessages.NoError, "OK");
        }
    }
}