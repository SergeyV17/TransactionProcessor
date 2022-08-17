using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace TransactionProcessor.Extensions;

public static class ValidationExtensions
{
    private static readonly string[] Formats = 
    {
        "MM/dd/yyyy hh:mm:ss tt",
        "yyyy-MM-dd hh:mm:ss",
        "yyyy.MM.dd hh:mm:ss",
        "yyyy,MM,dd hh:mm:ss",
        "dd.MM.yyyy hh:mm:ss",
        "dd-MM-yyyy hh:mm:ss",
        "dd,MM,yyyy hh:mm:ss",
        "dd/MM/yyyy hh:mm:ss",
        "yyyy-MM-ddTHH:mm:sszz",
    };
    
    public static T GetValueIfValid<T>(this string? input) where T : struct
    {
        var handledInput = input?.Trim();
        if (string.IsNullOrEmpty(handledInput))
        {
            throw new ValidationException("Входящая строка не может быть пустой");
        }

        return Convert<T>(handledInput);
    }
    
    private static T Convert<T>(string input) where T : struct
    {
        var converter = TypeDescriptor.GetConverter(typeof(T));
        if (converter is DateTimeConverter)
        {
            DateTime.TryParseExact(input, Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime);
            return (T)(object)dateTime;
        }
        if(converter?.IsValid(input) is true)
        {
            return (T)converter.ConvertFromString(input)!;
        }
        
        throw new ValidationException("Некорректный формат значения.");
    }
}