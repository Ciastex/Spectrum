using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Spectrum.Prism.Enums;
using Spectrum.Prism.IO;
using Spectrum.Prism.Patches;
using Spectrum.Prism.Runtime;

namespace Spectrum.Prism
{
    internal class Program
    {
        private static string _distanceAssemblyFilename;
        private static string _bootstrapAssemblyFilename;
        private static string _requestedPatchName;

        private static ModuleDefinition _distanceAssemblyDefinition;
        private static ModuleDefinition _bootstrapAssemblyDefinition;

        private static Patcher _patcher;

        internal static void Main(string[] args)
        {
            WriteStartupHeader();

            if (!IsValidSyntax(args))
            {
                ColoredOutput.WriteInformation($"Usage: {GetExecutingFileName()} <-t (--target) Assembly-CSharp.dll> [options]");
                ColoredOutput.WriteInformation("  Options:");
                ColoredOutput.WriteInformation("    -t [--target]+: Specify the target Distance DLL you want to patch.");
                ColoredOutput.WriteInformation("    -s [--source]+: Specify the source DLL you want to cross-reference.");
                ColoredOutput.WriteInformation("    -p [--patch]+:  Run only patch with the specified name.");
                ErrorHandler.TerminateWithError("Invalid syntax provided.", TerminationReason.InvalidSyntax);
            }

            ParseArguments(args);

            if (string.IsNullOrEmpty(_distanceAssemblyFilename))
                ErrorHandler.TerminateWithError("Target DLL name not specified.", TerminationReason.TargetDllNotProvided);
            if ((args.Contains("-p") || args.Contains("--patch")) && string.IsNullOrEmpty(_requestedPatchName))
                ErrorHandler.TerminateWithError("Patch name not specified.", TerminationReason.PatchNameNotProvided);
            if ((args.Contains("-s") || args.Contains("--source")) && string.IsNullOrEmpty(_bootstrapAssemblyFilename))
                ErrorHandler.TerminateWithError("Source DLL name not specified.", TerminationReason.SourceDllNotProvided);

            if (!DistanceFileExists())
                ErrorHandler.TerminateWithError("Specified TARGET DLL not found.", TerminationReason.TargetDllNonexistant);

            if (!BootstrapFileExists() && (args.Contains("-s") || args.Contains("--source")))
                ErrorHandler.TerminateWithError("Specified SOURCE DLL not found.", TerminationReason.SourceDllNonexistant);

            CreateBackup();
            PreparePatches();
            RunPatches();
            ModuleWriter.SavePatchedFile(_distanceAssemblyDefinition, _distanceAssemblyFilename);

            // Run decapsulation after all other configured patches.
            _patcher.AddPatch(new DecapsulationPatch());
            _patcher.RunSpecific("Decapsulation");

            var devDllFileName = $"{Path.GetFileNameWithoutExtension(_distanceAssemblyFilename)}.dev.dll";
            ModuleWriter.SavePatchedFile(_distanceAssemblyDefinition, devDllFileName);
            ColoredOutput.WriteSuccess($"Saved decapsulated development DLL to {devDllFileName}");

            ColoredOutput.WriteSuccess("Patch process completed.");
        }

        private static void WriteStartupHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine($"Prism patcher for Spectrum. Version {version.Major}.{version.Minor}.{version.Build}.{version.Revision}");
            Console.WriteLine("------------------------------------------");
            Console.ResetColor();
        }

        private static bool IsValidSyntax(ICollection<string> args)
        {
            return args.Count >= 1 && (args.Contains("-t") || args.Contains("--target"));
        }

        private static void ParseArguments(string[] args)
        {
            for (var i = 0; i < args.Length; i++)
            {
                if ((args[i] == "-s" || args[i] == "--source") && (i + 1) < args.Length)
                {
                    _bootstrapAssemblyFilename = args[i + 1];
                    i++;
                }

                if ((args[i] == "-t" || args[i] == "--target") && (i + 1) < args.Length)
                {
                    _distanceAssemblyFilename = args[i + 1];
                    i++;
                }

                if ((args[i] == "-p" || args[i] == "--patch") && (i + 1) < args.Length)
                {
                    _requestedPatchName = args[i + 1];
                    i++;
                }
            }
        }

        private static string GetExecutingFileName()
        {
            return Path.GetFileName(Assembly.GetExecutingAssembly().Location);
        }

        private static bool DistanceFileExists()
        {
            return File.Exists(_distanceAssemblyFilename);
        }

        private static bool BootstrapFileExists()
        {
            return File.Exists(_bootstrapAssemblyFilename);
        }

        private static void CreateBackup()
        {
            if (!File.Exists($"{_distanceAssemblyFilename}.backup"))
            {
                ColoredOutput.WriteInformation("Performing a backup...");
                File.Copy($"{_distanceAssemblyFilename}", $"{_distanceAssemblyFilename}.backup");
            }
        }

        private static void PreparePatches()
        {
            ColoredOutput.WriteInformation("Preparing patches...");

            _distanceAssemblyDefinition = ModuleLoader.LoadDistanceModule(_distanceAssemblyFilename);

            if (!string.IsNullOrEmpty(_bootstrapAssemblyFilename))
            {
                _bootstrapAssemblyDefinition = ModuleLoader.LoadBootstrapModule(_bootstrapAssemblyFilename);
            }
            _patcher = new Patcher(_bootstrapAssemblyDefinition, _distanceAssemblyDefinition);

            _patcher.AddPatch(new SpectrumInitCodePatch());
            _patcher.AddPatch(new SpectrumUpdateCodePatch());
        }

        private static void RunPatches()
        {
            if (string.IsNullOrEmpty(_requestedPatchName))
            {
                ColoredOutput.WriteInformation("Running all patches...");
                _patcher.RunAll();
            }
            else
            {
                ColoredOutput.WriteInformation($"Running the requested patch: '{_requestedPatchName}'");
                _patcher.RunSpecific(_requestedPatchName);
            }
        }
    }
}
