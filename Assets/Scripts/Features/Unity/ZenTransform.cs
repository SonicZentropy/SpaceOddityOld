using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Scripting;

#pragma warning disable 1587

/// <summary>
///   <para>Position, rotation and scale of an object.</para>
/// </summary>
public class ZenTransform
{
	/// <summary>
	///   <para>The position of the transform in world space.</para>
	/// </summary>
	private Vector3 _position = Vector3.zero;
	public Vector3 position
	{
		get { return _position; }
		set
		{
			_position = value;
			bMatrixIsDirty = true;
		}
	}

	private Quaternion _rotation = Quaternion.identity;
	public Quaternion rotation
	{
		get { return _rotation; }
		set
		{
			_rotation = value;
			bMatrixIsDirty = true;
		}
	}

	private Vector3 _scale = Vector3.one;
	public Vector3 scale
	{
		get { return _scale; }
		set
		{
			_scale = value;
			bMatrixIsDirty = true;
		}
	}

	private bool bMatrixIsDirty = true;
	private Matrix4x4 _matrixTRS;

	public Matrix4x4 matrixTRS
	{
		set { _matrixTRS = value; }
		get
		{
			if (bMatrixIsDirty)
			{
				_matrixTRS = GetMatrixTRS();
				bMatrixIsDirty = false;
			}
			return _matrixTRS;
		}
	}

	/// <summary>
	///   <para>The rotation as Euler angles in degrees.</para>
	/// </summary>
	public Vector3 eulerAngles
	{
		get { return this.rotation.eulerAngles; }
		set { this.rotation = Quaternion.Euler(value); }
	}

	/// <summary>
	///   <para>The red axis of the transform in world space.</para>
	/// </summary>
	public Vector3 right
	{
		get { return this.rotation * Vector3.right; }
		set { this.rotation = Quaternion.FromToRotation(Vector3.right, value); }
	}

	/// <summary>
	///   <para>The green axis of the transform in world space.</para>
	/// </summary>
	public Vector3 up
	{
		get { return this.rotation * Vector3.up; }
		set { this.rotation = Quaternion.FromToRotation(Vector3.up, value); }
	}

	/// <summary>
	///   <para>The blue axis of the transform in world space.</para>
	/// </summary>
	public Vector3 forward
	{
		get { return this.rotation * Vector3.forward; }
		set { this.rotation = Quaternion.LookRotation(value); }
	}

	public ZenTransform()
	{}

	/// <summary>
	///   <para>Sets the world space position and rotation of the Transform component.</para>
	/// </summary>
	/// <param name="inPosition"></param>
	/// <param name="inRotation"></param>
	public void SetPositionAndRotation(Vector3 inPosition, Quaternion inRotation)
	{
		position = inPosition;
		rotation = inRotation;
	}

	/// <summary>
	///   <para>Moves the transform in the direction and distance of translation.</para>
	/// </summary>
	/// <param name="translation"></param>
	/// <param name="relativeTo"></param>
	public void Translate(Vector3 translation)
	{
		this.position += translation;

		//if (relativeTo == Space.World)
		//{
		//	this.position += translation;
		//}
		//else
		//{
		//	this.position += this.TransformDirection(translation);
		//}
	}

	/// <summary>
	///   <para>Moves the transform by x along the x axis, y along the y axis, and z along the z axis.</para>
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <param name="relativeTo"></param>
	[ExcludeFromDocs]
	public void Translate(float x, float y, float z)
	{
		this.Translate(x, y, z);
	}

	/// <summary>
	///   <para>Moves the transform in the direction and distance of translation.</para>
	/// </summary>
	/// <param name="translation"></param>
	/// <param name="relativeTo"></param>
	public void Translate(Vector3 translation, Transform relativeTo)
	{
		if (relativeTo)
		{
			this.position += relativeTo.TransformDirection(translation);
		}
		else
		{
			this.position += translation;
		}
	}

	/// <summary>
	///   <para>Moves the transform by x along the x axis, y along the y axis, and z along the z axis.</para>
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <param name="relativeTo"></param>
	public void Translate(float x, float y, float z, Transform relativeTo)
	{
		this.Translate(new Vector3(x, y, z), relativeTo);
	}

	/// <summary>
	///   <para>Applies a rotation of eulerAngles.z degrees around the z axis, eulerAngles.x degrees around the x axis, and eulerAngles.y degrees around the y axis (in that order).</para>
	/// </summary>
	/// <param name="inEulerAngles"></param>
	public void Rotate(Vector3 inEulerAngles)
	{
		Quaternion rhs = Quaternion.Euler(inEulerAngles.x, inEulerAngles.y, inEulerAngles.z);

		this.rotation *= Quaternion.Inverse(this.rotation) * rhs * this.rotation;
	}

	/// <summary>
	///   <para>Applies a rotation of zAngle degrees around the z axis, xAngle degrees around the x axis, and yAngle degrees around the y axis (in that order).</para>
	/// </summary>
	/// <param name="xAngle"></param>
	/// <param name="yAngle"></param>
	/// <param name="zAngle"></param>
	public void Rotate(float xAngle, float yAngle, float zAngle)
	{
		this.Rotate(new Vector3(xAngle, yAngle, zAngle));
		//Space relativeTo = Space.World;
		//this.Rotate(xAngle, yAngle, zAngle, relativeTo);
	}

	/// <summary>
	///   <para>Rotates the transform around axis by angle degrees.</para>
	/// </summary>
	//public void Rotate(Vector3 axis, float angle, [DefaultValue("Space.Self")] Space relativeTo)
	//{
	//	if (relativeTo == Space.Self)
	//	{
	//		this.RotateAroundInternal(base.transform.TransformDirection(axis), angle * 0.0174532924f);
	//	}
	//	else
	//	{
	//		this.RotateAroundInternal(axis, angle * 0.0174532924f);
	//	}
	//}
	/// <summary>
	///   <para>Rotates the transform about axis passing through point in world coordinates by angle degrees.</para>
	/// </summary>
	/// <param name="point"></param>
	/// <param name="axis"></param>
	/// <param name="angle"></param>
	//public void RotateAround(Vector3 point, Vector3 axis, float angle)
	//{
	//	Vector3 vector = this.position;
	//	Quaternion rotation = Quaternion.AngleAxis(angle, axis);
	//	Vector3 vector2 = vector - point;
	//	vector2 = rotation * vector2;
	//	vector = point + vector2;
	//	this.position = vector;
	//	this.RotateAroundInternal(axis, angle * 0.0174532924f);
	//}
	/// <summary>
	///   <para>Rotates the transform so the forward vector points at target's current position.</para>
	/// </summary>
	/// <param name="target">Object to point towards.</param>
	[ExcludeFromDocs]
	public void LookAt(Transform target)
	{
		this.LookAt(target, Vector3.up);
	}

	/// <summary>
	///   <para>Rotates the transform so the forward vector points at target's current position.</para>
	/// </summary>
	/// <param name="target">Object to point towards.</param>
	/// <param name="worldUp">Vector specifying the upward direction.</param>
	public void LookAt(Transform target, [DefaultValue("Vector3.up")] Vector3 worldUp)
	{
		if (target)
		{
			rotation.SetLookRotation(target.position, worldUp);
			//this.LookAt(target.position, worldUp);
		}
	}

	public Matrix4x4 GetMatrixTRS()
	{
		return Matrix4x4.TRS(position, rotation, scale);
	}

	public void SetFromMatrix(Matrix4x4 m)
	{
		position = PositionFromMatrix(m);
		rotation = QuaternionFromMatrix(m);
		scale = ScaleFromMatrix(m);
	}

	public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
	{
		return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
	}

	public static Vector3 ScaleFromMatrix(Matrix4x4 m)
	{
		return new Vector3(
			m.GetColumn(0).magnitude,
			m.GetColumn(1).magnitude,
			m.GetColumn(2).magnitude
			);
	}

	public static Vector3 PositionFromMatrix(Matrix4x4 m)
	{
		return m.GetColumn(3);
	}

	/// <summary>
	///   <para>Transforms direction from local space to world space.</para>
	/// </summary>
	//public Vector3 TransformDirection(Vector3 direction)
	//{
	//	Vector3 result;
	//	
	//	Transform.INTERNAL_CALL_TransformDirection(this, ref direction, out result);
	//	return result;
	//}
	//
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//private static extern void INTERNAL_CALL_TransformDirection(Transform self, ref Vector3 direction, out Vector3 value);

	/// <summary>
	///   <para>Transforms direction x, y, z from local space to world space.</para>
	/// </summary>
	//public Vector3 TransformDirection(float x, float y, float z)
	//{
	//	return this.TransformDirection(new Vector3(x, y, z));
	//}

	/// <summary>
	///   <para>Transforms a direction from world space to local space. The opposite of Transform.TransformDirection.</para>
	/// </summary>
	/// <param name="direction"></param>
	//public Vector3 InverseTransformDirection(Vector3 direction)
	//{
	//	Vector3 result;
	//	Transform.INTERNAL_CALL_InverseTransformDirection(this, ref direction, out result);
	//	return result;
	//}
	//
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//private static extern void INTERNAL_CALL_InverseTransformDirection(Transform self, ref Vector3 direction, out Vector3 value);

	/// <summary>
	///   <para>Transforms the direction x, y, z from world space to local space. The opposite of Transform.TransformDirection.</para>
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	//public Vector3 InverseTransformDirection(float x, float y, float z)
	//{
	//	return this.InverseTransformDirection(new Vector3(x, y, z));
	//}

	/// <summary>
	///   <para>Transforms vector from local space to world space.</para>
	/// </summary>
	/// <param name="vector"></param>
	//public Vector3 TransformVector(Vector3 vector)
	//{
	//	Vector3 result;
	//	Transform.INTERNAL_CALL_TransformVector(this, ref vector, out result);
	//	return result;
	//}
	//
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//private static extern void INTERNAL_CALL_TransformVector(Transform self, ref Vector3 vector, out Vector3 value);

	/// <summary>
	///   <para>Transforms vector x, y, z from local space to world space.</para>
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	//public Vector3 TransformVector(float x, float y, float z)
	//{
	//	return this.TransformVector(new Vector3(x, y, z));
	//}

	/// <summary>
	///   <para>Transforms a vector from world space to local space. The opposite of Transform.TransformVector.</para>
	/// </summary>
	/// <param name="vector"></param>
	//public Vector3 InverseTransformVector(Vector3 vector)
	//{
	//	Vector3 result;
	//	Transform.INTERNAL_CALL_InverseTransformVector(this, ref vector, out result);
	//	return result;
	//}

	//[MethodImpl(MethodImplOptions.InternalCall)]
	//private static extern void INTERNAL_CALL_InverseTransformVector(Transform self, ref Vector3 vector, out Vector3 value);

	/// <summary>
	///   <para>Transforms the vector x, y, z from world space to local space. The opposite of Transform.TransformVector.</para>
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	//public Vector3 InverseTransformVector(float x, float y, float z)
	//{
	//	return this.InverseTransformVector(new Vector3(x, y, z));
	//}
	//
	///// <summary>
	/////   <para>Transforms position from local space to world space.</para>
	///// </summary>
	///// <param name="position"></param>
	//public Vector3 TransformPoint(Vector3 position)
	//{
	//	Vector3 result;
	//	Transform.INTERNAL_CALL_TransformPoint(this, ref position, out result);
	//	return result;
	//}
	//
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//private static extern void INTERNAL_CALL_TransformPoint(Transform self, ref Vector3 position, out Vector3 value);
	//
	///// <summary>
	/////   <para>Transforms the position x, y, z from local space to world space.</para>
	///// </summary>
	///// <param name="x"></param>
	///// <param name="y"></param>
	///// <param name="z"></param>
	//public Vector3 TransformPoint(float x, float y, float z)
	//{
	//	return this.TransformPoint(new Vector3(x, y, z));
	//}
	//
	///// <summary>
	/////   <para>Transforms position from world space to local space.</para>
	///// </summary>
	///// <param name="position"></param>
	//public Vector3 InverseTransformPoint(Vector3 position)
	//{
	//	Vector3 result;
	//	Transform.INTERNAL_CALL_InverseTransformPoint(this, ref position, out result);
	//	return result;
	//}
	//
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//private static extern void INTERNAL_CALL_InverseTransformPoint(Transform self, ref Vector3 position, out Vector3 value);
	//
	///// <summary>
	/////   <para>Transforms the position x, y, z from world space to local space. The opposite of Transform.TransformPoint.</para>
	///// </summary>
	///// <param name="x"></param>
	///// <param name="y"></param>
	///// <param name="z"></param>
	//public Vector3 InverseTransformPoint(float x, float y, float z)
	//{
	//	return this.InverseTransformPoint(new Vector3(x, y, z));
	//}
	//
	///// <summary>
	/////   <para>Unparents all children.</para>
	///// </summary>
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//public extern void DetachChildren();
	//
	///// <summary>
	/////   <para>Move the transform to the start of the local transform list.</para>
	///// </summary>
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//public extern void SetAsFirstSibling();
	//
	///// <summary>
	/////   <para>Move the transform to the end of the local transform list.</para>
	///// </summary>
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//public extern void SetAsLastSibling();
	//
	///// <summary>
	/////   <para>Sets the sibling index.</para>
	///// </summary>
	///// <param name="index">Index to set.</param>
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//public extern void SetSiblingIndex(int index);
	//
	///// <summary>
	/////   <para>Gets the sibling index.</para>
	///// </summary>
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//public extern int GetSiblingIndex();
	//
	///// <summary>
	/////   <para>Finds a child by name and returns it.</para>
	///// </summary>
	///// <param name="name">Name of child to be found.</param>
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//public extern Transform Find(string name);
	//
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//internal extern void SendTransformChangedScale();
	//
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//private extern void INTERNAL_get_lossyScale(out Vector3 value);
	//
	///// <summary>
	/////   <para>Is this transform a child of parent?</para>
	///// </summary>
	///// <param name="parent"></param>
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//public extern bool IsChildOf(Transform parent);
	//
	//public Transform FindChild(string name)
	//{
	//	return this.Find(name);
	//}
	//
	//public IEnumerator GetEnumerator()
	//{
	//	return new Transform.Enumerator(this);
	//}
	//
	///// <summary>
	/////   <para></para>
	///// </summary>
	///// <param name="axis"></param>
	///// <param name="angle"></param>
	//[Obsolete("use Transform.Rotate instead.")]
	//public void RotateAround(Vector3 axis, float angle)
	//{
	//	Transform.INTERNAL_CALL_RotateAround(this, ref axis, angle);
	//}
	//
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//private static extern void INTERNAL_CALL_RotateAround(Transform self, ref Vector3 axis, float angle);
	//
	//[Obsolete("use Transform.Rotate instead.")]
	//public void RotateAroundLocal(Vector3 axis, float angle)
	//{
	//	Transform.INTERNAL_CALL_RotateAroundLocal(this, ref axis, angle);
	//}
	//
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//private static extern void INTERNAL_CALL_RotateAroundLocal(Transform self, ref Vector3 axis, float angle);
	//
	///// <summary>
	/////   <para>Returns a transform child by index.</para>
	///// </summary>
	///// <param name="index">Index of the child transform to return. Must be smaller than Transform.childCount.</param>
	///// <returns>
	/////   <para>Transform child by index.</para>
	///// </returns>
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//public extern Transform GetChild(int index);
	//
	//[Obsolete("use Transform.childCount instead."), GeneratedByOldBindingsGenerator]
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//public extern int GetChildCount();
	//
	//[MethodImpl(MethodImplOptions.InternalCall)]
	//internal extern bool IsNonUniformScaleTransform();
}