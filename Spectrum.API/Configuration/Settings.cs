using Newtonsoft.Json;
using Spectrum.API.Logging;
using System;
using System.IO;
using System.Reflection;

namespace Spectrum.API.Configuration
{
    public class Settings : Section
    {
        private static Logger _logger;
        private static Logger Logger => _logger ?? (_logger = new Logger(Defaults.SettingsSystemLogFileName) { WriteToConsole = true });
        private FileSystemWatcher Watcher;

        private string FileName { get; }
        private string RootDirectory { get; }
        private string SettingsDirectory => Path.Combine(RootDirectory, Defaults.PrivateSettingsDirectory);
        private string FilePath => Path.Combine(SettingsDirectory, FileName);
        public event Action<Settings, FileSystemEventArgs> OnChanged;
        
        public Settings(string fileName)
        {
            RootDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            FileName = $"{fileName}.json";

            Logger.Info($"Settings instance for '{FilePath}' initializing...");

            if (File.Exists(FilePath))
            {
                using (var sr = new StreamReader(FilePath))
                {
                    var json = sr.ReadToEnd();
                    Section sec = null;

                    try
                    {
                        sec = JsonConvert.DeserializeObject<Section>(json);
                    }
                    catch (JsonException je)
                    {
                        Logger.Error($"Could not deserialize JSON in '{FilePath}'. Probably a syntax error. Check the log file for details.");
                        Logger.ExceptionSilent(je);
                    }
                    catch (Exception e)
                    {
                        Logger.Error($"Unexpected exception occured while loading file {FilePath}. Check the log file for details.");
                        Logger.ExceptionSilent(e);
                    }

                    if (sec != null)
                    {
                        foreach (string k in sec.Keys)
                            Add(k, sec[k]);
                    }
                }
            }

            Watcher = new FileSystemWatcher
            {
                Path = FilePath,
                Filter = "*",
                NotifyFilter = NotifyFilters.FileName
                             | NotifyFilters.LastAccess
                             | NotifyFilters.LastWrite
                             | NotifyFilters.DirectoryName
                             | NotifyFilters.CreationTime
            };

            var eventhandler = new FileSystemEventHandler(OnFileChanged);
            Watcher.Created += eventhandler;
            Watcher.Changed += eventhandler;
            Watcher.Deleted += eventhandler;

            Watcher.EnableRaisingEvents = true;

            Dirty = false;
        }

        ~Settings()
        {
            Watcher.Dispose();
        }
        
        private void OnFileChanged(object source, FileSystemEventArgs e)
        {
            OnChanged(this, e);
        }

        public void Save(bool formatJson = true)
        {
            if (!Directory.Exists(SettingsDirectory))
                Directory.CreateDirectory(SettingsDirectory);

            try
            {
                using (var sw = new StreamWriter(FilePath, false))
                    sw.WriteLine(JsonConvert.SerializeObject(this, (formatJson ? Formatting.Indented : Formatting.None)));

                Dirty = false;
            }
            catch (JsonException je)
            {
                Logger.Error($"Could not serialize the settings object back to JSON for '{FilePath}'. See the log file for details.");
                Logger.ExceptionSilent(je);
            }
            catch (Exception e)
            {
                Logger.Error($"An unexpected exception occured while saving '{FilePath}'. See the log file for details.");
                Logger.ExceptionSilent(e);
            }
        }
    }
}
