using System.IO;
using System.Reflection;

namespace Spectrum.API
{
    public class Defaults
    {
        private static string BasePath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string PrivateDependencyDirectory => "Dependencies";
        public static string PrivateDataDirectory => "Data";
        public static string PrivateLogDirectory => "Logs";
        public static string PrivateSettingsDirectory => "Settings";
        public static string PrivateAssetsDirectory => "Assets";

        public static string ManagerPluginDirectory => Path.Combine(BasePath, "Plugins");
        public static string ManagerLogDirectory => Path.Combine(BasePath, "Logs");
        public static string ManagerSettingsDirectory => Path.Combine(BasePath, "Settings");

        public const string HotkeyManagerLogFileName = "HotkeyManager";
        public const string MenuManagerLogFileName = "MenuManager";
        public const string PluginLoaderLogFileName = "PluginLoader";
        public const string DependencyResolverLogFileName = "DependencyResolver";
        public const string FileSystemLogFileName = "FileSystem";
        public const string RuntimeAssetLoaderLogFileName = "RuntimeAssetLoader";
        public const string SettingsSystemLogFileName = "SettingsSystem";
        public const string ManagerLogFileName = "Manager";
        public const string EventRouterLogFileName = "EventRouter";
        public const string CheatSystemLogFileName = "CheatSystem";
    }
}
