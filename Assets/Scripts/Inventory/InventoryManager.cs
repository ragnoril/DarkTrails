using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails.Inventory
{
	public class InventoryManager : MonoBehaviour
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
		public List<Item> Inventory;
		public CharacterData Character;
		// selected item
		// envanter 1
		// envanter 2
		// equipments
		// trading?
		// withmoney?


		// Use this for initialization
		void Start()
		{
			
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
