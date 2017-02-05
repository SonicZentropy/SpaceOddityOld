// /** 
// * ZenUtils.cs
// * Dylan Bailey
// * 20161203
// */

namespace Zen.Common.Extensions
{
	#region Dependencies

	using System;
	using System.Collections.Generic;
	using Components;
	using MEC;
	using UnityEngine;
	using ZenECS;
	using Random = System.Random;

	#endregion

	public static class ZenUtils
	{
		private static readonly Random RandGen = new Random();

		public static void ExecuteAtEndOfFrame(Action callback)
		{
			Timing.RunCoroutine(_ExecuteAtEndOfFrame(callback), Segment.LateUpdate);
		}

		private static IEnumerator<float> _ExecuteAtEndOfFrame(Action callback)
		{
			//Debug.Log($"In execute at end of frame");
			callback?.Invoke();
			yield return 0f;
		}

		public static Vector3 GetTerrainPositionFromCursor(int terrainLayerMask)
		{
			var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			Physics.Raycast(mouseRay, out hitInfo, float.MaxValue, terrainLayerMask);

			return hitInfo.point;
		}


			public static void ApplyExplosionForce(EntityWrapper objectHit, Vector3 forceCenter, float forceMagnitude = 100f)
			{
				if (forceMagnitude.IsAlmost(0)) return;
				Vector3 forceDirection = objectHit.transform.position - forceCenter;
				var rbComp = objectHit.Entity.GetComponentDownward<RigidbodyComp>()?.rigidbody;
				if (rbComp == null)
				{
					return;
				}
				Vector3 added = forceDirection.normalized * forceMagnitude * 10;
				//Debug.Log($"Applying force to {objectHit.gameObject.name}: {added}");

				rbComp.AddForce(forceDirection.normalized * forceMagnitude * 100, ForceMode.Impulse);
			}



		public static class RandUtil
		{
			public static long RandomLong()
			{
				var buffer = new byte[8];
				RandGen.NextBytes(buffer);
				return BitConverter.ToInt64(buffer, 0);
			}

			public static byte RandomByte()
			{
				return (byte) UnityEngine.Random.Range(0, byte.MaxValue);
			}

			public static float GetRandomSign()
			{
				return UnityEngine.Random.Range(0, 1f) < 0.5 ? -1 : 1f;
			}
		}

		public static float SmoothApproach(
			float pastPosition,
			float pastTargetPosition,
			float targetPosition,
			float speed,
			float deltaTime)
		{
			float t = deltaTime * speed;
			float v = (targetPosition - pastTargetPosition) / t;
			float f = pastPosition - pastTargetPosition + v;
			return targetPosition - v + f * Mathf.Exp(-t);
		}



		public static int LayerMaskFromIDs(params int[] ints)
		{
			int mask = 0;
			foreach (int i in ints)
			{
				mask = mask | i;
			}
			return mask;
		}
	}
}