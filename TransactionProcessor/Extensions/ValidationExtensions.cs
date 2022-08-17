using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace TransactionProcessor.Extensions;

public static class ValidationExtensions
{
    private static readonly string[] DateTimeFormats = 
    {
        "MM/dd/yyyy hh:mm:ss tt",
        "yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz",
        "yyyy-MM-dd HH:mm:ss",
        "yyyy.MM.dd HH:mm:ss",
        "yyyy,MM,dd HH:mm:ss",
        "dd.MM.yyyy HH:mm:ss",
        "yyyy.MM.dd HH:mm:ss",
        "dd-MM-yyyy HH:mm:ss",
        "dd,MM,yyyy HH:mm:ss",
        "dd/MM/yyyy HH:mm:ss",
        "yyyy-MM-ddTHH:mm:sszz",
        "yyyy-MM-dd",
        "yyyy.MM.dd",
        "yyyy,MM,dd",
        "dd/MM/yyyy",
        "dd-MM-yyyy",
        "dd-MM-yyyy",
        "MM/dd/yyyy",
        "dd.MM.yyyy",
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
        switch (converter)
        {
            case DateTimeConverter:
            {
                DateTime.TryParseExact(input, DateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dateTime);

                if (dateTime != DateTime.MinValue)
                {
                    return (T)(object)dateTime;
                }
                throw new ValidationException("Некорректный формат даты.");
            }
            case DecimalConverter:
            {
                decimal.TryParse(input, out var amount);
                if (amount >= 0)
                {
                    return (T)(object)amount;
                }
                throw new ValidationException("Некорректный формат суммы.");
            }
        }

        if(converter?.IsValid(input) is true)
        {
            return (T)converter.ConvertFromString(input)!;
        }
        
        throw new ValidationException("Некорректный формат значения.");
    }
}