using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.Inventory
{
    public class UIManager : MonoBehaviour
    {

        public GameObject InventoryAgentPrefab;
        public GameObject InventoryPanel;
        public GameObject TransferPanel;
        public GameObject EquipmentPanel;

        public Transform InventoryPanelTransform;

        public EquipmentAgent[] Equipments;

        public Text TooltipText;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowInventory()
        {
            ClearInventory();

            Inventory inventory = InventoryManager.instance.Inventories[InventoryManager.instance.InventoryAId];
            InventoryManager.instance.Inventory = inventory.Items;


            for (int i = 0; i < inventory.Items.Count; i++)
            {
                var item = InventoryManager.instance.ItemList[inventory.Items[i].ItemId];
                GameObject go = GameObject.Instantiate(InventoryAgentPrefab);
                InventoryAgent itemAgent = go.GetComponent<InventoryAgent>();
                itemAgent.ItemData = item;
                itemAgent.InventoryId = i;
                itemAgent.UpdateItemData();

                go.transform.SetParent(InventoryPanelTransform);
            }

            UpdateEquipments();
        }

        public void ClearInventory()
        {
            foreach (Transform child in InventoryPanelTransform)
            {
                Destroy(child.gameObject);
            }
        }

        public void ShowFilteredInventory(int filter)
        {
            ClearInventory();

            for (int i = 0; i < InventoryManager.instance.Inventory.Count; i++)
            {
                var item = InventoryManager.instance.ItemList[InventoryManager.instance.Inventory[i].ItemId];
                if (item.ItemType == (ItemType)filter)
                {
                    GameObject go = GameObject.Instantiate(InventoryAgentPrefab);
                    InventoryAgent itemAgent = go.GetComponent<InventoryAgent>();
                    itemAgent.ItemData = item;
                    itemAgent.InventoryId = i;
                    itemAgent.UpdateItemData();

                    go.transform.SetParent(InventoryPanelTransform);
                }
            }
        }

        public void UpdateEquipments()
        {
            for(int i = 0; i <  Equipments.Length; i++)
            {
                Equipments[i].ItemData = Character.CharacterManager.instance.CharacterList[InventoryManager.instance.CharacterAId].Equipments[i];
                Equipments[i].UpdateItemData();
            }
        }

        public void CloseUI()
        {
            switch (InventoryManager.instance.PreviousModule)
            {
                case GAMEMODULES.OverWorld:
                    GameManager.instance.OpenOverWorld("");
                    break;
                case GAMEMODULES.Travel:
                    GameManager.instance.OpenTravel("");
                    break;
                case GAMEMODULES.Dialogue:
                    GameManager.instance.OpenDialogue("");
                    break;
                case GAMEMODULES.Combat:
                    GameManager.instance.OpenCombat("");
                    break;
                default:
                    GameManager.instance.ChangeModule(InventoryManager.instance.PreviousModule);
                    break;
            }
        }

    }
}
