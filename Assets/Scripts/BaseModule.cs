using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails
{
	public enum GAMEMODULES
	{
		Combat = 0,
		Dialogue,
		Travel,
		Inventory,
		Character,
		Party,
        OverWorld,
		ModuleCount
	};


	public class BaseModule : MonoBehaviour
	{
		public GAMEMODULES ModuleType;

		public virtual void Initialize(string filename) { }
		public virtual void Pause() { }
		public virtual void Resume() { }
		public virtual void Quit() { }

	}
}
