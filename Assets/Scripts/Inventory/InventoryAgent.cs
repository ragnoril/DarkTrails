using DarkTrails;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DarkTrails.Inventory
{

	public class InventoryAgent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		public Text NameText;
		public Item ItemData;

		public void OnPointerClick(PointerEventData eventData)
		{
			switch (InventoryManager.instance.Mode)
			{
				case InventoryMode.Personal:
					if (ItemData.ItemType == ItemType.Weapon)
					{
						var oldItem = InventoryManager.instance.Character.Equipments[(int)EQUIP.MainHand];
						if (oldItem != null)
						{
							InventoryManager.instance.Inventory.Add(oldItem);
						}

						InventoryManager.instance.Character.Equipments[(int)EQUIP.MainHand] = this.ItemData;
						InventoryManager.instance.Inventory.Remove(this.ItemData);
					}
					else if (ItemData.ItemType == ItemType.Shield)
					{
						var oldItem = InventoryManager.instance.Character.Equipments[(int)EQUIP.OffHand];
						if (oldItem != null)
						{
							InventoryManager.instance.Inventory.Add(oldItem);
						}

						InventoryManager.instance.Character.Equipments[(int)EQUIP.OffHand] = this.ItemData;
						InventoryManager.instance.Inventory.Remove(this.ItemData);
					}
					else if (ItemData.ItemType == ItemType.Armor)
					{
						var oldItem = InventoryManager.instance.Character.Equipments[(int)EQUIP.BodyArmor];
						if (oldItem != null)
						{
							InventoryManager.instance.Inventory.Add(oldItem);
						}

						InventoryManager.instance.Character.Equipments[(int)EQUIP.BodyArmor] = this.ItemData;
						InventoryManager.instance.Inventory.Remove(this.ItemData);
					}
					break;
				case InventoryMode.Trade:
					InventoryManager.instance.Inventory2.Add(this.ItemData);
					InventoryManager.instance.Inventory.Remove(this.ItemData);
					// add money transfer
					break;
				case InventoryMode.Transfer:
					InventoryManager.instance.Inventory2.Add(this.ItemData);
					InventoryManager.instance.Inventory.Remove(this.ItemData);
					break;
				default:
					break;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			//Debug.Log("111" + this.ItemData.ItemName);
		}

		public void OnPointerExit(PointerEventData eventData)
		{

		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				//Debug.Log(this.gameObject.name);
			}
		}

		public void UpdateItemData()
		{
			NameText.text = ItemData.ItemName;
		}

	}
}
