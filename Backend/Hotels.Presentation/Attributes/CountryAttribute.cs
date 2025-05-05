using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Hotels.Presentation.Attributes;

// fixme : возможны ошибки, нужно тестить
public class CountryAttribute : ValidationAttribute
{
    private static readonly HashSet<string> _countryCodes = new(
           CultureInfo.GetCultures(CultureTypes.SpecificCultures)
               .Select(culture => new RegionInfo(culture.Name).TwoLetterISORegionName)
               .Distinct()
    );

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string countryCode = value.ToString()!;
        if (value == null || string.IsNullOrWhiteSpace(countryCode))
        {
            return new ValidationResult($"'{nameof(value)}' can't be null or white space");
        }

        if (!_countryCodes.Contains(countryCode))
        {
            return new ValidationResult("Invalid country code");
        }

        return ValidationResult.Success!;
    }
}
