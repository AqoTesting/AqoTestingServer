using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.Attributes
{
    public class PositiveAttribute : ValidationAttribute
    {
        public PositiveAttribute() { }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var valueType = value.GetType();

            if(value != null)
                if( valueType == typeof(int) )
                {
                    if( (int)value <= 0 )
                        return new ValidationResult(
                            $"The field {context.DisplayName} must be a positive number." );
                }
                else if( valueType == typeof(float) )
                {
                    if( (float)value <= 0 )
                        return new ValidationResult(
                            $"The field {context.DisplayName} must be a positive number." );
                }
                else if( valueType == typeof(double) )
                {
                    if( (double)value <= 0 )
                        return new ValidationResult(
                            $"The field {context.DisplayName} must be a positive number." );
                }

            return ValidationResult.Success;
        }
    }
}

