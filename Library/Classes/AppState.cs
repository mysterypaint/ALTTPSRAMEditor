namespace Library.Classes;

public static class AppState
{
    public static Dictionary<ushort, char> rawENChar = new();
    public static Dictionary<ushort, char> rawJPChar = new();

    public static Dictionary<char, int> enChar = new();
    public static Dictionary<char, int> jpChar = new();

    private static readonly Data dataHandler = new Data(enChar, jpChar, rawENChar, rawJPChar);
}
