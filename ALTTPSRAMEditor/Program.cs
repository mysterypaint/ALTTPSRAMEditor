var thread = new Thread(() =>
{
    Application.SetHighDpiMode(HighDpiMode.SystemAware);
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new MainForm(new TextCharacterData()));
});
thread.SetApartmentState(ApartmentState.STA);
thread.Start();
