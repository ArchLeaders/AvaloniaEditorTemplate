using Avalonia.Controls;
using Avalonia.Input;

namespace AvaloniaEditorTemplate.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
        DragRegion.AddHandler(DragDrop.DropEvent, DragDropEvent);
    }

    public void DragDropEvent(object? sender, DragEventArgs e)
    {
        IEnumerable<string>? paths = e.Data.GetFileNames();

        if (paths != null) {
            foreach (var path in paths) {
                // Handle Path
            }
        }
    }
}
