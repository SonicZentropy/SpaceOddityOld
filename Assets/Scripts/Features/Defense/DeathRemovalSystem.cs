// /** 
//  * DeathRemovalSystem.cs
//  * Will Hart and Dylan Bailey
//  * 20161109
// */

namespace Zenobit.Systems
{
    #region Dependencies
    
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Zenobit.Common.ZenECS;
    using Zenobit.Components;

    #endregion

    /// <summary>
    ///     Removes dead units
    /// </summary>
    public class DeathRemovalSystem : AbstractEcsSystem
    {
		private static EcsEngine _staticEngine;

		protected static EcsEngine staticEngine
		{
			get
			{
				if (_staticEngine == null)
					_staticEngine = EcsEngine.Instance;
				return _staticEngine;
			}
		}

		public override void Update()
        {
            var casualties = new List<Entity>(
                engine.Get(ComponentTypes.HealthComp)
                    .Where(h => ((HealthComp)h).IsDead)
                    .Select(h => h.Owner));

            foreach (var destroyMe in casualties)
            {
                DestroyEntity(destroyMe);
            }
        }
        
        private static void DestroyEntity(Entity entity)
        {
            // destroy the entity
            foreach (var comp in entity.Components)
            {
                staticEngine.DestroyComponent(comp);
            }
			
            Object.Destroy(entity.Wrapper.gameObject);
        }
    }
}