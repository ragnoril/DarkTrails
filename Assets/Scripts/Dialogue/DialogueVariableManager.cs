using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails.Dialogue
{
	public class DialogueVariableManager : Yarn.Unity.VariableStorageBehaviour
	{
		Dictionary<string, Yarn.Value> Variables = new Dictionary<string, Yarn.Value>();

		/// A default value to apply when the object wakes up, or
		/// when ResetToDefaults is called
		[System.Serializable]
		public class DefaultVariable
		{
			/// Name of the variable
			public string name;
			/// Value of the variable
			public string value;
			/// Type of the variable
			public Yarn.Value.Type type;
		}

		/// Our list of default variables, for debugging.
		public DefaultVariable[] DefaultVariables;

		public override void ResetToDefaults()
		{
			Clear();

			// For each default variable that's been defined, parse the string
			// that the user typed in in Unity and store the variable
			foreach (var variable in DefaultVariables)
			{

				object value;

				switch (variable.type)
				{
					case Yarn.Value.Type.Number:
						float f = 0.0f;
						float.TryParse(variable.value, out f);
						value = f;
						break;

					case Yarn.Value.Type.String:
						value = variable.value;
						break;

					case Yarn.Value.Type.Bool:
						bool b = false;
						bool.TryParse(variable.value, out b);
						value = b;
						break;

					case Yarn.Value.Type.Variable:
						// We don't support assigning default variables from other variables
						// yet
						Debug.LogErrorFormat("Can't set variable {0} to {1}: You can't " +
							"set a default variable to be another variable, because it " +
							"may not have been initialised yet.", variable.name, variable.value);
						continue;

					case Yarn.Value.Type.Null:
						value = null;
						break;

					default:
						throw new System.ArgumentOutOfRangeException();

				}

				var v = new Yarn.Value(value);

				SetValue("$" + variable.name, v);
			}

			SetGameVariables();
			//SetCharacterVariables();
			//SetPartyVariables();
			//SetInventoryVariables();

		}

		/// Set a variable's value
		public override void SetValue(string variableName, Yarn.Value value)
		{
			// Copy this value into our list
			Variables[variableName] = new Yarn.Value(value);
		}

		/// Get a variable's value
		public override Yarn.Value GetValue(string variableName)
		{
			// If we don't have a variable with this name, return the null value
			if (Variables.ContainsKey(variableName) == false)
				return Yarn.Value.NULL;

			return Variables[variableName];
		}

		/// Erase all variables
		public override void Clear()
		{
			Variables.Clear();
		}

		private void SetPartyVariables()
		{
			throw new NotImplementedException();
		}

		private void SetCharacterVariables()
		{
			SetValue("$char_", new Yarn.Value(0));
		}

		private void SetGameVariables()
		{
			SetValue("$roll_result", new Yarn.Value(0));
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}
