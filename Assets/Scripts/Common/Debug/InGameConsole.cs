namespace Zen.Common.Debug
{
    using System;
    using TMPro;
    using UnityEngine;
    using Zen.AI.Common;
    using Zen.Common.Extensions;

    public class InGameConsole : Singleton<InGameConsole>
	{
		private TextMeshProUGUI _debugLabel;
		public TextMeshProUGUI DebugLabel
		{
			get
			{
				if (_debugLabel == null)
				{
					_debugLabel = GameObject.Find("DebugConsole")?.GetComponent<TextMeshProUGUI>();
				}
				return _debugLabel;
			}
		}

	    private TextMeshProUGUI _AIStateLabel;

	    public TextMeshProUGUI AIStateLabel
	    {
	        get
	        {
	            if (_AIStateLabel == null)
	            {
	                _AIStateLabel = GameObject.Find("AIStateConsole")?.GetComponent<TextMeshProUGUI>();
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