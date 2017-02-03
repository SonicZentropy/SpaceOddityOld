using UnityEngine;
using UnityEditor;
using Zen.Common.Automation;
using Zen.Editor.Utils;

class ZenAllAssetPostprocessor : AssetPostprocessor
{
	static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		//bool regenerateRes = false;
		//bool regenerateComps = false;
		foreach (string str in importedAssets)
		{/*
			if (str.Contains("Entities") && str.Contains(".json"))
			{
				regenerateRes = true;
				//Debug.Log("Reimported Asset: " + str);
			}

			if (str.Contains("Comp.cs"))
			{
				regenerateComps = true;
			}*/

			if (str.Contains("RA.cs"))
			{
				FixRewiredFile(str);
			}
		}
		/*foreach (string str in deletedAssets)
		{
			if (str.Contains("Entities") && str.Contains(".json"))
			{
				regenerateRes = true;
				//Debug.Log("Deleted Asset: " + str);
			}
			if (str.Contains("Comp.cs"))
			{
				regenerateComps = true;
			}
		}

		for (int i = 0; i < movedAssets.Length; i++)
		{
			if (movedAssets[i].Contains("Entities") && movedAssets[i].Contains(".json"))
			{
				regenerateRes = true;
			}
			if (movedAssets[i].Contains("Comp.cs"))
			{
				regenerateComps = true;
			}
		}*/

	/*	if (regenerateRes)
		{
			//Debug.Log("Regenerating Res File");
			UnityDrawerStatics.RefreshEntityList();
			ResCodeGenerator.GenerateCode();
		}

		if (regenerateComps)
		{
			//Debug.Log("In Postprocessor at " +System.DateTime.Now + ", is compiling: " + EditorApplication.isCompiling);
			UnityDrawerStatics.RefreshAllComponentList();
			ComponentFactoryGenerator.GenerateCode();
		}*/
	}

	static void FixRewiredFile(string file)
	{
		string filePath = Application.dataPath + file.Replace("Assets", "");
		FileOps.ReplaceTextInFile(filePath, "RewiredActions", "RA");

//		string fileData = FileOps.ReturnFileAsString(filePath);
//		fileData = fileData.Replace("RewiredActions", "RA");
//		FileOps.WriteStringToFile(fileData, filePath);
	}
}