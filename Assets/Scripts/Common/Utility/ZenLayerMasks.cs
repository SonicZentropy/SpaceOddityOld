// // /**
// //  * ZenLayerMasks.cs
// //  * Dylan Bailey
// //  * 20170214
// // */
namespace Common.Utility
{
    using Zen.Common.Extensions;

    public static class ZenLayerMasks
    {
        public static int AllInteractables  = 
            ZenUtils.LayerMaskFromIDs(SRLayerMask.npc, SRLayerMask.player, SRLayerMask.foreground);
        public static int NPCTargets =
            ZenUtils.LayerMaskFromIDs(SRLayerMask.npc, SRLayerMask.player);
    }
}