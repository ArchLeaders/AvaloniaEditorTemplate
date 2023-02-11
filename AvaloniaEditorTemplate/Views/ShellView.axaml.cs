using Avalonia.Controls;

namespace AvaloniaEditorTemplate.Views;

public partial class ShellView : Window
{
    public ShellView()
    {
        InitializeComponent();
        DataContext = Shell;
    }
}
