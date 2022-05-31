namespace Library.Classes;

public static class AppState
{
    public static Dictionary<ushort, char> rawEnChar = new();
    public static Dictionary<ushort, char> rawJpChar = new();

    public static Dictionary<char, int> enChar = new();
    public static Dictionary<char, int> jpChar = new();

    private static readonly Data dataHandler = new(enChar, jpChar, rawEnChar, rawJpChar);
}
