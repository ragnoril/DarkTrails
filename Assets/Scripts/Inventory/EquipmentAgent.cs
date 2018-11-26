using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.Inventory
{
	public class EquipmentAgent : MonoBehaviour
	{
		public ItemType FilterType;
		public Text NameText;
		public Item ItemData;

		// Use this for initialization
		void Start()
		{
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void UpdateItemData()
		{
			NameText.text = ItemData.ItemName;
		}
	}
}
