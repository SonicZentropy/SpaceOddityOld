// /** 
//  * JSONSerializer.cs
//  * Dylan Bailey
//  * 20161209
// */

#pragma warning disable 0414, 0219, 649, 169, 618, 1570

namespace Zen.Common.Serialization
{
    #region Dependencies

    using System;
    using System.IO;
    using FullSerializer;
    using UnityEditor;
    using UnityEngine;
    using Zen.Common.ZenECS;

    #endregion

    public static class JsonSerializer
    {
        private static readonly fsSerializer Serializer = new fsSerializer();
        private static fsData _data;

        public static void SaveToJson<T>(T classToSave, string optionalInstanceId = "") where T : ComponentEcs
        {
            Serializer.TrySerialize(classToSave.ObjectType, classToSave, out _data).AssertSuccess();

            //_serializer.TrySerialize(classToSave.ObjectType, classToSave, out data).AssertSuccessWithoutWarnings();

            var filePath = Application.dataPath + "/Resources/JSON/" + classToSave.AssetParentFolder + "/" +
                           classToSave.AssetName;
            if (!optionalInstanceId.Equals("")) filePath += "_" + optionalInstanceId;
            filePath += ".json";

            //Debug.Log("Saving to: " + filePath);
            Directory.CreateDirectory(Application.dataPath + "/Resources/JSON/" + classToSave.AssetParentFolder + "/");

            using (var file = File.Open(filePath, FileMode.Create))
            {
                using (var writer = new StreamWriter(file))
                {
                    fsJsonPrinter.PrettyJson(_data, writer);
                }
            }
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public static T LoadFromJson<T>(T classToLoad, string optionalInstanceId = "") where T : ComponentEcs
        {
            var typeToLoad = classToLoad.GetType();
            var filePath = Application.dataPath + "/Resources/JSON/" + classToLoad.AssetParentFolder + "/" +
                           classToLoad.AssetName;
            if (!optionalInstanceId.Equals("")) filePath += "_" + optionalInstanceId;
            filePath += ".json";

            //Debug.Log("Loading from: " + filePath);
            if (!File.Exists(filePath))
            {
                Debug.Log("No existing JSON file, creating new model now.");
                //T newT = new T(classToLoad.Owner);
                var newT = (T) Activator.CreateInstance(classToLoad.ObjectType, classToLoad.Owner);
                SaveToJson(newT, optionalInstanceId);
            }
            using (var reader = new StreamReader(filePath))
            {
                var strdata = reader.ReadToEnd();
                fsJsonParser.Parse(strdata, out _data);
                object deserialized = null;
                Serializer.TryDeserialize(_data, typeToLoad, ref deserialized).AssertSuccess();

                if (deserialized != null)
                {
                    return deserialized as T;
                }
                Debug.Log("Not deserialized");
                return (T) Activator.CreateInstance(classToLoad.ObjectType, classToLoad.Owner);
            }
        }

        public static void OverwriteFromJson<T>(T classToLoad, string optionalInstanceId = "")
            where T : ComponentEcs, new()
        {
            var filePath = Application.dataPath + "/Resources/JSON/" + classToLoad.AssetParentFolder + "/" +
                           classToLoad.AssetName;
            if (!optionalInstanceId.Equals("")) filePath += "_" + optionalInstanceId;
            filePath += ".json";

            //Debug.Log("Overwriting from: " + filePath);
            if (!File.Exists(filePath))
            {
                Debug.Log("No existing JSON file, creating new model file before overwrite.");
                var newT = new T();
                SaveToJson(newT, optionalInstanceId);
                //var newT = Activator.CreateInstance(classToLoadType);
                //SaveToJSON(newT, optionalInstanceID);
            }
            //using (StreamReader reader = new StreamReader(filePath))
            //{
            //	string strdata = reader.ReadToEnd();
            //	//EditorJsonUtility.FromJsonOverwrite(strdata, classToLoad);
            //}
        }

        public static void OverwriteNew<T>(T classToLoad, string optionalInstanceId = "") where T : ComponentEcs, new()
        {
            var filePath = Application.dataPath + "/Resources/JSON/" + classToLoad.AssetParentFolder + "/" +
                           classToLoad.AssetName;
            if (!optionalInstanceId.Equals("")) filePath += "_" + optionalInstanceId;
            filePath += ".json";

            //Debug.Log("Loading from: " + filePath);
            if (!File.Exists(filePath))
            {
                Debug.Log("No existing JSON file, creating new model now.");
                var newT = new T();
                SaveToJson(newT, optionalInstanceId);
            }
            using (var reader = new StreamReader(filePath))
            {
                var strdata = reader.ReadToEnd();
                fsJsonParser.Parse(strdata, out _data);

                object deserialized = null;
                Serializer.TryDeserialize(_data, typeof(T), ref deserialized).AssertSuccess();

                if (deserialized != null)
                {
                    var newClass = (T) deserialized;
                    //classToLoad = (T) newClass.Clone();
                    classToLoad = newClass;
                }
                else //make new T b/c JSON doesn't exist
                {
                    classToLoad = new T();
                }
            }
        }

        /*public static void OverwriteFromJSON(Type classToLoadType, ComponentECS classToLoad, string optionalInstanceID = "") //where T : ComponentECS, new()
		{
			string filePath = Application.dataPath + "/Resources/JSON/" + classToLoad.AssetParentFolder + "/" + classToLoad.AssetName;
			if (!optionalInstanceID.Equals("")) filePath += "_" + optionalInstanceID;
			filePath += ".json";

			Debug.Log("Overwriting from: " + filePath);
			if (!File.Exists(filePath))
			{
				Debug.Log("No existing JSON file, Nothing from which to overwrite.");
				//return;
				var newT = Activator.CreateInstance(classToLoadType);
				//SaveToJSON(newT, optionalInstanceID);
			}
			using (StreamReader reader = new StreamReader(filePath))
			{
				string strdata = reader.ReadToEnd();
				JsonUtility.FromJsonOverwrite(strdata, classToLoad);
			}
		}*/
    }
}