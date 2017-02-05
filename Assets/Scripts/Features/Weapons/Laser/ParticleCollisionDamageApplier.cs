namespace Features.Weapons.Laser
{
	using UnityEngine;
	using Zen.Common;
	using Features.Explosions;
	using Zen.Common.Extensions;
	using Zen.Common.ZenECS;
	using Zen.Components;

	public class ParticleCollisionDamageApplier : MonoBehaviour
	{
	    private DamagePacket particleDmgPacket;
	    private bool NeedsDamageInit = true;

	    private void OnEnable()
	    {
	        NeedsDamageInit = true;
	    }

		private void OnParticleCollision(GameObject other)
		{
		    if (NeedsDamageInit)
		    {
		        NeedsDamageInit = false;
                BuildDamagePacket();
		    }
			//Debug.Log($"Creating explosion at: {other.transform.position}");

			//Debug.Log($"Laser hit {go.name}");
			if (other.HasEntityTag(EntityTags.IsDamageable))
			{
				//Debug.Log($"Adding dmg component to ship");
				//#TODO: Change damage packet to match weapon info
				other.GetEntity().GetComponent<DamageComp>().damagePackets.Push(particleDmgPacket);
			}
			Explosions.Create(Res.Prefabs.ExplosionPFX_Blue_Local, other.transform.position);
			//Debug.Break();
		}

	    private void BuildDamagePacket()
	    {
            Entity e = gameObject.GetComponentDownThenUp<EntityWrapper>().Entity;

            foreach (var fit in e.GetComponent<ShipFittingsComp>().fittingList)
            {
                if (gameObject == fit.FittedWeapon.WeaponGameObject)
                {
                    Debug.Log($"Found proper fitted weapon, copying dmg packet");
                    particleDmgPacket = new DamagePacket(fit.FittedWeapon.HullDamage, fit.FittedWeapon.ShieldDamage);
                    break;
                }
            }
        }
	}
}