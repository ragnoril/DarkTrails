using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails
{
	public class Item
	{
		public ItemType ItemType { get; set; } // 0 weapon 1 armor 2 shield 3 consumables fuck it, it is like this for now. likely to change it for something better later
		public int PriceValue { get; set; }
		public string ItemName { get; set; }
        public bool IsStackable { get; set; }
        public int MaxStack { get; set; }
	}

	public enum ItemType
	{
		Weapon,
		BodyArmor,
        Helmet,
        Boots,
		Shield
	}
}
