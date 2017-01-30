namespace Zen.Components
{
    #region Dependencies

    using Zen.Common.ZenECS;

    #endregion

    public class UnitPropertiesComp : ComponentEcs
    {
        public int SideId { get; set; }

        public UnitPropertiesComp() : base()
        {
        }

        public override ComponentTypes ComponentType => ComponentTypes.UnitPropertiesComp;
    }
}