// /** 
//  * ExtensionMethodsZen.cs
//  * Dylan Bailey
//  * 20161210
// */

using System;

#pragma warning disable 0414, 0219, 649, 169, 1570

namespace Zenobit.Common.Extensions
{
	#region Dependencies

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using ZenECS;

	#endregion

	public static class ExtensionMethodsZen
	{
		public static T GetComponentDownThenUp<T>(this Component gameObject)
		{
			var comp = gameObject.GetComponentInChildren<T>();
			if (comp == null)
				comp = gameObject.GetComponentInParent<T>();

			return comp;
		}

		public static T GetComponentDownThenUp<T>(this GameObject gameObject)
		{
			var comp = gameObject.GetComponentInChildren<T>();
			if (comp == null)
				comp = gameObject.GetComponentInParent<T>();

			return comp;
		}

		public static void SetAllActiveRecursively(this GameObject rootObject, bool active)
		{
			rootObject.SetActive(active);

			foreach (Transform childTransform in rootObject.transform)
			{
				SetAllActiveRecursively(childTransform.gameObject, active);
			}
		}

		public static void SetChildrenLayerRecursively(this GameObject rootObject)
		{
			var newLayer = rootObject.layer;
			foreach (Transform t in rootObject.transform)
				t.gameObject.layer = newLayer;
		}

		public static T[] RemoveAt<T>(this T[] source, int index)
		{
			var dest = new T[source.Length - 1];
			int i = 0, j = 0;

			while (i < source.Length)
			{
				if (i != index)
				{
					dest[j] = source[i];
					j++;
				}
				i++;
			}

			return dest;
		}

		/// <summary>
		///     Gets or add a component. Usage example:
		///     BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
		/// </summary>
		public static T GetOrAddComponent<T>(this Component child) where T : Component
		{
			var result = child.GetComponent<T>();
			if (result == null)
			{
				result = child.gameObject.AddComponent<T>();
			}
			return result;
		}

		public static RaycastHit2D[] FilterObjects(this RaycastHit2D[] hits, params GameObject[] objsToFilter)
		{
			var filtered = new List<RaycastHit2D>(hits.Length);
			foreach (var t in hits)
			{
				var shouldFilter = false;
				foreach (var t1 in objsToFilter)
				{
					if (t && (t.transform.gameObject == t1))
					{
						shouldFilter = true;
					}
				}

				if (!shouldFilter) filtered.Add(t);
			}

			return filtered.ToArray();
		}

		public static bool HasFlag(this Enum e, Enum flag)
		{
			return (Convert.ToInt64(e) & Convert.ToInt64(flag)) != 0;
		}

		public static float MaxVectorElement(this Vector3 v)
		{
			return Mathf.Max(v.x, v.y, v.z);
		}

		public static Vector3 Clamp(this Vector3 v, float min, float max)
		{
			return new Vector3(
				Mathf.Clamp(v.x, min, max),
				Mathf.Clamp(v.y, min, max),
				Mathf.Clamp(v.z, min, max)
				);
		}

		public static bool IsAlmost(this float a, float b)
		{
			return Mathf.Approximately(a, b);
		}

		public static bool IsAlmost(this float a, int b)
		{
			return Mathf.Approximately(a, b);
		}

		public static string StripNonAlphanumeric(this string inString)
		{
			return new string(inString.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());
		}

		public static bool IsNullEmpty(this string inString)
		{
			return String.IsNullOrEmpty(inString);
		}

		public static int GetLayerIndex(this LayerMask layerMask)
		{
			int layerNumber = 0;
			int layer = layerMask.value;
			while(layer > 0)
			{
				layer = layer >> 1;
				layerNumber++;
			}
			if (layerNumber < 1) return 0;
			return layerNumber-1;
		}
	}
}
