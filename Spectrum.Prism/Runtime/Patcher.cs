using System.Collections.Generic;
using Mono.Cecil;
using Spectrum.Prism.Enums;
using Spectrum.Prism.IO;

namespace Spectrum.Prism.Runtime
{
    public class Patcher
    {
        private ModuleDefinition SourceModule { get; }
        private ModuleDefinition TargetModule { get; }

        private List<IPatch> Patches { get; }

        public Patcher(ModuleDefinition sourceModule, ModuleDefinition targetModule)
        {
            SourceModule = sourceModule;
            TargetModule = targetModule;

            Patches = new List<IPatch>();
        }

        public void AddPatch(IPatch patch)
        {
            if (patch == null)
                return;

            patch.PatchFailed += (sender, args) =>
            {
                ErrorHandler.TerminateWithError($"Patch '{args.Name}' failed. Reason: {args.Exception.Message}", TerminationReason.PatchFailure);
            };

            patch.PatchSucceeded += (sender, args) =>
            {
                ColoredOutput.WriteSuccess($"Patch '{args.Name}' succeeded.");
            };

            Patches.Add(patch);
        }

        public void RunAll()
        {
            foreach (var patch in Patches)
            {
                if (SourceModule == null && patch.NeedsSource)
                {
                    ColoredOutput.WriteInformation($"Skipping '{patch.Name}' because no source module was provided.");
                    continue;
                }
                RunPatch(patch);
            }
        }

        public void RunSpecific(string name)
        {
            foreach (var patch in Patches)
            {
                if (patch.Name == name)
                {
                    RunPatch(patch);
                    return;
                }
            }
            ErrorHandler.TerminateWithError($"No patch '{name}' exists.", TerminationReason.PatchNotFound);
        }

        private void RunPatch(IPatch patch)
        {
            if (patch.NeedsSource)
            {
                patch.Run(SourceModule, TargetModule);
            }
            else
            {
                patch.Run(TargetModule);
            }
        }
    }
}
