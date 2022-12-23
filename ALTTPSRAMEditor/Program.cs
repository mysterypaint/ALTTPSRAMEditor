var thread = new Thread(() =>
{
    Application.SetHighDpiMode(HighDpiMode.SystemAware);
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new MainForm(new TextCharacterData()));
});
#pragma warning disable CA1416 // Validate platform compatibility
thread.SetApartmentState(ApartmentState.STA);
#pragma warning restore CA1416 // Validate platform compatibility
thread.Start();
