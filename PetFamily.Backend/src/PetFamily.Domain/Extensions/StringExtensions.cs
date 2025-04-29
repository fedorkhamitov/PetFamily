using System.Text.RegularExpressions;
namespace PetFamily.Domain.Extensions;

public static class StringExtensions
{
    public static bool IsOnlyWords(this string str)
    {
        var regex = new Regex(@"^[A-Za-zА-Яа-яЁёs-]+$");
        return regex.IsMatch(str);
    }

    public static bool IsEmail(this string str)
    {
        var regex = new Regex(@"^[^@s]+@[^@a]+.[^@s]+$");
        return regex.IsMatch(str);
    }
}