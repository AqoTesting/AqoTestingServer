using AqoTesting.Shared.DTOs.API.CommonAPI;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AqoTesting.Core.Utils
{
    public static class FieldsValidator
    {
        public static (bool, OperationErrorMessages, object) Validate(RoomsDB_FieldDTO[] patternFields, Dictionary<string, string> inputFields)
        {
            foreach(var patternField in patternFields)
            {
                if(inputFields.ContainsKey(patternField.Name))
                {
                    var inputFieldValue = inputFields[patternField.Name];

                    if(patternField.Type == FieldType.Input)
                    {
                        var mask = patternField.Data["Mask"];
                        if(mask.IsString)
                        {
                            var regex = new Regex(mask.AsString);
                            if(!regex.IsMatch(inputFieldValue))
                                return (false, OperationErrorMessages.FieldRegexMismatch, new CommonAPI_ErrorDTO { ErrorSubject = patternField.Name });
                        }
                    }
                    else if(patternField.Type == FieldType.Select)
                    {
                        var options = patternField.Data["Options"];
                        if(options.IsBsonArray && !options.AsBsonArray.Select(item => item.AsString).ToArray().Contains(inputFieldValue))
                            return (false, OperationErrorMessages.FieldOptionNotInList, new CommonAPI_ErrorDTO { ErrorSubject = patternField.Name });
                    }
                }
                else if(patternField.IsRequired)
                    return (false, OperationErrorMessages.FieldNotPassed, new CommonAPI_ErrorDTO { ErrorSubject = patternField.Name });
            }

            return (true, OperationErrorMessages.NoError, null);
        }
    }
}
