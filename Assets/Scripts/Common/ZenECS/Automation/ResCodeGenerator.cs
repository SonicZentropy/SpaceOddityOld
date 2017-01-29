// /** 
//  * ResCodeGenerator.cs
//  * Will Hart and Dylan Bailey
//  * 2017
// */

namespace Zenobit.Common.Automation
{
	using System.IO;
	using System.Linq;
	using Extensions;
	using UnityEditor;
	using UnityEngine;
	using Zenobit.Editor.Utils;

	public static class ResCodeGenerator
	{
		public static readonly string ResFilePath;
		public static bool AutoGenerate = true;

		//[MenuItem("Zenobit/Enable Res File Auto Generation")]
		public static void EnableGenerateCode()
		{
			AutoGenerate = true;
		}

		//[MenuItem("Zenobit/Disable Res File Auto Generation")]
		public static void DisableGenerateCode()
		{
			AutoGenerate = false;
		}

		static ResCodeGenerator()
		{
			ResFilePath = Application.dataPath + "/Scripts/Common/ZenECS/Res.cs";
		}

		public const string HEADER_FORMAT = @"using UnityEngine;

namespace Zenobit.Common
{
    using System.Collections.Generic;
	using Zenobit.Common.ObjectPool;

	public static class Res
	{
		";

		public const string FOOTER_FORMAT = @"
			
		private static Dictionary<string, GameObject> LoadCache = new Dictionary<string,GameObject>();

		public static GameObject Load(string PrefabToLoad)
		{
			GameObject go;
			if (!LoadCache.TryGetValue(PrefabToLoad, out go))
			{
				go = Resources.Load<GameObject>(PrefabToLoad);
				LoadCache.Add(PrefabToLoad, go);
			}
			return go;
		}

		public static GameObject Instantiate(string PrefabToCreate)
		{
			return Object.Instantiate(Load(PrefabToCreate));
		}

		public static GameObject CreateFromPool(string PrefabToCreate)
		{
			return Load(PrefabToCreate).InstantiateFromPool();
		}
	}
}";

		public static string EntityListFormatted;

		[MenuItem("Zenobit/Generate Res File", false, 60)]
		public static void ForcedGenerateCode()
		{
			bool autoState = AutoGenerate;
			AutoGenerate = true;
			GenerateCode();
			AutoGenerate = autoState;
		}

		public static void GenerateCode()
		{
			if (!AutoGenerate) return;
			UnityDrawerStatics.RefreshAll();
			EntityListFormatted = @"public static class Entities
		{";

			foreach (var ent in UnityDrawerStatics.EntityList)
			{
				string entName = FileOps.GetStringAfterLastSlash(ent).StripNonAlphanumeric();
				EntityListFormatted += "\n\t\t\tpublic const string " + entName + " = \"" + ent + "\";";
			}

			EntityListFormatted += @"
		}
		
		public static class Prefabs
		{";
			foreach (var pre in UnityDrawerStatics.PrefabList)
			{
				string preName = FileOps.GetStringAfterLastSlash(pre).StripNonAlphanumeric();
				EntityListFormatted += "\n\t\t\tpublic const string " + preName + " = \"" + pre + "\";";
			}

			EntityListFormatted += @"
		}";

			string fullString = FileOps.ReplaceLineEndings(HEADER_FORMAT + EntityListFormatted + FOOTER_FORMAT);

			using (var file = File.Open(ResFilePath, FileMode.Create))
			{
				using (var writer = new StreamWriter(file))
				{
					writer.Write(fullString);
				}
			}

			AssetDatabase.Refresh();

		}


		
	}
}