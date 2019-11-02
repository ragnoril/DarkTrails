using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DarkTrails.Inventory
{
	public class EquipmentAgent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
		public Text NameText;
		public Item ItemData;
        public Character.EQUIP EquipType;

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
            if (ItemData != null)
                NameText.text = ItemData.ItemName;
            else
                NameText.text = "";
		}

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ItemData != null)
                InventoryManager.instance.UI.TooltipText.text = this.ItemData.ItemName;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            InventoryManager.instance.UI.TooltipText.text = "";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Character.CharacterData characterA = Character.CharacterManager.instance.CharacterList[InventoryManager.instance.CharacterAId];
            Inventory inventoryA = InventoryManager.instance.Inventories[InventoryManager.instance.InventoryAId];

            if (ItemData != null)
            {
                var slot = new InventorySlot();
                slot.ItemId = InventoryManager.instance.ItemList.IndexOf(this.ItemData);
                slot.Amount = 1;
                inventoryA.Items.Add(slot);

                characterA.Equipments[(int)EquipType] = null;
            }

            InventoryManager.instance.UI.ShowInventory();
        }
    }
}
