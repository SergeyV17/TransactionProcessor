using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TransactionProcessor.Extensions;

public static class ValidationExtensions
{
    public static T GetValueIfValid<T>(this string? input) where T : struct
    {
        var handledInput = input?.Trim();
        if (string.IsNullOrEmpty(handledInput))
        {
            throw new ValidationException("Входящая строка не может быть пустой");
        }

        return Convert<T>(handledInput);
        
        // var type = typeof(T);
        // var dictionary = new Dictionary<Type, Func<string, T>> 
        // {
        //     [typeof(int)] = Convert<T>,
        // };
        //
        // var action = dictionary[type];
        // var result = action(handledInput);
        //
        // return result;
    }
    
    private static T Convert<T>(string input)
    {
        var converter = TypeDescriptor.GetConverter(typeof(T));
        if(converter is not null && converter.IsValid(input))
        {
            return (T)converter.ConvertFromString(input)!;
        }
        
        throw new ValidationException("Некорректный формат значения.");
    }
    
    // private static T ValidateInteger<T>(string s)
    // {
    //     if (typeof(T).TryParse(s, out var id))
    //     {
    //         return (T)Convert.ChangeType(id, typeof(T));
    //     }
    //
    //     throw new ValidationException("Некорректный формат числового значения.");
    // }
}