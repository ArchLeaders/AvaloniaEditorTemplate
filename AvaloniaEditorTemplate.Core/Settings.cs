#pragma warning disable CA1822 // Mark members as static

using Avalonia.SettingsFactory.Core;
using System.Runtime.InteropServices;
using System.Text.Json;
using static System.Environment;

namespace AvaloniaEditorTemplate.Core;

public class Settings : ISettingsBase
{
    private static Settings? _config = null;
    public static Settings Config => _config ?? throw new Exception("The settings were not loaded, please use Settings.LoadConfig() to initialize the settings");
    public static string DataFolder { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"{GetFolderPath(SpecialFolder.LocalApplicationData)}/{nameof(AvaloniaEditorTemplate)}" : $"{GetFolderPath(SpecialFolder.ApplicationData)}/{nameof(AvaloniaEditorTemplate)}";

    public bool RequiresInput { get; set; }

    [Setting("Base Game Directory", "The folder containing the base game files for BOTW, without the update ir DLC files. The last folder should be \"content\", e.g. \"C:/Games/Botw/BaseGame/content\"")]
    public string GameDir { get; set; } = "";

    [Setting("Update Directory", "The folder containing the update files for BOTW, version 1.5.0. The last folder should be \"content\", e.g. \"C:/Games/Botw/Update/content\"")]
    public string UpdateDir { get; set; } = "";

    [Setting("DLC Directory", "The folder containing the DLC files for BOTW, version 3.0. The last folder should be \"0010\", e.g. \"C:/Games/Botw/DLC/content/0010\"")]
    public string DlcDir { get; set; } = "";

    [Setting("Switch Base Game Directory", "Path should end in '01007EF00011E000/romfs'")]
    public string GameDirNx { get; set; } = "";

    [Setting("Switch DLC Directory", "Path should end in '01007EF00011F001/romfs'")]
    public string DlcDirNx { get; set; } = "";

    [Setting(UiType.Dropdown, "Switch", "WiiU")]
    public string Mode { get; set; } = "WiiU";

    [Setting(UiType.Dropdown, "Dark", "Light", Category = "Appearance")]
    public string Theme { get; set; } = "Dark";

    public static void LoadConfig()
    {
        if (File.Exists($"{DataFolder}/Config.json"))
        {
            _config = JsonSerializer.Deserialize<Settings>(File.ReadAllText($"{DataFolder}/Config.json")) ?? new();
        }
        else if (File.Exists($"{DataFolder}/../bcml/settings.json"))
        {
            _config = new();

            Dictionary<string, object> settings =
                JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText($"{DataFolder}/../bcml/settings.json")) ?? new();

            _config.GameDir = settings["game_dir"].ToString() ?? "";
            _config.GameDirNx = settings["game_dir_nx"].ToString() ?? "";
            _config.UpdateDir = settings["update_dir"].ToString() ?? "";
            _config.DlcDir = settings["dlc_dir"].ToString() ?? "";
            _config.DlcDirNx = settings["dlc_dir_nx"].ToString() ?? "";
            _config.Save();
        }
        else
        {
            _config = new()
            {
                RequiresInput = true
            };
            _config.Save();
        }
    }

    public ISettingsBase Save()
    {
        Directory.CreateDirectory(DataFolder);
        File.WriteAllText($"{DataFolder}/Config.json", JsonSerializer.Serialize(this));
        return this;
    }

    public bool ValidateFolder(string path, string mode)
    {
        if (path == null || File.Exists(path))
        {
            return false;
        }

        return mode switch
        {
            "GameDir" => File.Exists($"{path}/Pack/Dungeon000.pack") && path.EndsWith("content"),
            "UpdateDir" => File.Exists($"{path}/Actor/Pack/ActorObserverByActorTagTag.sbactorpack") && path.EndsWith("content"),
            "DlcDir" => File.Exists($"{path}/Pack/AocMainField.pack") && path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).EndsWith("content/0010"),
            "GameDirNx" => File.Exists($"{path}/Actor/Pack/ActorObserverByActorTagTag.sbactorpack") && File.Exists($"{path}/Pack/Dungeon000.pack") && path.EndsWith("romfs"),
            "DlcDirNx" => File.Exists($"{path}/Pack/AocMainField.pack") && path.EndsWith("romfs"),
            _ => false,
        };
    }
}
