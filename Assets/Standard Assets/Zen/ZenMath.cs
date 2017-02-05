// /** 
//  * MathApprox.cs
//  * Dylan Bailey
//  * 20161209
// */


#pragma warning disable 0414, 0219, 649, 169, 1570

namespace Zen.Common.Extensions
{
	#region Dependencies

	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using UnityEngine;

	#endregion

	public static class ZenMath
	{

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

		public class MathApprox
		{
			public static float FastSqrt(float z)
			{
				if (Math.Abs(z) < float.Epsilon) return 0;
				FloatIntUnion u;
				u.tmp = 0;
				u.f = z;
				u.tmp -= 1 << 23; /* Subtract 2^m. */
				u.tmp >>= 1; /* Divide by 2. */
				u.tmp += 1 << 29; /* Add ((b + 1) / 2) * 2^m. */
				return u.f;
			}

			[StructLayout(LayoutKind.Explicit)]
			private struct FloatIntUnion
			{
				[FieldOffset(0)]
				public float f;

				[FieldOffset(0)]
				public int tmp;
			}

			public static float FindFastApproxDistance(float x1, float y1, float x2, float y2)
			{
				return FastSqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
			}

			public static float FindFastApproxDistance(Vector2 first, Vector2 second)
			{
				return FastSqrt((second.x - first.x) * (second.x - first.x) + (second.y - first.y) * (second.y - first.y));
			}

			public static float FindFastApproxDistance(GameObject first, GameObject second)
			{
				return FindFastApproxDistance(first.transform.position, second.transform.position);
			}

			public static float FindFasterApproxDistanceSquared(Vector2 first, Vector2 second)
			{
				return (second.x - first.x) * (second.x - first.x) + (second.y - first.y) * (second.y - first.y);
			}

			public static float FindFasterApproxDistanceSquared(float x1, float y1, float x2, float y2)
			{
				return (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
			}
		}
	}
}