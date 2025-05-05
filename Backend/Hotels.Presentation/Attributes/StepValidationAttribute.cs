using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.Attributes;

public class StepValidationAttribute : ValidationAttribute
{
    private readonly float _min;
    private readonly float _max;
    private readonly float _step;

    public StepValidationAttribute(float min, float max, float step)
    {
        _min = min;
        _max = max;
        _step = step;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not float floatValue)
        {
            return new ValidationResult("Invalid data type.");
        }

        // Проверка диапазона
        if (floatValue < _min || floatValue > _max)
        {
            return new ValidationResult($"The value must be between {_min} and {_max}.");
        }

        // Проверка шага
        if ((floatValue - _min) % _step != 0)
        {
            return new ValidationResult($"The value must be a multiple of {_step}.");
        }

        return ValidationResult.Success;
    }
}
