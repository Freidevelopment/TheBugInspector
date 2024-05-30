using System.ComponentModel.DataAnnotations;
using System.Reflection;

internal static class EnumExtensionsHelpers
{
    public static string GetDisplayName(this Enum enumValue)
    {
        string? displayName = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName();

        if (string.IsNullOrEmpty(displayName))
        {
            displayName = enumValue.ToString();
        }

        return displayName;
    }
}