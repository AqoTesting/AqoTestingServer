using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.Attributes
{
    public class ArrayStringLengthAttribute : ValidationAttribute
    {
        public int MinimumLength;
        private int _maximumLength;
        public ArrayStringLengthAttribute(int maximumLength) 
        {
            _maximumLength = maximumLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if(value != null)
                foreach(var @string in (string[]) value)
                    if(MinimumLength != 0)
                    {
                        if(@string.Length > _maximumLength || @string.Length < MinimumLength)
                            return new ValidationResult(
                                $"The field {context.DisplayName} must be an array of strings with a minimum length of {MinimumLength} and maximum length of {_maximumLength}." );

                    }
                    else if(@string.Length > _maximumLength)
                        return new ValidationResult(
                            $"The field {context.DisplayName} must be an array of strings with a  maximum length of {_maximumLength}." );

            return ValidationResult.Success;
        }
    }
}
