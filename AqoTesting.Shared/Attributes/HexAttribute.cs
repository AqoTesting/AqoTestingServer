using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.Attributes
{
    public class HexAttribute : RegularExpressionAttribute
    {
        public HexAttribute() : base(@"^[0-9abcdef]*$")
        {
            ErrorMessage = "The {0} field is not a valid hex string.";
        }
    }
}
