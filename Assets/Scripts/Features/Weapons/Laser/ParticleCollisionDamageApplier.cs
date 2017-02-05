namespace Features.Weapons.Laser
{
	using UnityEngine;
	using Zen.Common;
	using Features.Explosions;
	using Zen.Common.Extensions;
	using Zen.Components;

	public class ParticleCollisionDamageApplier : MonoBehaviour
	{
		private void OnParticleCollision(GameObject other)
		{
			//Debug.Log($"Creating explosion at: {other.transform.position}");

			//Debug.Log($"Laser hit {go.name}");
			if (other.HasEntityTag(EntityTags.IsDamageable))
			{
				//Debug.Log($"Adding dmg component to ship");
				//todo:Change damage packet to match weapon info
				other.GetEntity().GetComponent<DamageComp>().damagePackets.Push(new DamagePacket(10, 10));
			}
			Explosions.Create(Res.Prefabs.ExplosionPFX_Blue_Local, other.transform.position);
			//Debug.Break();
		}
	}
}