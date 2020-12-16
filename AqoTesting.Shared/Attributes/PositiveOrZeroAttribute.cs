using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.Attributes
{
    public class PositiveOrZeroAttribute : ValidationAttribute
    {
        public PositiveOrZeroAttribute() { }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var valueType = value.GetType();

            if(value != null)
                if( valueType == typeof(int) )
                {
                    if( (int)value < 0 )
                        return new ValidationResult(
                            $"The field {context.DisplayName} must be a positive number or zero." );
                }
                else if( valueType == typeof(float) )
                {
                    if( (float)value < 0 )
                        return new ValidationResult(
                            $"The field {context.DisplayName} must be a positive number or zero." );
                }
                else if( valueType == typeof(double) )
                {
                    if( (double)value < 0 )
                        return new ValidationResult(
                            $"The field {context.DisplayName} must be a positive number or zero." );
                }

            return ValidationResult.Success;
        }
    }
}

