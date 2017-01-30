// /**
//  * ComponentFactoryGenerator.cs
//  * Dylan Bailey
//  * 2017
// */

namespace Zen.Common.Automation
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;
	using Zen.Editor.Utils;

	public static class ComponentFactoryGenerator
	{
		public static readonly string CompFactoryFilePath;
		public static bool AutoGenerate;
		public static bool IsGenerating;

		[MenuItem("Zen/Enable Component Factory Auto Generation", false, 25)]
		public static void EnableGenerateCode()
		{
			AutoGenerate = true;
		}

		[MenuItem("Zen/Disable Component Factory Auto Generation", false, 27)]
		public static void DisableGenerateCode()
		{
			AutoGenerate = false;
		}

		static ComponentFactoryGenerator()
		{
			CompFactoryFilePath = Application.dataPath + "/Scripts/Common/ZenECS/ComponentFactory.cs";
		}

		public const string HEADER_FORMAT = @"// /**
//  * ComponentFactory.cs
//  * Dylan Bailey
//  * 20161210
// */

namespace Zen.Common.ZenECS
{
    #region Dependencies

    using System;
    using System.Collections.Generic;
    using Zen.Components;

    #endregion

    public static class ComponentFactory
    {
        public static readonly Dictionary<ComponentTypes, Type> ComponentLookup = new Dictionary<ComponentTypes, Type>(new FastEnumIntEqualityComparer<ComponentTypes>())
        {";

		public const string FOOTER_FORMAT = @"
    }
}";

		public static string ComponentListFormatted;
		public static string ComponentEnums;

		[MenuItem("Zen/Generate Component Factory File", false, 65)]
		public static void ForcedGenerateCode()
		{
			bool autoState = AutoGenerate;
			AutoGenerate = true;
			GenerateCode();
			AutoGenerate = autoState;
		}

		//Duplicated from UnityDrawerStatics to reduce compile time dependencies (Fixes some unity-specific trouble)
		public static List<string> GetAllComponentList()
		{
			return FileOps.FindAllFilesRecursively(Application.dataPath + "/Scripts/", ".cs")
				.Where(f => f.Name.EndsWith("Comp.cs"))
				.Select(
				f => f.FullName.Replace(@"\", @"/")
					.Replace(Application.dataPath.Replace(@"\", @"/") + "/Scripts/", "")
					.Replace("Components/", "")
					.Replace("Features/", "")
					.Replace(".cs", ""))
				.ToList();
		}

		public static void GenerateCode()
		{
			if (!AutoGenerate || IsGenerating) return;
			//Re-entrant code generation no good
			IsGenerating = true;
			ComponentListFormatted = "";
			ComponentEnums = "";

			var allcomp = GetAllComponentList();

			foreach (var comp in allcomp)
			{
				string entName = FileOps.GetStringAfterLastSlash(comp);
				string comma = ",";
				if (comp == allcomp.Last()) //last element
					comma = "";

				string newLine = "\n\t\t\t" + "{ComponentTypes." + entName + ", typeof(" + entName + ")}" + comma;
				ComponentListFormatted += newLine;
				ComponentEnums += "\n\t\t" + entName + comma;
				Console.WriteLine(newLine);
			}

			ComponentListFormatted += @"
		};

        public static ComponentEcs Create(ComponentTypes type)
        {
            if (!ComponentLookup.ContainsKey(type)) return null;
	        return ComponentCache.Instance.Get(type);
        }

	    public static ComponentEcs Instantiate(ComponentTypes type)
	    {
		    return (ComponentEcs) Activator.CreateInstance(ComponentLookup[type]);
	    }
    }

    public enum ComponentTypes
    {";

			ComponentListFormatted += ComponentEnums;

			string fullString = FileOps.ReplaceLineEndings(HEADER_FORMAT + ComponentListFormatted + FOOTER_FORMAT);
			//Debug.Log(fullString);

			using (var file = File.Open(CompFactoryFilePath, FileMode.Truncate))
			{
				using (var writer = new StreamWriter(file))
				{
					writer.Write(fullString);
				}
			}
			int idx = CompFactoryFilePath.IndexOf("Assets");
			string newFilePath = CompFactoryFilePath.Substring(idx, CompFactoryFilePath.Length - idx);
			AssetDatabase.ImportAsset(newFilePath);
			IsGenerating = false;

		}


	}

}