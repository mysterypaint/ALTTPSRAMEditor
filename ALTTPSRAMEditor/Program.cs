#pragma warning disable CA1416 // Validate platform compatibility
var thread = new Thread(() =>
{
    var builder = Host.CreateDefaultBuilder(args);

    builder.ConfigureServices(
        services => services
            .AddScoped<TextCharacterData>()
            .AddScoped<MainForm>());

    using var host = builder.Build();

    Application.SetHighDpiMode(HighDpiMode.SystemAware);
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);

    Application.Run(host.Services.GetRequiredService<MainForm>());
});

thread.SetApartmentState(ApartmentState.STA);

thread.Start();
#pragma warning restore CA1416 // Validate platform compatibility
