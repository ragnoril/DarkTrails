using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
	public int MinDamage { get; set; }
	public int MaxDamage { get; set; }

	public bool IsTwoHanded { get; set; }
	public int WeaponReach { get; set; }
	public int WeaponRange { get; set; }
	public int WeaponType { get; set; }
}
