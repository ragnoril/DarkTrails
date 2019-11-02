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
        public int InventoryId;

		public void OnPointerClick(PointerEventData eventData)
		{
            Character.CharacterData characterA = Character.CharacterManager.instance.CharacterList[InventoryManager.instance.CharacterAId];
            Inventory inventoryA = InventoryManager.instance.Inventories[InventoryManager.instance.InventoryAId];

            switch (InventoryManager.instance.Mode)
			{
				case InventoryMode.Personal:
					if (ItemData.ItemType == ItemType.Weapon)
					{
						var oldItem = characterA.Equipments[(int)Character.EQUIP.MainHand];
						if (oldItem != null)
						{
                            var slot = new InventorySlot();
                            slot.ItemId = InventoryManager.instance.ItemList.IndexOf(oldItem);
                            slot.Amount = 1;
                            inventoryA.Items.Add(slot);
						}

						characterA.Equipments[(int)Character.EQUIP.MainHand] = this.ItemData;
                        inventoryA.Items.RemoveAt(this.InventoryId);
					}
					else if (ItemData.ItemType == ItemType.Shield)
					{
						var oldItem = characterA.Equipments[(int)Character.EQUIP.OffHand];
						if (oldItem != null)
						{
                            var slot = new InventorySlot();
                            slot.ItemId = InventoryManager.instance.ItemList.IndexOf(oldItem);
                            slot.Amount = 1;
                            inventoryA.Items.Add(slot);
                        }

                        characterA.Equipments[(int)Character.EQUIP.OffHand] = this.ItemData;
                        inventoryA.Items.RemoveAt(this.InventoryId);
					}
					else if (ItemData.ItemType == ItemType.BodyArmor)
					{
						var oldItem = characterA.Equipments[(int)Character.EQUIP.BodyArmor];
						if (oldItem != null)
						{
                            var slot = new InventorySlot();
                            slot.ItemId = InventoryManager.instance.ItemList.IndexOf(oldItem);
                            slot.Amount = 1;
                            inventoryA.Items.Add(slot);
                        }

                        characterA.Equipments[(int)Character.EQUIP.BodyArmor] = this.ItemData;
                        inventoryA.Items.RemoveAt(this.InventoryId);
                    }
                    else if (ItemData.ItemType == ItemType.Boots)
                    {
                        var oldItem = characterA.Equipments[(int)Character.EQUIP.Boots];
                        if (oldItem != null)
                        {
                            var slot = new InventorySlot();
                            slot.ItemId = InventoryManager.instance.ItemList.IndexOf(oldItem);
                            slot.Amount = 1;
                            inventoryA.Items.Add(slot);
                        }

                        characterA.Equipments[(int)Character.EQUIP.Boots] = this.ItemData;
                        inventoryA.Items.RemoveAt(this.InventoryId);
                    }
                    else if (ItemData.ItemType == ItemType.Helmet)
                    {
                        var oldItem = characterA.Equipments[(int)Character.EQUIP.Helmet];
                        if (oldItem != null)
                        {
                            var slot = new InventorySlot();
                            slot.ItemId = InventoryManager.instance.ItemList.IndexOf(oldItem);
                            slot.Amount = 1;
                            inventoryA.Items.Add(slot);
                        }

                        characterA.Equipments[(int)Character.EQUIP.Helmet] = this.ItemData;
                        inventoryA.Items.RemoveAt(this.InventoryId);
                    }
                    break;
				case InventoryMode.Trade:

                    //InventoryManager.instance.Inventory2.Add(this.ItemData);
					//InventoryManager.instance.Inventory.Remove(this.ItemData);
					// add money transfer
					break;
				case InventoryMode.Transfer:
					//InventoryManager.instance.Inventory2.Add(this.ItemData);
					//InventoryManager.instance.Inventory.Remove(this.ItemData);
					break;
				default:
					break;
			}

            InventoryManager.instance.UI.ShowInventory();
            
        }

		public void OnPointerEnter(PointerEventData eventData)
		{
            //Debug.Log("111" + this.ItemData.ItemName);
            InventoryManager.instance.UI.TooltipText.text = this.ItemData.ItemName;
        }

		public void OnPointerExit(PointerEventData eventData)
		{
            InventoryManager.instance.UI.TooltipText.text = "";
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
