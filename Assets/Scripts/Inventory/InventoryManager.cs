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

		public GameObject InventoryAgentPrefab;
		public GameObject InventoryPanel;
		public GameObject TransferPanel;
		public List<Item> Inventory;
		public List<Item> Inventory2;
		public CharacterData Character;

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
			string filePath = Application.dataPath + "/" + filename;

			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			XmlNode root = doc.SelectSingleNode("ItemList");
			XmlNodeList itemList = root.SelectNodes(".//Item");

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

				GameManager.instance.ItemList.Add(item);

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

		void FillInventoryRandomly()
		{
			Inventory = new List<Item>();
			int invMax = Random.Range(4, 14);
			for (int i = 0; i < invMax; i++)
			{
				var item = GameManager.instance.ItemList[Random.Range(0, GameManager.instance.ItemList.Count-1)];
				Debug.Log(GameManager.instance.ItemList.Count);
				Inventory.Add(item);
			}
		}

		public void ShowInventory()
		{
			/*
			for (int i = 0; i < InventoryPanel.transform.childCount; i++)
			{
				var child = InventoryPanel.transform.GetChild(i);
				Destroy(child);
			}
			*/
			foreach (Transform child in InventoryPanel.transform)
			{
				Destroy(child);
			}

			for (int i = 0; i< Inventory.Count; i++)
			{
				var item = Inventory[i];
				GameObject go = GameObject.Instantiate(InventoryAgentPrefab);
				InventoryAgent itemAgent = go.GetComponent<InventoryAgent>();
				itemAgent.ItemData = item;
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
				var item = Inventory[i];
				if (item.ItemType == (ItemType)filter)
				{
					GameObject go = GameObject.Instantiate(InventoryAgentPrefab);
					InventoryAgent itemAgent = go.GetComponent<InventoryAgent>();
					itemAgent.ItemData = item;
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
				FillInventoryRandomly();
				ShowInventory();
				
			}
		}
	}
}
