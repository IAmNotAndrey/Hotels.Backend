using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Hotels.Presentation.Attributes;

public sealed partial class CoordinatesAttribute : ValidationAttribute
{

    // Регулярное выражение для проверки формата координат "latitude,longitude"
    private const string CoordinatesPattern = @"^(\-?\d+(\.\d+)?),\s*(\-?\d+(\.\d+)?)$";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success!; // значение null считается допустимым
        }

        var coordinateString = value.ToString();

        if (string.IsNullOrWhiteSpace(coordinateString))
        {
            return new ValidationResult("Coordinates cannot be empty.");
        }

        // Проверка соответствия строке шаблона координат
        if (!MyCoordinatesRegex().IsMatch(coordinateString))
        {
            return new ValidationResult("Invalid coordinates format. Expected format: 'latitude,longitude'.");
        }

        // Разделение координат на широту и долготу
        var parts = coordinateString.Split(',');

        if (parts.Length != 2)
        {
            return new ValidationResult("Invalid coordinates format. Expected format: 'latitude,longitude'.");
        }

        // Преобразование строк в числа и проверка допустимых диапазонов
        if (double.TryParse(parts[0], out double latitude) && double.TryParse(parts[1], out double longitude))
        {
            if (latitude < -90 || latitude > 90)
            {
                return new ValidationResult("Latitude must be between -90 and 90.");
            }

            if (longitude < -180 || longitude > 180)
            {
                return new ValidationResult("Longitude must be between -180 and 180.");
            }
        }
        else
        {
            return new ValidationResult("Invalid numeric values for latitude or longitude.");
        }

        return ValidationResult.Success!;
    }

    [GeneratedRegex(CoordinatesPattern)]
    private static partial Regex MyCoordinatesRegex();
}