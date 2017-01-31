namespace Zen.Common.Debug
{
	using UnityEngine;
	using Zen.Common.Extensions;

	public class InGameConsole : Singleton<InGameConsole>
	{
		private UILabel _debugLabel;
		public UILabel DebugLabel
		{
			get
			{
				if (_debugLabel == null)
				{
					_debugLabel = GameObject.Find("DebugConsole")?.GetComponent<UILabel>();
				}
				return _debugLabel;
			}
		}

		public void Print(string inString)
		{
			DebugLabel.text = inString;
		}
	}
}