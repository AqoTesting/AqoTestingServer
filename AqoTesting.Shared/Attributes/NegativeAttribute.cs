using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.Attributes
{
    public class NegativeAttribute : ValidationAttribute
    {
        public NegativeAttribute() { }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var valueType = value.GetType();

            if( value != null )
                if( valueType == typeof(int) )
                {
                    if( (int) value >= 0 )
                        return new ValidationResult(
                            $"The field {context.DisplayName} must be a negative number." );
                }
                else if( valueType == typeof(float) )
                {
                    if( (float) value >= 0 )
                        return new ValidationResult(
                            $"The field {context.DisplayName} must be a negative number." );
                }
                else if( valueType == typeof(double) )
                {
                    if( (double) value >= 0 )
                        return new ValidationResult(
                            $"The field {context.DisplayName} must be a negative number." );
                }

            return ValidationResult.Success;
        }
    }
}

