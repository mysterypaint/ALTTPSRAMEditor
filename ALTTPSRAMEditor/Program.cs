namespace ALTTPSRAMEditor;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        var form1 = new Form1();
        Application.Run(form1);
    }
}
