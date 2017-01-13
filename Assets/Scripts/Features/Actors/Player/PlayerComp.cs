using AdvancedInspector;
using UnityEngine;

namespace Zenobit.Components
{
	using Common.ZenECS;

	public class PlayerComp : AbstractActorComp
	{
		[SerializeField]
		private int _credits;
		
		public int Credits
		{
			get { return _credits; }
			set { _credits = value >= 0 ? value : 0; }
		}

		public const int playerID = 0;
		
		public override ComponentTypes ComponentType => ComponentTypes.PlayerComp;
	}
}