using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	public ItemType ItemType { get; set; } // 0 weapon 1 armor 2 shield fuck it, it is like this for now. likely to change it for something better later
	public int PriceValue { get; set; }
	public string ItemName { get; set; }
}

public enum ItemType
{
	Weapon,
	Armor,
	Shield
}
