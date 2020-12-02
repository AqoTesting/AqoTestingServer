using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.Attributes
{
    public class ObjectIdAttribute : RegularExpressionAttribute
    {
        public ObjectIdAttribute() : base(@"^[0-9abcdef]{24}$")
        {
            ErrorMessage = "The {0} field is not a valid hex-string of length 24.";
        }
    }
}
