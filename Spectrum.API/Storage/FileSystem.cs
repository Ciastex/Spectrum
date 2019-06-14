using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Spectrum.API.Logging;

namespace Spectrum.API.Storage
{
    public class FileSystem
    {
        public string RootDirectory { get; }
        public string VirtualFileSystemRoot => Path.Combine(RootDirectory, Defaults.PrivateDataDirectory);

        private static Logger Log { get; set; }

        static FileSystem()
        {
            Log = new Logger(Defaults.FileSystemLogFileName);
        }

        public FileSystem()
        {
            RootDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            if (!Directory.Exists(VirtualFileSystemRoot))
                Directory.CreateDirectory(VirtualFileSystemRoot);
        }

        public bool FileExists(string path)
        {
            var targetFilePath = Path.Combine(VirtualFileSystemRoot, path);
            return File.Exists(targetFilePath);
        }

        public bool DirectoryExists(string path)
        {
            var targetDirectoryPath = Path.Combine(VirtualFileSystemRoot, path);
            return Directory.Exists(targetDirectoryPath);
        }

        public bool PathExists(string path)
            => FileExists(path) || DirectoryExists(path);

        public byte[] ReadAllBytes(string filePath)
        {
            var targetFilePath = Path.Combine(VirtualFileSystemRoot, filePath);

            if (!File.Exists(targetFilePath))
            {
                Log.Error($"Couldn't read a file for path '{targetFilePath}'. File does not exist.");
                return null;
            }

            return File.ReadAllBytes(targetFilePath);
        }

        public FileStream CreateFile(string filePath, bool overwrite = false)
        {
            var targetFilePath = Path.Combine(VirtualFileSystemRoot, filePath);

            if (File.Exists(targetFilePath))
            {
                if (overwrite)
                {
                    Log.Error($"Couldn't delete a PluginData file for path '{targetFilePath}'. File does not exist.");
                    RemoveFile(filePath);
                }
                else
                {
                    Log.Error($"Couldn't create a PluginData file for path '{targetFilePath}'. The file already exists.");
                    return null;
                }
            }

            try
            {
                return File.Create(targetFilePath);
            }
            catch (Exception ex)
            {
                Log.Error($"Couldn't create a PluginData file for path '{targetFilePath}'.");
                Log.Exception(ex);

                return null;
            }
        }

        public void RemoveFile(string filePath)
        {
            var targetFilePath = Path.Combine(VirtualFileSystemRoot, filePath);

            if (!File.Exists(targetFilePath))
            {
                Log.Error($"Couldn't delete a PluginData file for path '{targetFilePath}'. File does not exist.");
                return;
            }

            try
            {
                File.Delete(targetFilePath);
            }
            catch (Exception ex)
            {
                Log.Error($"Couldn't delete a PluginData file for path '{targetFilePath}'.");
                Log.Exception(ex);
            }
        }

        public void IterateOver(string directoryPath, Action<string, bool> action, bool sort = true)
        {
            var targetDirectoryPath = Path.Combine(VirtualFileSystemRoot, directoryPath);

            if (!Directory.Exists(targetDirectoryPath))
            {
                Log.Error($"Cannot iterate over directory at '{targetDirectoryPath}'. It doesn't exist.");
                return;
            }

            var paths = Directory.GetFiles(targetDirectoryPath);
            paths.AddRange(Directory.GetDirectories(targetDirectoryPath));

            if (sort)
                paths = paths.OrderBy(x => x).ToArray();

            foreach (var path in paths)
            {
                try
                {
                    var isDirectory = Directory.Exists(path);
                    action(path, isDirectory);
                }
                catch (Exception e)
                {
                    Log.Error($"Action for the element at path '{path}' failed. See file system exception log for details.");
                    Log.ExceptionSilent(e);

                    return;
                }
            }
        }

        public List<string> GetDirectories(string directoryPath, string searchPattern)
        {
            var targetDirectoryPath = Path.Combine(VirtualFileSystemRoot, directoryPath);

            if (!Directory.Exists(targetDirectoryPath))
            {
                Log.Error($"Cannot get directories in directory at '{targetDirectoryPath}'. It doesn't exist.");
                return null;
            }

            return Directory.GetDirectories(targetDirectoryPath, searchPattern).ToList();
        }

        public List<string> GetDirectories(string directoryPath)
            => GetDirectories(directoryPath, "*");

        public List<string> GetFiles(string directoryPath, string searchPattern)
        {
            var targetDirectoryPath = Path.Combine(VirtualFileSystemRoot, directoryPath);

            if (!Directory.Exists(targetDirectoryPath))
            {
                Log.Error($"Cannot get files in directory at '{targetDirectoryPath}'. It doesn't exist.");
                return null;
            }

            return Directory.GetFiles(targetDirectoryPath, searchPattern).ToList();
        }

        public List<string> GetFiles(string directoryPath)
            => GetFiles(directoryPath, "*");

        public FileStream OpenFile(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            var targetFilePath = Path.Combine(VirtualFileSystemRoot, filePath);

            if (!File.Exists(targetFilePath))
            {
                Log.Error($"Couldn't open the file. The requested file: '{targetFilePath}' does not exist.");
                return null;
            }

            try
            {
                return File.Open(targetFilePath, fileMode, fileAccess, fileShare);
            }
            catch (Exception ex)
            {
                Log.Error($"Couldn't open a PluginData file for path '{targetFilePath}'.");
                Log.Exception(ex);

                return null;
            }
        }

        public FileStream OpenFile(string filePath)
            => OpenFile(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

        public string CreateDirectory(string directoryName)
        {
            var targetDirectoryPath = Path.Combine(VirtualFileSystemRoot, directoryName);

            try
            {
                Directory.CreateDirectory(targetDirectoryPath);
                return targetDirectoryPath;
            }
            catch (Exception ex)
            {
                Log.Error($"Couldn't create a PluginData directory for path '{targetDirectoryPath}'.");
                Log.Exception(ex);

                return string.Empty;
            }
        }

        public void RemoveDirectory(string directoryPath)
        {
            var targetDirectoryPath = Path.Combine(VirtualFileSystemRoot, directoryPath);

            if (!Directory.Exists(targetDirectoryPath))
            {
                Log.Error($"Couldn't remove a PluginData directory for path '{targetDirectoryPath}'. Directory does not exist.");
                return;
            }

            try
            {
                Directory.Delete(targetDirectoryPath, true);
            }
            catch (Exception ex)
            {
                Log.Error($"Couldn't remove a PluginData directory for path '{targetDirectoryPath}'.");
                Log.Exception(ex);
            }
        }

        public static string GetValidFileName(string dirtyFileName, string replaceInvalidCharsWith = "_")
            => Resource.GetValidFileName(dirtyFileName, replaceInvalidCharsWith);

        public static string GetValidFileNameToLower(string dirtyFileName, string replaceInvalidCharsWith = "_")
            => Resource.GetValidFileNameToLower(dirtyFileName, replaceInvalidCharsWith);
    }
}
