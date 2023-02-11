global using static AvaloniaEditorTemplate.App;
global using static AvaloniaEditorTemplate.Core.Settings;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Generics;
using Avalonia.Generics.Builders;
using Avalonia.Markup.Xaml;
using Avalonia.Themes.Fluent;
using AvaloniaEditorTemplate.Models;
using AvaloniaEditorTemplate.ViewModels;
using AvaloniaEditorTemplate.Views;

namespace AvaloniaEditorTemplate;

public partial class App : Application
{
    public static string Title { get; } = "$projectname$";
    public static string? Version { get; } = typeof(App).Assembly.GetName().Version?.ToString(3);

    public static ShellView ShellView { get; set; } = null!;
    public static ShellViewModel Shell { get; set; } = new();
    public static FluentTheme Theme { get; set; } = new(new Uri("avares://$safeprojectname$/Styles"));

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        LoadConfig();

        Theme.Mode = Config.Theme == "Dark" ? FluentThemeMode.Dark : FluentThemeMode.Light;
        Current!.Styles[0] = Theme;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            ShellView = new();
            desktop.MainWindow = WindowBuilder.Initialize(ShellView)
                .WithMenu(new ShellMenuModel())
                .WithWindowColors("SystemChromeLowColor", "SystemChromeHighColor", 0.4)
                .WithMinBounds(800, 450)
                .Build();

#if DEBUG
            desktop.MainWindow.AttachDevTools();
#endif
            ApplicationLoader.Attach(this);

            // Make sure settings are always set
            SettingsView settings = new();
            if (Config.RequiresInput || settings.ValidateSave() != null) {
                DockFactory.AddDocument(new SettingsViewModel());
                await Task.Run(() => {
                    while (Config.RequiresInput) { }
                });
            }

            // Create dock layout
            var factory = new DockFactory(Shell);
            var layout = factory.CreateLayout();
            factory.InitLayout(layout);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
