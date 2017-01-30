using UnityEngine;

namespace Zen.Common
{
    using System.Collections.Generic;
	using Zen.Common.ObjectPool;

	public static class Res
	{
		public static class Entities
		{
			public const string BaseShip = "Actors/BaseShip";
			public const string Enemy = "Actors/Enemy";
			public const string EnemyOld = "Actors/EnemyOld";
			public const string EnemyTestShip = "Actors/EnemyTestShip";
			public const string Player = "Actors/Player";
			public const string TestEnt = "Actors/TestEnt";
			public const string Camera = "Core/Camera";
			public const string GameSettings = "Core/GameSettings";
			public const string SectorGenerationMain = "Sector/SectorGenerationMain";
			public const string DumbfireMissile = "Weapons/DumbfireMissile";
			public const string HomingMissile = "Weapons/HomingMissile";
			public const string SwarmMissile = "Weapons/SwarmMissile";
			public const string MissileLauncher1 = "Weapons/Launchers/MissileLauncher1";
		}
		
		public static class Prefabs
		{
			public const string EmpyGO = "Prefabs/EmpyGO";
			public const string EnemyShip = "Prefabs/EnemyShip";
			public const string Player = "Prefabs/Player";
			public const string ShieldPFXObj = "Prefabs/ShieldPFXObj";
			public const string CameraMain = "Prefabs/Core/CameraMain";
			public const string ExplosionPFX_Blue = "Prefabs/Effects/Explosions/ExplosionPFX_Blue";
			public const string Explosion_007_example = "Prefabs/Effects/Explosions/Explosion_007_example";
			public const string Explosion_01 = "Prefabs/Effects/Explosions/Explosion_01";
			public const string Explosion_02 = "Prefabs/Effects/Explosions/Explosion_02";
			public const string Explosion_03 = "Prefabs/Effects/Explosions/Explosion_03";
			public const string Explosion_Plasma_Blue = "Prefabs/Effects/Explosions/Explosion_Plasma_Blue";
			public const string flames_flame_blue = "Prefabs/Effects/Flames/flames_flame_blue";
			public const string flames_flame_green = "Prefabs/Effects/Flames/flames_flame_green";
			public const string flames_flame_red = "Prefabs/Effects/Flames/flames_flame_red";
			public const string trail_002 = "Prefabs/Effects/RedParticleTrails/trail_002";
			public const string trail_007 = "Prefabs/Effects/RedParticleTrails/trail_007";
			public const string trail_014 = "Prefabs/Effects/RedParticleTrails/trail_014";
			public const string trail_017 = "Prefabs/Effects/RedParticleTrails/trail_017";
			public const string ShieldSmallShell = "Prefabs/Effects/Shields/ShieldSmallShell";
			public const string VibrantShieldSmall = "Prefabs/Effects/Shields/VibrantShieldSmall";
			public const string Asteroid = "Prefabs/Environment/Asteroid";
			public const string StarContainer = "Prefabs/Environment/StarContainer";
			public const string UIRoot3D = "Prefabs/GUI/UI Root (3D)";
			public const string UIRoot = "Prefabs/GUI/UIRoot";
			public const string BaseFighter = "Prefabs/Ships/BaseFighter";
			public const string BaseShip = "Prefabs/Ships/BaseShip";
			public const string Frigate = "Prefabs/Ships/Frigate";
			public const string MK6_Strike_Drone = "Prefabs/Ships/MK6_Strike_Drone";
			public const string Ship_Corvette = "Prefabs/Ships/Ship_Corvette";
			public const string MissileLauncher1 = "Prefabs/Weapons/Launchers/MissileLauncher1";
			public const string DumbfireMissile = "Prefabs/Weapons/Projectiles/DumbfireMissile";
			public const string HomingMissile = "Prefabs/Weapons/Projectiles/HomingMissile";
			public const string LaserBeamPrefab = "Prefabs/Weapons/Projectiles/LaserBeamPrefab";
			public const string LaserBeamPrefabLR = "Prefabs/Weapons/Projectiles/LaserBeamPrefabLR";
			public const string laser_impulse_projectile_002 = "Prefabs/Weapons/Projectiles/laser_impulse_projectile_002";
			public const string PIDMissile = "Prefabs/Weapons/Projectiles/PIDMissile";
			public const string plasma_gun_bolt = "Prefabs/Weapons/Projectiles/plasma_gun_bolt";
			public const string plasma_gun_projectile_001 = "Prefabs/Weapons/Projectiles/plasma_gun_projectile_001";
			public const string plasma_gun_projectile_002 = "Prefabs/Weapons/Projectiles/plasma_gun_projectile_002";
			public const string SwarmMissile = "Prefabs/Weapons/Projectiles/SwarmMissile";
		}
			
		private static Dictionary<string, GameObject> LoadCache = new Dictionary<string,GameObject>();

		public static GameObject Load(string PrefabToLoad)
		{
			GameObject go;
			if (!LoadCache.TryGetValue(PrefabToLoad, out go))
			{
				go = Resources.Load<GameObject>(PrefabToLoad);
				LoadCache.Add(PrefabToLoad, go);
			}
			return go;
		}

		public static GameObject Instantiate(string PrefabToCreate)
		{
			return Object.Instantiate(Load(PrefabToCreate));
		}

		public static GameObject CreateFromPool(string PrefabToCreate)
		{
			return Load(PrefabToCreate).InstantiateFromPool();
		}
	}
}