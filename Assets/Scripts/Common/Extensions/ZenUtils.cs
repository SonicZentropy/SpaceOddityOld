﻿// /** 
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

		public static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360F)
				angle += 360F;
			if (angle > 360F)
				angle -= 360F;
			return Mathf.Clamp(angle, min, max);
		}

		//Measures distance in clockwise and counterclockwise directions between angles in radians and returns smallest
		public static float ShortestAngleBetween(float anglestart, float anglefinish)
		{
			float counterClockwiseDistance, clockwiseDistance; //Counterclockwise
			if (anglefinish < anglestart)
			{
				clockwiseDistance = anglestart - anglefinish;
				counterClockwiseDistance = Mathf.PI * 2 - anglestart + anglefinish;
			}
			else
			{
				clockwiseDistance = Mathf.PI * 2 - anglefinish + anglestart;
				counterClockwiseDistance = anglefinish - anglestart;
			}
			if (counterClockwiseDistance < clockwiseDistance) return counterClockwiseDistance;
			return -clockwiseDistance;
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

		public static class PhysicsUtil
		{
			public static void ApplyExplosionForce(EntityWrapper objectHit, Vector3 forceCenter, float forceMagnitude = 100f)
			{
				if (forceMagnitude.IsAlmost(0)) return;
				Vector3 forceDirection = objectHit.transform.position - forceCenter;
				var rbComp = objectHit.Entity.GetComponentDownward<RigidbodyComp>()?.Rigidbody;
				if (rbComp == null)
				{
					return;
				}
				Vector3 added = forceDirection.normalized * forceMagnitude * 10;
				//Debug.Log($"Applying force to {objectHit.gameObject.name}: {added}");

				rbComp.AddForce(forceDirection.normalized * forceMagnitude * 100, ForceMode.Impulse);
			}
		}

		public static class QuaternionUtil
		{
			public static Quaternion RotationToTarget2D(Vector3 objectToRotate, Vector3 targetPos)
			{
				var diff = targetPos - objectToRotate;
				diff.Normalize();

				var rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
				return Quaternion.Euler(0f, 0f, rot_z - 90);
			}

			public static Quaternion SlerpLookAtTarget(Transform currentTransform, Vector3 positionToFace, float SlerpAmount)
			{
				var q = Quaternion.LookRotation(positionToFace - currentTransform.position);
				return Quaternion.Slerp(currentTransform.rotation, q, SlerpAmount);
			}
		}

		public static class Vec2Util
		{
			//Checks angle in radians between vectors. Positive is counterClockwise, negative is clockwise.
			public static float AngleBetweenVectors(Vector2 first, Vector2 second)
			{
				float ang1 = Mathf.Atan2(first.y, first.x), ang2 = Mathf.Atan2(second.y, second.x);
				return ShortestAngleBetween(ang1, ang2);
			}

			public static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal)
			{
				return 2.0f * Vector2.Dot(inDirection, inNormal) * inNormal - inDirection;
			}
		}

		public static class Vec3Util
		{
			public static Vector3 GetVectorsSum(IList<Vector3> input)
			{
				Vector3 output = Vector3.zero;
				for (int i = 0; i < input.Count; i++)
				{
					output += input[i];
				}
				return output;
			}

			public static Vector3 GetVectorsAvg(IList<Vector3> input)
			{
				Vector3 output = Vector3.zero;
				for (int i = 0; i < input.Count; i++)
				{
					output += input[i];
				}
				output /= input.Count;
				return output;
			}

			public static Vector3 GetRandomVector(float xRange = 10f, float yRange = 10f, float zRange = 10f)
			{
				return new Vector3(
				                   UnityEngine.Random.Range(-xRange, xRange),
				                   UnityEngine.Random.Range(-yRange, yRange),
				                   UnityEngine.Random.Range(-zRange, zRange));
			}
		}

		public static class DoubleUtil
		{
			public static bool AreAlmostEqual(double a, double b)
			{
				//return Mathf.Abs(b - a) < (double) Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)), Mathf.Epsilon * 8f);
				return Math.Abs(a - b) <= 0.00001;
			}
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