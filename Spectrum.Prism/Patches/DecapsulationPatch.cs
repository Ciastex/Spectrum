using Mono.Cecil;
using Spectrum.Prism.IO;
using Spectrum.Prism.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Spectrum.Prism.Patches
{
    public class DecapsulationPatch : BasePatch
    {
        public override bool NeedsSource => false;
        public override string Name => "Decapsulation";

        public override void Run(ModuleDefinition moduleDefinition)
        {
            ColoredOutput.WriteInformation("Decapsulating non-public types and their members...");
            ColoredOutput.WriteInformation("WARNING: You will need to compile your plug-ins with\n           unsafe code allowed if you're going to publish them.");

            var assemblyTypes = ScanTypes(moduleDefinition);
            DecapsulateMembers(assemblyTypes);
        }

        private List<TypeDefinition> ScanTypes(ModuleDefinition moduleDefinition)
        {
            List<TypeDefinition> recurseNested(IList<TypeDefinition> types)
            {
                if (types == null || types.Count == 0)
                    return new List<TypeDefinition>();

                return types.Concat(recurseNested(types.SelectMany(x => x.NestedTypes).ToList())).ToList();
            }

            return recurseNested(moduleDefinition.Types.ToList());
        }

        private void DecapsulateMembers(List<TypeDefinition> types)
        {
            types.ForEach(t =>
            {
                if (t == null) return;

                if (!t.IsPublic && !t.IsNestedPublic)
                {
                    if (t.IsNested)
                        t.IsNestedPublic = true;
                    else t.IsPublic = true;
                }

                if (t.HasMethods)
                {
                    foreach (var method in t.Methods)
                        method.IsPublic = true;
                }

                if (t.HasFields)
                {
                    foreach (var field in t.Fields)
                        field.IsPublic = true;
                }
            });
        }
    }
}
