using Cyberdeck.Data;
using Cyberdeck.Desktop.Views.Pages;
using Cyberdeck.Desktop.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Cyberdeck.Data.Seed;
using Cyberdeck.Data.Import;
using System.Diagnostics;

namespace Cyberdeck.Desktop;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=cyberdeck.db"));

        services.AddTransient<DeckBuilderViewModel>();
        services.AddTransient<MainWindow>();
        services.AddTransient<DeckBuilderPage>();
        services.AddTransient<CardService>();
        services.AddTransient<CardImporterService>();

        Services = services.BuildServiceProvider();

        using var scope = Services.CreateScope();


        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();


        var importer = scope.ServiceProvider.GetService<CardImporterService>();
        await importer.ImportCardsAsync("cards.json");


        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }
}