// /**
//  * EntityGUICreator.cs
//  * Dylan Bailey
//  * 20161209
// */
#pragma warning disable 67
namespace Zenobit.Serialization
{
	#region Dependencies

	using System;
	using System.Collections.Generic;
	using System.IO;
	using AdvancedInspector;
	using Common.Extensions;
	using FullSerializer;
	using UnityEditor;
	using UnityEngine;
	using Zenobit.Common.ZenECS;

	#endregion

	[ExecuteInEditMode]
	[AdvancedInspector(true, false)]
	public class EntityGUICreator : MonoSingleton<EntityGUICreator>,/*MonoBehaviour,*/ IDataChanged
	{
		private static readonly fsSerializer Serializer = new fsSerializer();
		private static fsData _data;

		private string _entityLoad;
		private static Vector3 colorDirty = new Vector3(1, 0, 0);

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

		[Space]
		[Descriptor(":", "Components On Entity")]
		[Inspect(5000)]
		[NonSerialized]
		[Expandable(Expanded = true)]
		public readonly Dictionary<ComponentTypes, List<ComponentEcs>>
			ComponentPools =
				new Dictionary<ComponentTypes, List<ComponentEcs>>(Enum.GetNames(typeof(ComponentTypes)).Length);

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

		[TextField(TextFieldType.JSON)]
		[Inspect(90)]
		public string EntityLoad
		{
			get { return _entityLoad; }
			set
			{
				Reset();
				_entityLoad = Application.dataPath + "/Resources/JSON/" + value + ".json";
				LoadJson(_entityLoad);
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
			ComponentPools[TypeToCreate].Add(
				(ComponentEcs)Activator.CreateInstance(ComponentFactory.ComponentLookup[TypeToCreate]));

			OnDataChanged?.Invoke();
			IsDirty = true;
		}

		[Inspect("GetIsDirty", 100)]
		[Method]
		[Descriptor("Save Entity", "Save Entity To JSON In Resources folder", "", 1, 0.5f, 0.5f)]
		public void SaveEntityToJson()
		{
			_newEnt = new Entity(GetEntityNameFromFullName(EntityName));
			foreach (var typ in ComponentPools)
			{
				foreach (var cmp in typ.Value)
				{
					_newEnt.AddComponent(cmp);
					cmp.SetId(Guid.Empty); //Keep this out of prod code
				}
			}

			Serializer.TrySerialize(typeof(Entity), _newEnt, out _data).AssertSuccess();

			var filePath = Application.dataPath + "/Resources/JSON/" + EntityName + ".json";

			Directory.CreateDirectory(Application.dataPath + "/Resources/JSON/" + GetTypeFromFullName(EntityName) + "/");

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
				ZenLogger.Log("No existing JSON file");
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
					foreach (var cmp in _newEnt.Components)
					{
						if (!ComponentPools.ContainsKey(cmp.ComponentType))
						{
							ComponentPools[cmp.ComponentType] = new List<ComponentEcs>();
						}
						ComponentPools[cmp.ComponentType].Add(cmp);
					}
				}
				else
				{
					ZenLogger.Log("Not deserialized");
				}
			}
			OnDataChanged?.Invoke();
			AvoidDirtyFlag = true;
		}

		private static string GetTypeFromFullName(string fullName)
		{
			var tokens = fullName.Split("/".ToCharArray());
			return tokens[0];
		}

		private static string GetFileNameFromFullName(string fullName)
		{
			var tokens = fullName.Split("/".ToCharArray());
			return tokens[tokens.Length - 1] + ".json";
		}

		private static string GetEntityNameFromFullName(string fullName)
		{
			var tokens = fullName.Split("/".ToCharArray());
			return tokens[tokens.Length - 1];
		}
	}
}