using UnityEngine;

namespace Zenobit.Common
{
	public static class Res
	{
		public static class Entities
		{
			public const string Camera = "Core/Camera";
			public const string GameSettings = "Core/GameSettings";
			public const string BaseShip = "Entities/BaseShip";
			public const string Enemy = "Entities/Enemy";
			public const string EnemyTestShip = "Entities/EnemyTestShip";
			public const string Player = "Entities/Player";
			public const string TestEnt = "Entities/TestEnt";
			public const string SectorGenerationMain = "Sector/SectorGenerationMain";
			public const string DumbfireMissile = "Weapons/DumbfireMissile";
			public const string HomingMissile = "Weapons/HomingMissile";
			public const string SwarmMissile = "Weapons/SwarmMissile";
		}
		
		public static class Prefabs
		{
			public const string EnemyShip = "EnemyShip";
			public const string Player = "Player";
			public const string CameraMain = "Core/CameraMain";
			public const string Explosion_007_example = "Effects/Explosions/Explosion_007_example";
			public const string Explosion_01 = "Effects/Explosions/Explosion_01";
			public const string Explosion_02 = "Effects/Explosions/Explosion_02";
			public const string Explosion_03 = "Effects/Explosions/Explosion_03";
			public const string flames_flame_blue = "Effects/Flames/flames_flame_blue";
			public const string flames_flame_green = "Effects/Flames/flames_flame_green";
			public const string flames_flame_red = "Effects/Flames/flames_flame_red";
			public const string trail_002 = "Effects/RedParticleTrails/trail_002";
			public const string trail_007 = "Effects/RedParticleTrails/trail_007";
			public const string trail_014 = "Effects/RedParticleTrails/trail_014";
			public const string trail_017 = "Effects/RedParticleTrails/trail_017";
			public const string Asteroid = "Environment/Asteroid";
			public const string StarContainer = "Environment/StarContainer";
			public const string UIRoot3D = "GUI/UI Root (3D)";
			public const string UIRoot = "GUI/UIRoot";
			public const string BaseFighter = "Ships/BaseFighter";
			public const string BaseShip = "Ships/BaseShip";
			public const string Frigate = "Ships/Frigate";
			public const string MK6_Strike_Drone = "Ships/MK6_Strike_Drone";
			public const string Ship_Corvette = "Ships/Ship_Corvette";
			public const string DumbfireMissile = "Weapons/DumbfireMissile";
			public const string HomingMissile = "Weapons/HomingMissile";
			public const string LaserBeamPrefab = "Weapons/LaserBeamPrefab";
			public const string LaserBeamPrefabLR = "Weapons/LaserBeamPrefabLR";
			public const string laser_impulse_projectile_002 = "Weapons/laser_impulse_projectile_002";
			public const string PIDMissile = "Weapons/PIDMissile";
			public const string plasma_gun_bolt = "Weapons/plasma_gun_bolt";
			public const string plasma_gun_projectile_001 = "Weapons/plasma_gun_projectile_001";
			public const string plasma_gun_projectile_002 = "Weapons/plasma_gun_projectile_002";
			public const string SwarmMissile = "Weapons/SwarmMissile";
		}
			
		public static GameObject Load(string PrefabToLoad)
		{
			return Resources.Load<GameObject>(PrefabToLoad);
		}

		public static GameObject Instantiate(string PrefabToCreate)
		{
			return Object.Instantiate(Load(PrefabToCreate));
		}
	}
}