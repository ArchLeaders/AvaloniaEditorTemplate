using AvaloniaEditorTemplate.Core.Extensions;
using Dock.Model.ReactiveUI.Controls;

namespace AvaloniaEditorTemplate.ViewModels;

public class HomeViewModel : Document
{
    public HomeViewModel()
    {
        Title = "Home";
        CanFloat = false;
        CanClose = false;
        CanPin = false;
    }

    public static async Task VersionLink()
    {
        await BrowserExtension.OpenUrl("https://github.com/$username$/$projectname$/releases/");
    }
}
