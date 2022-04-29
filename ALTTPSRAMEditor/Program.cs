namespace ALTTPSRAMEditor;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        var mainForm = new MainForm();
        Application.Run(mainForm);
    }
}
