// // /**
// //  * AIStates.cs
// //  * Dylan Bailey
// //  * 20170206
// // */
namespace Zen.AI.Common
{
    public enum EAINavState
    {
        IDLE,
        PURSUE,
        FLEE,
        ORBIT,
        APPROACH,
        ATTACKING,
        AVOIDANCE,
        GET_BEHIND,
        CHASELONG,
        SQUIGGLE,
        GUARD,
        WAYPOINTS,
        DOCK,
        BIGSHIP,
        PATH,
        EVADE_WEAPON,
        STRAFE,
        PLAYDEAD,
        BAYEMERGE,
        BAYDEPART,
        WARPOUT,
        SENTRYGUN
    }

    public enum EAIATTACK_SUBMODE
    {
        GLIDE_IN_RANGE, // When in attack range, glide and fire
        BEHIND_ONLY, // Always navigate to rear, then attack
        NAV_FOCUS_FIRST // Focus on steering first, only fire if it's conveniently in front of you
    }

    public enum EAICHASE_SUBMODE
    {
        NONE,
        CONT_TURN,
        ATTACK,
        E_SQUIG,
        E_BRAKE,
        EVADE,
        SUP_ATTAK,
        AVOID,
        BEHIND,
        GET_AWAY,
        E_WEAPON,
        FLY_AWAY,
        ATK_4EVER,
        STLTH_FND,
        STLTH_SWP,
        BIG_APPR,
        BIG_CIRC,
        BIG_PARL
    }

    public enum EAISTRAFE_SUBMODE
    {
        NONE,
        ATTACK,
        AVOID,
        RETREAT1,
        RETREAT2,
        POSITION
    }
}