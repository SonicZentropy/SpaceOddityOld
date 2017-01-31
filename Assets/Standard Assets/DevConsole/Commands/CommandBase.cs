using System;
using UnityEngine;
using System.Reflection;

namespace DevConsole
{
    public enum VariableType {Delagate, Float, Int, Bool}

	[System.Serializable]
	public abstract class CommandBase
    {

		//=======================
		//VARS
		//=======================
		public string name{
			get; private set;
		}
		public string helpText{
			get; private set;
		}
		public delegate void HelpMethod();
		HelpMethod helpMethod;
		Delegate method;
        VariableType varType;

        object refObject;
        Type objectType;
        string varName;
		
		//==============================
		//CONSTRUCTORS
		//==============================
		public CommandBase (string name, Delegate method){
			this.name = name;
			this.method = method;
            varType = VariableType.Delagate;
		}
		public CommandBase (string name, Delegate method, string helpText):this(name, method){
			this.helpText = helpText;
            varType = VariableType.Delagate;
        }
		public CommandBase (string name, Delegate method, HelpMethod helpMethod):this(name, method){
			this.helpMethod = helpMethod;
            varType = VariableType.Delagate;
        }
		public CommandBase(Delegate method):this(method.Method.DeclaringType.Name+"."+method.Method.Name, method){ varType = VariableType.Delagate; }
		public CommandBase(Delegate method, string helpText):this(method.Method.DeclaringType.Name+"."+method.Method.Name, method, helpText){ varType = VariableType.Delagate; }
		public CommandBase(Delegate method, HelpMethod helpMethod):this(method.Method.DeclaringType.Name+"."+method.Method.Name, method, helpMethod){ varType = VariableType.Delagate; }

        public CommandBase(string _name, string _varName, VariableType _varType, object _object)
        {
            this.name = _name;
            refObject = _object;
            varName = _varName;
            varType = VariableType.Float;
            objectType = refObject.GetType();
        }
        //=======================
        //EXECUTE
        //=======================
        public void Execute(string args)
        {
            switch (varType)
            {
                case VariableType.Delagate:
                    try
                    {
                        method.Method.Invoke(method.Target, ParseArguments(args));
                    }
                    catch (Exception e)
                    {
                        Console.LogError(e.InnerException.Message + (Console.verbose ? "\n" + e.InnerException.StackTrace : string.Empty));
                    }
                    break;

                case VariableType.Float:
                    ChangeFloat(args);
                    break;

                case VariableType.Int:
                    ChangeInt(args);
                    break;

                case VariableType.Bool:
                    ChangeBool(args);
                    break;
            }
        }

        void ChangeFloat(string sValue)
        {         
            float fValue;

            if (float.TryParse(sValue, out fValue))
            {
                FieldInfo[] myField = objectType.GetFields();
             
                for (int i = 0; i < myField.Length; i++)
                {
                    // Determine whether or not each field is a special name.
                    if (myField[i].Name == varName)
                    {
                        myField[i].SetValue(refObject, fValue);
                    }
                }
                    Console.Log("success", Color.green);
                }
            else
                Console.LogError("The entered value is not a valid float value");
        }

        void ChangeInt(string sValue)
        {
            int iValue;

            if (int.TryParse(sValue, out iValue))
            {
                FieldInfo[] myField = objectType.GetFields();

                for (int i = 0; i < myField.Length; i++)
                {
                    // Determine whether or not each field is a special name.
                    if (myField[i].Name == varName)
                    {
                        myField[i].SetValue(refObject, iValue);
                    }
                }
                Console.Log("success", Color.green);
            }
            else
                Console.LogError("The entered value is not a valid int value");
        }

        void ChangeBool(string sValue)
        {
            bool bValue;

            if (bool.TryParse(sValue, out bValue))
            {
                FieldInfo[] myField = objectType.GetFields();

                for (int i = 0; i < myField.Length; i++)
                {
                    // Determine whether or not each field is a special name.
                    if (myField[i].Name == varName)
                    {
                        myField[i].SetValue(refObject, bValue);
                    }
                }
                Console.Log("success", Color.green);
            }
            else
                Console.LogError("The entered value is not a valid bool");
        }

        protected abstract object[] ParseArguments (string args);
		public virtual void ShowHelp(){
			if (helpMethod != null)
				helpMethod();
			else
				Console.LogInfo("Command Info: " + (helpText == null?"There's no help for this command":helpText));
		}
		//=======================
		//HELPERS
		//=======================
		protected T GetValueType<T>(string arg){
			try{
				T returnValue;
				if (typeof(bool) == typeof(T)){
					bool result;
					if (StringToBool(arg,out result))
						returnValue = (T)(object)result;
					else
						throw new System.Exception();
				}
				else
					returnValue = (T)System.Convert.ChangeType(arg,typeof(T));
				return returnValue;
			}catch{
				throw new ArgumentException("The entered value is not a valid "+typeof(T)+" value");
			}
		}
		protected bool StringToBool(string value, out bool result){
			bool bResult = result= false;
			int iResult = 0;
			
			if (bool.TryParse(value, out bResult))
				result = bResult;
			else if(int.TryParse(value, out iResult)){
				if (iResult == 1 || iResult == 0)
					result = iResult==1?true:false;
				else
					return false;
			}
			else{
				string s = value.ToLower().Trim();
				if (s.Equals("yes") || s.Equals("y"))
					result = true;
				else if (s.Equals("no") || s.Equals("n"))
					result = false;
				else
					return false;
			}
			return true;
		}
	}
}