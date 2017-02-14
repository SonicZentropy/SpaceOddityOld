// /**
//  * ComponentFactory.cs
//  * Dylan Bailey
//  * 20161210
// */

namespace Zen.Common.ZenECS
{
    #region Dependencies

    using System;
    using System.Collections.Generic;
    using Zen.Components;

    #endregion

    public static class ComponentFactory
    {
        public static readonly Dictionary<ComponentTypes, Type> ComponentLookup = new Dictionary<ComponentTypes, Type>(new FastEnumIntEqualityComparer<ComponentTypes>())
        {
			{ComponentTypes.AbstractActorComp, typeof(AbstractActorComp)},
			{ComponentTypes.AIActorComp, typeof(AIActorComp)},
			{ComponentTypes.PlayerComp, typeof(PlayerComp)},
			{ComponentTypes.PlayerShipComp, typeof(PlayerShipComp)},
			{ComponentTypes.AIShipComp, typeof(AIShipComp)},
			{ComponentTypes.ShipContextProviderComp, typeof(ShipContextProviderComp)},
			{ComponentTypes.AbstractCollisionComp, typeof(AbstractCollisionComp)},
			{ComponentTypes.DamageComp, typeof(DamageComp)},
			{ComponentTypes.HullComp, typeof(HullComp)},
			{ComponentTypes.ShieldComp, typeof(ShieldComp)},
			{ComponentTypes.DisableByDistanceComp, typeof(DisableByDistanceComp)},
			{ComponentTypes.ExplosionTriggerComp, typeof(ExplosionTriggerComp)},
			{ComponentTypes.FactionComp, typeof(FactionComp)},
			{ComponentTypes.GameSettingsComp, typeof(GameSettingsComp)},
			{ComponentTypes.CommandComp, typeof(CommandComp)},
			{ComponentTypes.MovementComp, typeof(MovementComp)},
			{ComponentTypes.PositionComp, typeof(PositionComp)},
			{ComponentTypes.SectorGenerationComp, typeof(SectorGenerationComp)},
			{ComponentTypes.ScannerComp, typeof(ScannerComp)},
			{ComponentTypes.ShipComp, typeof(ShipComp)},
			{ComponentTypes.ShipPrefabComp, typeof(ShipPrefabComp)},
			{ComponentTypes.AbstractModuleComp, typeof(AbstractModuleComp)},
			{ComponentTypes.InertialDamperModComp, typeof(InertialDamperModComp)},
			{ComponentTypes.TargetComp, typeof(TargetComp)},
			{ComponentTypes.TrackingCrosshairComp, typeof(TrackingCrosshairComp)},
			{ComponentTypes.AudioSourceComp, typeof(AudioSourceComp)},
			{ComponentTypes.CameraComp, typeof(CameraComp)},
			{ComponentTypes.ColliderComp, typeof(ColliderComp)},
			{ComponentTypes.CollisionEnterComp, typeof(CollisionEnterComp)},
			{ComponentTypes.CollisionExitComp, typeof(CollisionExitComp)},
			{ComponentTypes.LightComp, typeof(LightComp)},
			{ComponentTypes.LineRendererComp, typeof(LineRendererComp)},
			{ComponentTypes.MeshComp, typeof(MeshComp)},
			{ComponentTypes.ParticleSystemComp, typeof(ParticleSystemComp)},
			{ComponentTypes.RendererComp, typeof(RendererComp)},
			{ComponentTypes.RigidbodyComp, typeof(RigidbodyComp)},
			{ComponentTypes.TriggerEnterComp, typeof(TriggerEnterComp)},
			{ComponentTypes.TriggerExitComp, typeof(TriggerExitComp)},
			{ComponentTypes.UnityPrefabComp, typeof(UnityPrefabComp)},
			{ComponentTypes.TextMeshProComp, typeof(TextMeshProComp)},
			{ComponentTypes.UICameraComp, typeof(UICameraComp)},
			{ComponentTypes.UILabelComp, typeof(UILabelComp)},
			{ComponentTypes.UIPanelComp, typeof(UIPanelComp)},
			{ComponentTypes.UIRootComp, typeof(UIRootComp)},
			{ComponentTypes.UISpriteComp, typeof(UISpriteComp)},
			{ComponentTypes.UIWidgetComp, typeof(UIWidgetComp)},
			{ComponentTypes.ShipFittingsComp, typeof(ShipFittingsComp)},
			{ComponentTypes.WeaponComp, typeof(WeaponComp)},
			{ComponentTypes.LaserComp, typeof(LaserComp)},
			{ComponentTypes.LauncherComp, typeof(LauncherComp)},
			{ComponentTypes.LaunchedMissileComp, typeof(LaunchedMissileComp)},
			{ComponentTypes.MissileAreaDamageComp, typeof(MissileAreaDamageComp)},
			{ComponentTypes.MissileComp, typeof(MissileComp)}
		};

        public static ComponentEcs Create(ComponentTypes type)
        {
            if (!ComponentLookup.ContainsKey(type)) return null;
	        return ComponentCache.Instance.Get(type);
        }

	    public static ComponentEcs Instantiate(ComponentTypes type)
	    {
		    return (ComponentEcs) Activator.CreateInstance(ComponentLookup[type]);
	    }
    }

    public enum ComponentTypes
    {
		AbstractActorComp,
		AIActorComp,
		PlayerComp,
		PlayerShipComp,
		AIShipComp,
		ShipContextProviderComp,
		AbstractCollisionComp,
		DamageComp,
		HullComp,
		ShieldComp,
		DisableByDistanceComp,
		ExplosionTriggerComp,
		FactionComp,
		GameSettingsComp,
		CommandComp,
		MovementComp,
		PositionComp,
		SectorGenerationComp,
		ScannerComp,
		ShipComp,
		ShipPrefabComp,
		AbstractModuleComp,
		InertialDamperModComp,
		TargetComp,
		TrackingCrosshairComp,
		AudioSourceComp,
		CameraComp,
		ColliderComp,
		CollisionEnterComp,
		CollisionExitComp,
		LightComp,
		LineRendererComp,
		MeshComp,
		ParticleSystemComp,
		RendererComp,
		RigidbodyComp,
		TriggerEnterComp,
		TriggerExitComp,
		UnityPrefabComp,
		TextMeshProComp,
		UICameraComp,
		UILabelComp,
		UIPanelComp,
		UIRootComp,
		UISpriteComp,
		UIWidgetComp,
		ShipFittingsComp,
		WeaponComp,
		LaserComp,
		LauncherComp,
		LaunchedMissileComp,
		MissileAreaDamageComp,
		MissileComp
    }
}