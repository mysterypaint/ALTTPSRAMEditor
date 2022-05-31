namespace ALTTPSRAMEditor;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        var mainForm = new MainForm(new TextCharacterData());
        Application.Run(mainForm);
    }
}
