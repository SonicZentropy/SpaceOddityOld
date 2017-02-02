//namespace Zen.Components
//{
//	using Zen.Common.ZenECS;
//
//	public static class EntityHelper
//	{
//		public static Entity GetShipEntity(Entity inEntity)
//		{
//			if (inEntity.HasComponent(ComponentTypes.ShipComp))
//			{
//				return inEntity;
//			}
//			else if (inEntity.HasComponent(ComponentTypes.ShipConnectionComp))
//			{
//				return inEntity.GetComponent<ShipConnectionComp>().Ship.Owner;
//			}
//			else
//			{
//				Debug.LogError($"Called get ship entity on {inEntity.EntityName} that is neither actor nor ship");
//				return null;
//			}
//		}
//
//		public static Entity GetShipActorEntity(Entity inEntity)
//		{
//			if (inEntity.HasComponent(ComponentTypes.AbstractActorComp))
//			{
//				return inEntity;
//			}
//			else if (inEntity.HasComponent(ComponentTypes.ShipComp))
//			{
//				return inEntity.GetComponent<ShipComp>().OwningActor.Owner;
//			}
//			else
//			{
//				Debug.LogError($"Called getshipactor on {inEntity.EntityName} which has no actor or ship comp");
//				return null;
//			}
//
//		}
//	}
//}