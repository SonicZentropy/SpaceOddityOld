namespace Zen.Common.Debug
{
    using System;
    using UnityEngine;
    using Zen.AI.Common;
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

	    private UILabel _AIStateLabel;

	    public UILabel AIStateLabel
	    {
	        get
	        {
	            if (_AIStateLabel == null)
	            {
	                _AIStateLabel = GameObject.Find("AIStateConsole")?.GetComponent<UILabel>();
	            }
                return _AIStateLabel;
	        }
	    }

		public void Print(string inString)
		{
			DebugLabel.text = inString;
		}

	    public void SetAIState(EAINavState state)
	    {
	        AIStateLabel.text = Enum.GetName(typeof(EAINavState), state);
	    }
	}
}