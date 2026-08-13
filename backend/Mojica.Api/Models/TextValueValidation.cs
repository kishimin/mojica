using System.Globalization;

namespace Mojica.Api.Models;

internal static class TextValueValidation
{
    private const int MaximumCodeUnitsPerAllowedGrapheme = 1_024;

    public static ModelValidationReason? GetFailureReason(
        string value,
        int maximumGraphemes,
        bool rejectWhitespaceOnly)
    {
        // Unicode permits arbitrarily long grapheme clusters, so this defensive ceiling bounds
        // validation work while remaining far above realistic user-perceived characters.
        if (value.Length is 0
            || value.Length > maximumGraphemes * MaximumCodeUnitsPerAllowedGrapheme
            || ExceedsGraphemeLimit(value, maximumGraphemes))
        {
            return ModelValidationReason.LengthOutOfRange;
        }

        if (rejectWhitespaceOnly && string.IsNullOrWhiteSpace(value))
        {
            return ModelValidationReason.NotBlank;
        }

        return value.Any(char.IsControl)
            ? ModelValidationReason.ControlCharacter
            : null;
    }

    private static bool ExceedsGraphemeLimit(string value, int maximumGraphemes)
    {
        var graphemes = StringInfo.GetTextElementEnumerator(value);

        for (var count = 0; graphemes.MoveNext(); count++)
        {
            if (count == maximumGraphemes)
            {
                return true;
            }
        }

        return false;
    }
}
