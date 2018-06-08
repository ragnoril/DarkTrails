using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkTrails
{

	public class GameManager : MonoBehaviour
	{
		private static GameManager _instance;

		public static GameManager instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = GameObject.FindObjectOfType<GameManager>();
					DontDestroyOnLoad(_instance.gameObject);
				}

				return _instance;
			}

		}

		void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
				DontDestroyOnLoad(this);
			}
			else
			{
				if (this != _instance)
					Destroy(this.gameObject);
			}
		}

		public List<CharacterData> CharacterList = new List<CharacterData>();
		public List<Item> ItemList = new List<Item>();
		public Dictionary<string, int[]> EncounterList = new Dictionary<string, int[]>();
		public string DialogueFile = "";
		public string DialogueStartNode = "Start";
		public int PlayerCharacterId;
		public List<int> PlayerParty;

		// Use this for initialization
		void Start()
		{
			PlayerParty = new List<int>();
			LoadGameData();
			SceneManager.LoadScene("CharGen");
		}

		public void LoadGameData()
		{
			string filePath = Application.dataPath + "/GameData.xml";
			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			XmlNode root = doc.SelectSingleNode("Game");
			XmlNode items = root.SelectSingleNode(".//Items");
			string itemListFile = items.Attributes["filename"].Value;
			LoadItemList(itemListFile);

			XmlNode characters = root.SelectSingleNode(".//Characters");
			string charListFile = characters.Attributes["filename"].Value;
			LoadCharacterList(charListFile);

			XmlNode encounters = root.SelectSingleNode(".//Encounters");
			string encountersFile = encounters.Attributes["filename"].Value;
			LoadEncounters(encountersFile);

			XmlNode dialogue = root.SelectSingleNode(".//Dialogue");
			string dialogueFile = dialogue.Attributes["filename"].Value;
			string dialogueStart = dialogue.Attributes["startNode"].Value;
			LoadDialogue(dialogueFile, dialogueStart);
			
		}

		public void LoadEncounters(string filename)
		{
			string filePath = Application.dataPath + "/" + filename;

			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			XmlNode root = doc.SelectSingleNode("Encounters");
			XmlNodeList encounterList = root.SelectNodes(".//Encounter");

			foreach (XmlNode enc in encounterList)
			{
				string encounterName = enc.Attributes["name"].Value;
				XmlNodeList chrList = enc.SelectNodes(".//Character");
				int[] encounterChrList = new int[chrList.Count];
				for (int i = 0; i < chrList.Count; i++)
				{
					XmlNode chr = chrList.Item(i);
					int chrid = int.Parse(chr.Attributes["id"].Value);
					encounterChrList[i] = chrid;
				}
				EncounterList.Add(encounterName, encounterChrList);
			}
		}

		public void LoadDialogue(string filename, string start)
		{
			DialogueFile = filename;
			DialogueStartNode = start;
		}

		public void LoadItemList(string filename)
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

				ItemList.Add(item);

			}
		}

		public void LoadCharacterList(string filename)
		{
			string filePath = Application.dataPath + "/" + filename;

			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			XmlNode root = doc.SelectSingleNode("CharacterList");
			XmlNodeList charList = root.SelectNodes(".//Character");

			foreach (XmlNode chr in charList)
			{
				CharacterData charData = new CharacterData();
				charData.Name = chr.Attributes["name"].Value;
				charData.Level = int.Parse(chr.Attributes["level"].Value);

				XmlNode statRoot = chr.SelectSingleNode(".//Stats");
				XmlNodeList statList = statRoot.SelectNodes(".//Stat");
				foreach (XmlNode stat in statList)
				{
					int statId = int.Parse(stat.Attributes["id"].Value);
					charData.Stats[statId] = int.Parse(stat.Attributes["value"].Value);
				}

				XmlNode skillRoot = chr.SelectSingleNode(".//Skills");
				XmlNodeList skillList = skillRoot.SelectNodes(".//Skill");
				foreach (XmlNode skill in skillList)
				{
					int skillId = int.Parse(skill.Attributes["id"].Value);
					charData.Skills[skillId] = int.Parse(skill.Attributes["value"].Value);
				}

				XmlNode equipRoot = chr.SelectSingleNode(".//Equipments");
				XmlNodeList equipList = equipRoot.SelectNodes(".//Equipment");
				foreach (XmlNode equip in equipList)
				{
					int equipId = int.Parse(equip.Attributes["id"].Value);
					int equipVal = int.Parse(equip.Attributes["value"].Value);
					if (equipVal == -1 || equipVal >= ItemList.Count)
					{
						//charData.Equipments[equipId] == null;
					}
					else
					{
						charData.Equipments[equipId] = ItemList[equipVal];
					}
				}

				CharacterList.Add(charData);

			}
		}

		public void StartDialogue()
		{
			SceneManager.LoadScene("DialogScene");
		}

		public string EncounterName;
		public string WinDialogue;
		public string LoseDialogue;

		public void StartCombat(string encounterName, string winDialogue, string loseDialogue)
		{
			EncounterName = encounterName;
			WinDialogue = winDialogue;
			LoseDialogue = loseDialogue;
			SceneManager.LoadScene("CombatScene");
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}
