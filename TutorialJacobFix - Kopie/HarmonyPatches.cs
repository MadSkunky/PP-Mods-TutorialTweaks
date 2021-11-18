using Harmony;
using Base.Build;
using PhoenixPoint.Home.View.ViewModules;

namespace MadSkunky.TutorialTweaks
{
    class HarmonyPatches
    {
        // This "tag" allows Harmony to find this class and apply it as a patch.
        [HarmonyPatch(typeof(UIModuleBuildRevision), "SetRevisionNumber")]
        // Class can be any name, but must be static.
        internal static class UIModuleBuildRevision_SetRevisionNumber
        {

            // Rewrite to use configured value. User may set it to null.
            private static void Postfix(UIModuleBuildRevision __instance)
            {
                __instance.BuildRevisionNumber.text = $"{RuntimeBuildInfo.UserVersion} w/MSTT v{TutorialTweaks.ModVersion} ";
            }
        }
    }
}
