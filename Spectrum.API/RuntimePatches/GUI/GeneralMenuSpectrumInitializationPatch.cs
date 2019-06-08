using Harmony;
using Spectrum.API.GUI;

namespace Spectrum.API.RuntimePatches.GUI
{
    [HarmonyPatch(typeof(GeneralMenu), "InitializeVirtual")]
    internal class GeneralMenuSpectrumInitializationPatch
    {
        static void Postfix(GeneralMenu __instance)
        {
            MenuSystem.MenuBlueprint = __instance.menuBlueprint_;

            __instance.TweakAction("CONFIGURE SPECTRUM PLUGINS", () =>
            {
                MenuSystem.ShowMenu(MenuSystem.MenuTree, __instance, 0);
            });
        }
    }
}
