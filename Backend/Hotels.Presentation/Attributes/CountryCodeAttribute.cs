using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Hotels.Presentation.Attributes;

public class CountryCodeAttribute : ValidationAttribute
{
    private static readonly HashSet<string> _validCountryCodes = new(
            CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(c => new RegionInfo(c.Name).TwoLetterISORegionName)
            .Distinct(),
            StringComparer.OrdinalIgnoreCase);


    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string countryCode)
        {
            return new ValidationResult("Country code must be a string.");
        }

        if (!_validCountryCodes.Contains(countryCode))
        {
            return new ValidationResult($"'{countryCode}' is not a valid country code.");
        }

        return ValidationResult.Success;
    }
}
