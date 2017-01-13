// /** 
//  * HealthComp.cs
//  * Dylan Bailey
//  * 20161103
// */

namespace Zenobit.Components
{
    #region Dependencies

    using Zenobit.Common.ZenECS;

    #endregion

    public class HealthComp : ComponentEcs
    {
        public Reactive<float> MaxHealth { get; set; }
        public Reactive<float> CurrentHealth { get; set; }
        public float RegenRate { get; set; }
        public int ArmorValue { get; set; }

        public bool IsDead => CurrentHealth <= 0;

        public HealthComp() : base()
        {
            MaxHealth.ValueUpdated += ReactToChanges;
            CurrentHealth.ValueUpdated += ReactToChanges;
        }

        private void ReactToChanges(Reactive<float> reactive)
        {
            TriggerComponentUpdated(this);
        }

        public override ComponentTypes ComponentType => ComponentTypes.HealthComp;
    }
}