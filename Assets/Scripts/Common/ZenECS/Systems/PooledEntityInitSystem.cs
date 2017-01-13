// /**
// * PositionUpdateSystem.cs
// * Will Hart and Dylan Bailey
// * 20161211
// */

namespace Zenobit.Systems
{
    #region Dependencies

	using Common.ZenECS;
	using Common.ObjectPool;

	#endregion

    public class PooledEntityInitSystem : AbstractEcsSystem
	{
		public override bool Init()
		{
            engine.OnEntityAdded += InitialisePooledObject;
            engine.OnEntityRemoved += RemovePooledObject;
			return false;
		}

        private void RemovePooledObject(Entity entity)
        {
            var init = GetInitComponent(entity);
            if (init == null) return;

            init.RemoveEntityFromPool();
        }

        private static void InitialisePooledObject(Entity entity)
        {
            var init = GetInitComponent(entity);
            if (init == null) return;

            init.InitEntityFromPool();
        }

        private static IPoolEntityInit GetInitComponent(Entity entity)
        {
            var wrapper = entity.Wrapper;
            if (wrapper == null) return null;

            var init = wrapper.gameObject.GetComponent<IPoolEntityInit>();
            if (init == null)
                init = wrapper.gameObject.GetComponentInChildren<IPoolEntityInit>();
            if (init == null)
                init = wrapper.gameObject.GetComponentInParent<IPoolEntityInit>();

            return init;
        }
    }
}