// /** 
// * PlayerInitSystem.cs
// * Dylan Bailey
// * 20161223
// */

namespace Zen.Systems
{
	#region Dependencies

	using Common;
	using Common.ZenECS;
	using Components;
	using UnityEngine;

	#endregion

	public class PlayerInitSystem : AbstractEcsSystem
	{
		private Entity player;
		private CameraComp cc;
		public override bool Init()
		{
			player = engine.FindEntity(Res.Entities.Player);
			cc = engine.GetSingle<CameraComp>(ComponentTypes.CameraComp);
			cc.TargetToFollow = player.GetComponent<PositionComp>().transform;
			cc.StartingPositionOffset = player.GetComponent<ShipPrefabComp>().FirstPersonCameraOffset;
			//CreateShipEntity();

			return false;
		}

		public void CreateShipEntity()
		{
			var shipToCreate = player.GetComponent<PlayerComp>().CurrentShip;

			GameObject ship = Object.Instantiate(Resources.Load<GameObject>(shipToCreate));
			ship.transform.SetParent(player.Wrapper.transform);
			cc.StartingPositionOffset = player.GetComponent<ShipPrefabComp>().FirstPersonCameraOffset;


			//Entity ship = engine.CreateEntity(shipToCreate);
			//player.GetComponent<ShipConnectionComp>().Ship = ship.GetComponent<ShipComp>();
			//ship.Wrapper.transform.SetParent(player.Wrapper.transform, false);
			//ship.GetComponent<ShipComp>().OwningActor = player.GetComponent<PlayerComp>();
			//ship.AddComponent(ComponentTypes.PlayerShipComp);

		}
	}
}