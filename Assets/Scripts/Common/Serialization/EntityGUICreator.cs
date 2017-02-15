// /**
//  * EntityGUICreator.cs
//  * Dylan Bailey
//  * 20161209
// */
#pragma warning disable 67
namespace Zen.Serialization
{
	#region Dependencies

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using AdvancedInspector;
	using Common.Extensions;
	using FullInspector;
	using FullSerializer;
	using UnityEditor;
	using UnityEngine;
	using Zen.Common.ZenECS;
	using Zen.Editor.Utils;

    #endregion

	[ExecuteInEditMode]
	[AdvancedInspector(true, false)]
	public class EntityGUICreator : MonoSingleton<EntityGUICreator>,/*MonoBehaviour,*/ IDataChanged
	{
		private static readonly fsSerializer Serializer = new fsSerializer();
		private static fsData _data;

		private string _entityLoad;
		private static Vector3 colorDirty = new Vector3(1, 0, 0);

	    [Inspect(11000)]
	    private bool ShouldPoolEntity;

		[HideInInspector]
		public bool AvoidDirtyFlag;
		private bool isDirty;
		public bool IsDirty
		{
			get
			{
				return isDirty;
			}
			set { isDirty = value; }
		}

		private bool GetIsDirty()
		{
			return IsDirty;
		}

		//[Space]
		//[Descriptor(":", "Components On Entity")]
		//[Inspect(5000)]
		[NonSerialized]
		//[Expandable(Expanded = true)]
		public readonly Dictionary<ComponentTypes, List<ComponentEcs>>
			ComponentPools =
				new Dictionary<ComponentTypes, List<ComponentEcs>>(Enum.GetNames(typeof(ComponentTypes)).Length);

		#if UNITY_EDITOR

		[Inspect(10000), NonSerialized, Expandable(Expanded = true), Descriptor("Components", "Components on Entity")]
		public Dictionary<string, List<ComponentEcs>> GroupedComponents = new Dictionary<string, List<ComponentEcs>>();

		#endif

		[Inspect(60)]
		public string EntityName;
		private Entity _newEnt = new Entity();

		private ComponentTypes TypeToCreate
		{
			get
			{
				try
				{
					int pos = ComponentToAdd.LastIndexOf("/") + 1;
					string compname = ComponentToAdd.Substring(pos, ComponentToAdd.Length - pos);
					ComponentTypes ct = (ComponentTypes)Enum.Parse(typeof(ComponentTypes), compname);
					return ct;
				}
				catch (ArgumentException)
				{
					Console.WriteLine($"'{ComponentToAdd}' is not a member of the ComponentTypes enumeration.");
				}
				return ComponentTypes.PositionComp;
			}
		}

		private string _componentToAdd;

		[TextField(TextFieldType.Component)]
		[Inspect(4000)]
		public string ComponentToAdd
		{
			get { return _componentToAdd; }
			set
			{
				_componentToAdd = value;
				AvoidDirtyFlag = true;
			}
		}

		[TextField(TextFieldType.Entity)]
		[Inspect(90)]
		public string EntityLoad
		{
			get { return _entityLoad; }
			set
			{
				Reset();
				_entityLoad = Application.dataPath + "/Resources/Entities/" + value + ".json";

				try
				{
					LoadJson(_entityLoad);
				}
				catch (Exception e)
				{
					Debug.LogError($"Can't load entity: {e.Message}");
					Reset();
				}


				_entityLoad = value;
				EntityName = value;
				AvoidDirtyFlag = true;
			}
		}

		[Inspect(250)]
		[Method]
		public void Reset()
		{
			ComponentPools.Clear();
#if UNITY_EDITOR

			GroupedComponents.Clear();

#endif
		_newEnt = new Entity();
			EntityName = "";
			UnityDrawerStatics.RefreshPrefabList();
			UnityDrawerStatics.RefreshEntityList();
			UnityDrawerStatics.RefreshComponentList();
			//FindAllComponents();
			//OnDataChanged?.Invoke();
			IsDirty = false;
			AvoidDirtyFlag = true;
		}

		public void DataChanged()
		{
			if (!AvoidDirtyFlag)
			{
				IsDirty = true;
			}
			AvoidDirtyFlag = false;
		}

		public event GenericEventHandler OnDataChanged;

		[Inspect(4500)]
		[Method]
		public void AddChosenComponent()
		{
			if (!ComponentPools.ContainsKey(TypeToCreate))
			{
				ComponentPools[TypeToCreate] = new List<ComponentEcs>();
			}
			var comp = (ComponentEcs) Activator.CreateInstance(ComponentFactory.ComponentLookup[TypeToCreate]);
			ComponentPools[TypeToCreate].Add(comp);

#if UNITY_EDITOR
			if (!GroupedComponents.ContainsKey(comp.Grouping))
			{
				GroupedComponents[comp.Grouping] = new List<ComponentEcs>();
			}
			GroupedComponents[comp.Grouping].Add(comp);
#endif

			OnDataChanged?.Invoke();
			IsDirty = true;
		}

		[Inspect("GetIsDirty", 100)]
		[Method]
		[Descriptor("Save Entity", "Save Entity To JSON In Resources folder", "", 1, 0.5f, 0.5f)]
		public void SaveEntityToJson()
		{
			_newEnt = new Entity(FileOps.GetEntityNameFromFullName(EntityName));
		    //_newEnt = new Entity(EntityName);
			Debug.Log($"Saving entity: {EntityName}");
			foreach (var typ in ComponentPools)
			{
				foreach (var cmp in typ.Value)
				{
					//Debug.Log($"Adding comp from gui: {cmp.ObjectType.Name}");
					_newEnt.AddComponentFromGUI(cmp);
					cmp.SetId(Guid.Empty); //Keep this out of prod code
				}
			}
		    _newEnt.IsPooled = ShouldPoolEntity;

			Serializer.TrySerialize(typeof(Entity), _newEnt, out _data).AssertSuccess();

			var filePath = Application.dataPath + "/Resources/Entities/" + EntityName + ".json";

			Directory.CreateDirectory(Application.dataPath + "/Resources/Entities/" + FileOps.GetTypeFromFullName(EntityName) + "/");

			using (var file = File.Open(filePath, FileMode.Create))
			{
				using (var writer = new StreamWriter(file))
				{
					fsJsonPrinter.PrettyJson(_data, writer);
				}
			}
			IsDirty = false;

			AssetDatabase.Refresh();

			UnityDrawerStatics.RefreshEntityList();
			AvoidDirtyFlag = true;
		}

		private void LoadJson(string filePath)
		{
			if (!File.Exists(filePath))
			{
				Debug.Log("No existing JSON file");
			}
			using (var reader = new StreamReader(filePath))
			{
				var strdata = reader.ReadToEnd();
				fsJsonParser.Parse(strdata, out _data);
				object deserialized = null;
				Serializer.TryDeserialize(_data, typeof(Entity), ref deserialized).AssertSuccessWithoutWarnings();

				// wtf pretty much describes it, FS decides to create a new game object, this gets rid of it
				var wtf = GameObject.Find("New Game Object");
				if (wtf != null)
					UnityEngine.Object.DestroyImmediate(wtf);

				if (deserialized != null)
				{
					//return deserialized as Entity;
					_newEnt = (Entity)deserialized;
					foreach (var comp in _newEnt.Components)
					{
						if (!ComponentPools.ContainsKey(comp.ComponentType))
						{
							ComponentPools[comp.ComponentType] = new List<ComponentEcs>();
						}
						ComponentPools[comp.ComponentType].Add(comp);

#if UNITY_EDITOR
						if (!GroupedComponents.ContainsKey(comp.Grouping))
						{
							GroupedComponents[comp.Grouping] = new List<ComponentEcs>();
						}
						GroupedComponents[comp.Grouping].Add(comp);
#endif
					}
				    ShouldPoolEntity = _newEnt.IsPooled;

				}
				else
				{
					Debug.Log("Not deserialized");
				}
			}
			OnDataChanged?.Invoke();
			AvoidDirtyFlag = true;
		}
	}
}