#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Dock.Model.Core;

namespace AvaloniaEditorTemplate.ViewModels;

public class ShellViewModel : ReactiveObject
{
    private IDock layout;
    public IDock Layout {
        get => layout;
        set => this.RaiseAndSetIfChanged(ref layout, value);
    }

    private IFactory factory;
    public IFactory Factory {
        get => factory;
        set => this.RaiseAndSetIfChanged(ref factory, value);
    }
}
