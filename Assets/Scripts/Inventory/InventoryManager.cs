using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace DarkTrails.Inventory
{
	public enum InventoryMode
	{
		Personal = 0,
		Trade,
		Transfer
	}

	public class InventoryManager : BaseModule
	{
		private static InventoryManager _instance;
		public static InventoryManager instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = GameObject.FindObjectOfType<InventoryManager>();
					//DontDestroyOnLoad(_instance.gameObject);
				}

				return _instance;
			}

		}

		void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
				//DontDestroyOnLoad(this);
			}
			else
			{
				if (this != _instance)
					Destroy(this.gameObject);
			}
		}

        public List<Item> ItemList = new List<Item>();

        public GameObject InventoryAgentPrefab;
		public GameObject InventoryPanel;
		public GameObject TransferPanel;

        public int CharacterAId;
        public int CharacterBId;
        public int InventoryAId;
        public int InventoryBId;

        public List<InventorySlot> Inventory;
		public List<InventorySlot> Inventory2;
		public Character.CharacterData Character;
        public Character.CharacterData Character2;

        public List<Inventory> Inventories;

        public InventoryMode Mode;
		// selected item
		// envanter 1
		// envanter 2
		// equipments
		// trading?
		// withmoney?


		// Use this for initialization
		void Start()
		{
			ModuleType = GAMEMODULES.Inventory;
		}

		public override void Initialize(string filename)
		{
            Inventories = new List<Inventory>();

            string filePath = Application.dataPath + "/" + filename;

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode root = doc.SelectSingleNode("Items");
            XmlNodeList itemList = root.SelectSingleNode(".//ItemList").SelectNodes(".//Item");

            foreach (XmlNode itm in itemList)
            {
                Item item = new Item();
                string itemtype = itm.Attributes["Type"].Value;
                if (itemtype == "Weapon")
                {
                    item = new Weapon();
                    ((Weapon)item).ItemName = itm.Attributes["Name"].Value;
                    ((Weapon)item).PriceValue = int.Parse(itm.Attributes["PriceValue"].Value);
                    ((Weapon)item).MinDamage = int.Parse(itm.Attributes["MinDamage"].Value);
                    ((Weapon)item).MaxDamage = int.Parse(itm.Attributes["MaxDamage"].Value);
                    ((Weapon)item).WeaponReach = int.Parse(itm.Attributes["WeaponReach"].Value);
                    ((Weapon)item).WeaponRange = int.Parse(itm.Attributes["WeaponRange"].Value);
                    ((Weapon)item).WeaponType = int.Parse(itm.Attributes["WeaponType"].Value);
                    int twoHanded = int.Parse(itm.Attributes["TwoHanded"].Value);
                    if (twoHanded == 0)
                    {
                        ((Weapon)item).IsTwoHanded = false;
                    }
                    else
                    {
                        ((Weapon)item).IsTwoHanded = true;
                    }
                    ((Weapon)item).ItemType = ItemType.Weapon;
                }
                else if (itemtype == "Armor")
                {
                    item = new Armor();
                    ((Armor)item).ItemName = itm.Attributes["Name"].Value;
                    ((Armor)item).PriceValue = int.Parse(itm.Attributes["PriceValue"].Value);
                    ((Armor)item).ArmorValue = int.Parse(itm.Attributes["DamageResistance"].Value);
                    ((Armor)item).ArmorIndex = int.Parse(itm.Attributes["ArmorIndex"].Value);
                    ((Armor)item).ItemType = ItemType.Armor;
                }
                else if (itemtype == "Shield")
                {
                    item = new Shield();
                    ((Shield)item).ItemName = itm.Attributes["Name"].Value;
                    ((Shield)item).PriceValue = int.Parse(itm.Attributes["PriceValue"].Value);
                    ((Shield)item).ArmorValue = int.Parse(itm.Attributes["DamageResistance"].Value);
                    ((Shield)item).ArmorIndex = int.Parse(itm.Attributes["ArmorIndex"].Value);
                    ((Shield)item).ItemType = ItemType.Shield;
                }
                else
                {
                    item.ItemName = "Unidentified Item";
                }

                ItemList.Add(item);

            }

            XmlNodeList inventoryList = root.SelectSingleNode(".//InventoryList").SelectNodes(".//Inventory");

            foreach(XmlNode inv in inventoryList)
            {
                var slots = inv.SelectNodes(".//InventorySlot");

                Inventory inventory = new Inventory();

                inventory.Name = inv.Attributes["Name"].Value;
                inventory.Items = new List<InventorySlot>();

                foreach(XmlNode slot in slots)
                {
                    InventorySlot invSlot = new InventorySlot();

                    invSlot.Amount = int.Parse(slot.Attributes["Amount"].Value);
                    invSlot.ItemId = int.Parse(slot.Attributes["ItemId"].Value);

                    inventory.Items.Add(invSlot);
                }

                Inventories.Add(inventory);
            }
        }

        public override void Pause()
		{
			base.Pause();
		}

		public override void Resume()
		{
			base.Resume();
			ShowInventory();
		}

		public override void Quit()
		{
			base.Quit();
		}

		public void ShowInventory()
		{
			foreach (Transform child in InventoryPanel.transform)
			{
				Destroy(child);
			}

            Inventory inventory = Inventories[InventoryAId];

			for (int i = 0; i< inventory.Items.Count; i++)
			{
				var item = ItemList[inventory.Items[i].ItemId];
				GameObject go = GameObject.Instantiate(InventoryAgentPrefab);
				InventoryAgent itemAgent = go.GetComponent<InventoryAgent>();
				itemAgent.ItemData = item;
                itemAgent.InventoryId = i;
				itemAgent.UpdateItemData();

				go.transform.SetParent(InventoryPanel.transform);
			}
        }

		public void ShowFilteredInventory(int filter)
		{
			foreach (Transform child in InventoryPanel.transform)
			{
				Destroy(child.gameObject);
			}
			for (int i = 0; i < Inventory.Count; i++)
			{
				var item = ItemList[Inventory[i].ItemId];
				if (item.ItemType == (ItemType)filter)
				{
					GameObject go = GameObject.Instantiate(InventoryAgentPrefab);
					InventoryAgent itemAgent = go.GetComponent<InventoryAgent>();
					itemAgent.ItemData = item;
                    itemAgent.InventoryId = i;
					itemAgent.UpdateItemData();

					go.transform.SetParent(InventoryPanel.transform);
				}
			}
		}

		bool isLoaded = false;
		// Update is called once per frame
		void Update()
		{
			if (!isLoaded)
			{
				isLoaded = true;
				//FillInventoryRandomly();
				ShowInventory();
				
			}
		}

        public void SetA(int character, int inventory)
        {
            isLoaded = false;

            CharacterAId = character;
            InventoryAId = inventory;
        }

        public void SetB(int character, int inventory)
        {
            isLoaded = false;

            CharacterAId = character;
            InventoryAId = inventory;
        }
    }
}
