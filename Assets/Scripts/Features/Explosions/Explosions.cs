namespace Features.Explosions
{
	using UnityEngine;
	using Zenobit.Common;
	using Zenobit.Common.ObjectPool;

	public static class Explosions
	{
		//[TextField(TextFieldType.Prefab, "Explosions")]
		//public string TestField = "None";

		public static void Create(string explosionToCreate, Vector3 position)
		{
			var exp = Res.CreateFromPool(explosionToCreate);

			exp.transform.position = position;

			//var ps = (ParticleSystem) exp.GetComponent(typeof(ParticleSystem));
			//exp.ReleaseDelayed(ps.duration + ps.startLifetime);
		}

		//public static GameObject Create(string explosionToCreate)
		//{
		//	var exp = Res.CreateFromPool(explosionToCreate);
		//
		//	var ps = (ParticleSystem) exp.GetComponent(typeof(ParticleSystem));
		//	GameObject.Destroy(exp, ps.duration + ps.startLifetime);
		//}
	}
}