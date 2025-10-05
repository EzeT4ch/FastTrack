using System.Reflection;

namespace FastTrack.Persistence.Extensions;

public static class PrivateSetterExtensions
{
    public static void SetPrivateProperty<T>(this T target, string propertyName, object? value)
    {
        PropertyInfo? property = typeof(T)
            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        
        if (property == null)
        {
            throw new ArgumentException($"No se encontró la propiedad '{propertyName}' en el tipo {typeof(T).Name}");
        }

        property.SetValue(target, value);
    }
}