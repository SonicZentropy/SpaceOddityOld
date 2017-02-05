namespace Zen.Common.ZenECS
{
	#region Dependencies

	using System.Collections.Generic;
	using FullSerializer;
	using UnityEngine;
	using System;
	using System.Linq;
	using Editor.Utils;

	#endregion

	public class EntityFactory
	{
		private readonly fsSerializer _serializer = new fsSerializer();
		
		private readonly Dictionary<string, fsData> _entityTemplates = new Dictionary<string, fsData>();

		private EcsEngine _engine;

		protected EcsEngine engine
		{
			get { return _engine ?? (_engine = EcsEngine.Instance); }
		}

		public EntityFactory()
		{
			//engine = EcsEngine.Instance;
			PreparseJson();
		}

		private void PreparseJson()
		{
			var resourcePaths = UnityDrawerStatics.EntityList.Select(s => "Entities/" + s).ToArray();

			foreach (var filePath in resourcePaths)
			{
				var asset = Resources.Load<TextAsset>(filePath);
                
				try
				{
					var fsdata = fsJsonParser.Parse(asset.text);

					var fsdict = fsdata.AsDictionary;
					if (fsdict.ContainsKey("EntityName"))
					{
						var entname = fsdict["EntityName"]?.AsString ?? "Default";
						_entityTemplates.Add(entname, fsdata);
					}
				}
				catch (Exception)
				{
					continue;
				}
			}
		}

		public Entity CreateEntityFromTemplate(string entityName)
		{
			object deserialized = null;
			entityName = FileOps.GetStringAfterLastSlash(entityName);

			_serializer.TryDeserialize(
				_entityTemplates[entityName],
				typeof(Entity),
				ref deserialized
				);

			Entity newEntity = (Entity) deserialized;
			InitializeNewEntity(newEntity);
			engine?.EntityList.Add(newEntity);

            engine?.TriggerEntityAdded(newEntity);
			return newEntity;
		}

		public fsData GetTemplate(string entityName)
		{
			return !_entityTemplates.ContainsKey(entityName) ? null : _entityTemplates[entityName];
		}

		private void InitializeNewEntity(Entity ent)
		{
			foreach (var cmp in ent.Components)
			{
				cmp.Initialise(engine, ent);
                engine.AddComponent(cmp);
			}

			if (ent.HasComponent(ComponentTypes.UnityPrefabComp))
				LinkedGameObjectFactory.Instance.CreateGameObjectForEntity(ent);

			foreach (var cmp in ent.Components)
			{
				cmp.InitialiseLate(engine, ent);
			}
		}
	}
}