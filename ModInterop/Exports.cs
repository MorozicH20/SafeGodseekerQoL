using MonoMod.ModInterop;

using SafeGodseekerQoL.Modules.BossChallenge;
using SafeGodseekerQoL.Modules.QoL;
using System;

namespace SafeGodseekerQoL.ModInterop;

[ModExportName(nameof(SafeGodseekerQoL))]
public static class Exports
{
    public static int SuppressModules(string suppressor, params string[] modules)
    {
        try { return ModuleManager.SuppressModules(suppressor, modules); } catch (Exception ex) { Logger.Log(ex.Message); return 0; }
    }

    public static void CancelSuppression(int handle)
    {
        try
        {
            ModuleManager.CancelSuppression(handle);
        }
        catch (Exception ex) { Logger.Log(ex.Message); }
    }

    //public static void AddFastDashPredicate(Func<Scene, Scene, bool> predicate) =>
    //	FastDash.predicates.Add(predicate);

    public static void AddInfiniteChallengeReturnScenePredicate(Func<GameManager.SceneLoadInfo, bool> predicate)
    {
        try
        {
            InfiniteChallenge.returnScenePredicates.Add(predicate);
        }
        catch (Exception ex) { Logger.Log(ex.Message); }
    }
}
