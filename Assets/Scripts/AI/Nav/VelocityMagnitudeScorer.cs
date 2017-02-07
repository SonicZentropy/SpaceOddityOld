// /** 
//  * VelocityMagnitudeScorer.cs
//  * Dylan Bailey
//  * 20170206
// */

namespace Zen.AI.Apex.Scorers
{
    #region Dependencies

    using global::AI.Core;
    using global::Apex.AI;
    using global::Apex.Serialization;
    using Zen.AI.Apex.Contexts;

    #endregion

    public sealed class VelocityMagnitudeScorer : ZenContextualScorer<ShipContext>
    {
        [ApexSerialization(defaultValue = 10f),
         FriendlyName("VelocityThreshold", "Velocity magnitude ship must be above")]
        public float VelocityThreshold;
        
        public override float Score(ShipContext context)
        {
            if (context.rbComp.velocity.sqrMagnitude >= VelocityThreshold * VelocityThreshold)    
                return Success;
            return Failure;
        }
    }
} 
