using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
//using Zenobit.Common;

public static class UnityDrawerStatics
{
	private static string[] _prefabList;
	public static string[] PrefabList
	{
		set { _prefabList = value; }
		get
		{
			if (_prefabList == null)
				RefreshPrefabList();
			return _prefabList;
		}
	}

	public static void RefreshPrefabList()
	{
		PrefabList = FindAllFilesRecursively(Application.dataPath + "/Resources/Prefabs/", ".prefab")
			.Select(
				f => f.FullName.Replace(@"\", @"/")
					.Replace(Application.dataPath.Replace(@"\", @"/") + "/Resources/", "")
					.Replace(".prefab", ""))
			.ToArray();
	}

	private static string[] _entityList;
	public static string[] EntityList
	{
		set { _entityList = value; }
		get
		{
			if (_entityList == null)
				RefreshEntityList();
			return _entityList;
		}
	}

	private static string[] _componentList;
	public static string[] ComponentList
	{
		set { _componentList = value; }
		get
		{
			if (_componentList == null)
				RefreshComponentList();
			return _componentList;
		}
	}

	private static string[] _allComponentList;
	public static string[] AllComponentList
	{
		set { _allComponentList = value; }
		get
		{
			if (_allComponentList == null)
				RefreshAllComponentList();
			return _componentList;
		}
	}

	public static void RefreshAllComponentList()
	{
		ComponentList = FindAllFilesRecursively(Application.dataPath + "/Scripts/", ".cs")
				.Where(f => f.Name.EndsWith("Comp.cs"))
				.Select(
				f => f.FullName.Replace(@"\", @"/")
					.Replace(Application.dataPath.Replace(@"\", @"/") + "/Scripts/", "")
					.Replace("Components/", "")
					.Replace("Features/", "")
					.Replace(".cs", ""))
				.ToArray();
	}

	public static void RefreshComponentList()
	{
		ComponentList = FindAllFilesRecursively(Application.dataPath + "/Scripts/", ".cs")
				.Where(f => f.Name.EndsWith("Comp.cs") && !f.Name.Contains("Abstract"))
				.Select(
				f => f.FullName.Replace(@"\", @"/")
					.Replace(Application.dataPath.Replace(@"\", @"/") + "/Scripts/", "")
					.Replace("Components/", "")
					.Replace("Features/", "")
					.Replace(".cs", ""))
				.ToArray();
	}

	public static void RefreshEntityList()
	{
		EntityList = FindAllFilesRecursively(Application.dataPath + "/Resources/Entities", ".json")
			.Select(
				f => f.FullName.Replace(@"\", @"/")
					.Replace(Application.dataPath.Replace(@"\", @"/") + "/Resources/Entities/", "")
					.Replace(".json", ""))
			.ToArray();
	}

	public static void RefreshAll()
	{
		RefreshEntityList();
		RefreshComponentList();
		RefreshPrefabList();
	}

	/// <summary>
	/// Duplicate of FileOps.FindAllFilesRecursively due to being in different Assemblies
	/// </summary>
	/// <param name="basePath"></param>
	/// <param name="fileExtension"></param>
	/// <returns></returns>
	private static FileInfo[] FindAllFilesRecursively(string basePath, string fileExtension = null)
	{
		FileInfo[] files = new FileInfo[1];
		if (string.IsNullOrEmpty(basePath))
			return files;

		var dirPath = Path.GetDirectoryName(basePath);
		if (string.IsNullOrEmpty(dirPath))
		{
			return files;
		}

		return new DirectoryInfo(basePath).GetFiles("*" + fileExtension, SearchOption.AllDirectories);
	}
}
