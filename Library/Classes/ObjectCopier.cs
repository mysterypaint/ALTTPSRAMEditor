namespace Library.Classes;

/// <summary>
/// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
/// Provides a method for performing a deep copy of an object.
/// Binary Serialization is used to perform the copy.
/// </summary>
public static class ObjectCopier
{
    /// <summary>
    /// Perform a deep Copy of the object.
    /// </summary>
    /// <typeparam name="T">The type of object being copied.</typeparam>
    /// <param name="source">The object instance to copy.</param>
    /// <returns>The copied object.</returns>
    public static T? Clone<T>(this T source)
    {
        if (source is null)
        {
            // Don't serialize a null object, simply return the default for that object
            return default;
        }

        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source));
    }
}
