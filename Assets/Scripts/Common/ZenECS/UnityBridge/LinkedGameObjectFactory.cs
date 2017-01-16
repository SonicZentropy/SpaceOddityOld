// /**
//  * LinkedGameObjectFactory.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Common.ZenECS
{
    #region Dependencies

    using System;
    using TMPro;
    using UnityEngine;
    using Weapons;
    using Zenobit.Common.Debug;
    using Zenobit.Common.Extensions;
    using Zenobit.Common.ObjectPool;
    using Zenobit.Components;

    #endregion

    public class LinkedGameObjectFactory : MonoSingleton<LinkedGameObjectFactory>
    {
        private static void InitializeGameObject(GameObject go)
        {
            var ew = go.GetComponent<EntityWrapper>();

            if (ew == null)
            {
                ZenLogger.LogError("No entity wrapper on game object to be initialized");
                return;
            }

            //Sync and inject GO transform
            if (!ew.Entity.HasComponent(ComponentTypes.PositionComp)) return;

            var pc = ew.Entity.GetComponent<PositionComp>();
            go.transform.position = pc.Position;
            go.transform.rotation = pc.Rotation;
            pc.transform = go.transform;
        }

        // Inject all unity components from the GO prefab into the associated entity components
        private static void InjectGameObjectReferences(Entity e, GameObject go)
        {
            if (e.HasComponent(ComponentTypes.AudioSourceComp))
                e.GetComponent<AudioSourceComp>().AudioSource = go.GetComponentInChildren<AudioSource>();
            if (e.HasComponent(ComponentTypes.CameraComp))
                e.GetComponent<CameraComp>().MainCamera = go.GetComponentInChildren<Camera>();
            if (e.HasComponent(ComponentTypes.ColliderComp))
                e.GetComponent<ColliderComp>().collider = go.GetComponentInChildren<Collider>();
            if (e.HasComponent(ComponentTypes.LightComp))
                e.GetComponent<LightComp>().Light = go.GetComponentInChildren<Light>();
            if (e.HasComponent(ComponentTypes.LineRendererComp))
                e.GetComponent<LineRendererComp>().LineRenderer = go.GetComponentInChildren<LineRenderer>();
	        if (e.HasComponent(ComponentTypes.RendererComp))
		        e.GetComponent<RendererComp>().renderer = go.GetComponentInChildren<Renderer>();

	        if (e.HasComponent(ComponentTypes.MeshComp))
            {
                e.GetComponent<MeshComp>().MeshFilter = go.GetComponentInChildren<MeshFilter>();
                e.GetComponent<MeshComp>().MeshRenderer = go.GetComponentInChildren<MeshRenderer>();
            }
            if (e.HasComponent(ComponentTypes.ParticleSystemComp))
                e.GetComponent<ParticleSystemComp>().ParticleSystem = go.GetComponentInChildren<ParticleSystem>();
            if (e.HasComponent(ComponentTypes.RigidbodyComp))
                e.GetComponent<RigidbodyComp>().Rigidbody = go.GetComponentInChildren<Rigidbody>();
            if (e.HasComponent(ComponentTypes.TextMeshProComp))
                e.GetComponent<TextMeshProComp>().TextMesh = go.GetComponentInChildren<TextMeshPro>();
            if (e.HasComponent(ComponentTypes.UICameraComp))
                e.GetComponent<UICameraComp>().UICamera = go.GetComponentInChildren<UICamera>();
            if (e.HasComponent(ComponentTypes.UILabelComp))
                e.GetComponent<UILabelComp>().UILabel = go.GetComponentInChildren<UILabel>();
            if (e.HasComponent(ComponentTypes.UIPanelComp))
                e.GetComponent<UIPanelComp>().UIPanel = go.GetComponentInChildren<UIPanel>();
            if (e.HasComponent(ComponentTypes.UIRootComp))
                e.GetComponent<UIRootComp>().UIRoot = go.GetComponentInChildren<UIRoot>();
            if (e.HasComponent(ComponentTypes.UISpriteComp))
                e.GetComponent<UISpriteComp>().UISprite = go.GetComponentInChildren<UISprite>();
            if (e.HasComponent(ComponentTypes.UIWidgetComp))
                e.GetComponent<UIWidgetComp>().UIWidget = go.GetComponentInChildren<UIWidget>();
        }

        //Allows custom monobehaviors to initialize desired things via interface
        private static void PerformCustomInitializations(Entity e, GameObject go)
        {
            var icis = go.GetComponentsInChildren<ICustomInit>(true);
            foreach (var ici in icis)
            {
                ici.ExecuteInitialization(e, go);
            }
        }

        public GameObject CreateGameObjectForEntity(Entity e, Transform parent = null)
        {
            if (!e.HasComponent(ComponentTypes.UnityPrefabComp))
            {
                throw new ArgumentException("Entity does not have a prefab comp.");
            }

            var pc = e.GetComponent<UnityPrefabComp>();
			//string prefabString
            var go = pc.IsPooled
                ? Resources.Load<GameObject>(pc.PrefabLink).InstantiateFromPool()
                : Instantiate(Resources.Load<GameObject>(pc.PrefabLink));

            var ew = go.GetComponent<EntityWrapper>();

            if (go.GetComponent<EntityWrapper>() == null)
                ew = go.AddComponent<EntityWrapper>();
            else
            {
                ew.Entity = null;
            }

            ew.Entity = e;
            e.Wrapper = ew;

            if (parent != null)
                go.transform.SetParent(parent);

            InitializeGameObject(go);
            InjectGameObjectReferences(e, go);
            PerformCustomInitializations(e, go);
            return go;
        }
    }
}