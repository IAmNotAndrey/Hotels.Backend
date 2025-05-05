using Ardalis.GuardClauses;
using System.ComponentModel.DataAnnotations;

namespace Hotels.Presentation.Attributes;

//fixme ? Возможны ошибки, нужно протестировать
public sealed class GreaterOrEqualThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public GreaterOrEqualThanAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not IComparable currentValue)
        {
            throw new ArgumentException($"Parameter must implement '{nameof(IComparable)}'", nameof(value));
        }

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        Guard.Against.Null(property);

        var comparisonValue = property.GetValue(validationContext.ObjectInstance);

        if (comparisonValue is not IComparable)
        {
            throw new ArgumentException($"Comparison property must implement '{nameof(IComparable)}'", _comparisonProperty);
        }

        if (currentValue.CompareTo(comparisonValue) < 0)
        {
            return new ValidationResult(ErrorMessage ?? $"'{validationContext.DisplayName}' must be greater than or equal to '{_comparisonProperty}'");
        }

        return ValidationResult.Success;
    }
}
