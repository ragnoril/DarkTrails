using System.Collections;
using System.Collections.Generic;
using System.Xml;
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
		public Dictionary<string, Combat.EncounterData> EncounterList = new Dictionary<string, Combat.EncounterData>();
		public Dictionary<string, string> MapList = new Dictionary<string, string>();

		public int PlayerCharacterId;
		public List<int> PlayerParty = new List<int>();

		public GameObject[] ModulePrefabs;
		public List<BaseModule> GameModules;
		public BaseModule CurrentGameModule;

		// Use this for initialization
		void Start()
		{
			LoadGameData();
			//SceneManager.LoadScene("CharGen");
		}

		public void LoadGameData()
		{
			string filePath = Application.dataPath + "/GameData.xml";
			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			XmlNode root = doc.SelectSingleNode("Game");
			XmlNodeList moduleList = root.SelectNodes(".//Module");

			string startModule = root.Attributes["startModule"].Value;
			string startValue = root.Attributes["startValue"].Value;

			foreach (XmlNode module in moduleList)
			{
				string moduleType = module.Attributes["type"].Value;
				string modulefile = module.Attributes["filename"].Value;
				
				if (moduleType == "Combat")
				{
					GameObject prefab = null;
					foreach(var modulePrefab in ModulePrefabs)
					{
						if (modulePrefab.GetComponent<Combat.CombatManager>() != null)
						{
							prefab = modulePrefab;
							break;
						}
					}

					var go = GameObject.Instantiate(prefab);
					go.transform.SetParent(this.transform);
					Combat.CombatManager combat = go.GetComponent<Combat.CombatManager>();
					combat.Initialize(modulefile);
					GameModules.Add(combat);
					combat.Pause();
					go.SetActive(false);
				}
				else if (moduleType == "Dialogue")
				{
					GameObject prefab = null;
					foreach (var modulePrefab in ModulePrefabs)
					{
						if (modulePrefab.GetComponent<Dialogue.DialogueManager>() != null)
						{
							prefab = modulePrefab;
							break;
						}
					}

					var go = GameObject.Instantiate(prefab);
					go.transform.SetParent(this.transform);
					Dialogue.DialogueManager dialog = go.GetComponent<Dialogue.DialogueManager>();
					dialog.Initialize(modulefile);
					GameModules.Add(dialog);
					dialog.Pause();
					go.SetActive(false);
				}
				else if (moduleType == "Inventory")
				{
					GameObject prefab = null;
					foreach (var modulePrefab in ModulePrefabs)
					{
						if (modulePrefab.GetComponent<Inventory.InventoryManager>() != null)
						{
							prefab = modulePrefab;
							break;
						}
					}

					var go = GameObject.Instantiate(prefab);
					go.transform.SetParent(this.transform);
					Inventory.InventoryManager inventory = go.GetComponent<Inventory.InventoryManager>();
					inventory.Initialize(modulefile);
					GameModules.Add(inventory);
					inventory.Pause();
					go.SetActive(false);
				}
				else if (moduleType == "Character")
				{
					LoadCharacterList(modulefile);
				}
				else if (moduleType == "Travel")
				{
					GameObject prefab = null;
					foreach (var modulePrefab in ModulePrefabs)
					{
						if (modulePrefab.GetComponent<Travel.TravelManager>() != null)
						{
							prefab = modulePrefab;
							break;
						}
					}

					var go = GameObject.Instantiate(prefab);
					go.transform.SetParent(this.transform);
					Travel.TravelManager travel = go.GetComponent<Travel.TravelManager>();
					travel.Initialize(modulefile);
					GameModules.Add(travel);
					travel.Pause();
					go.SetActive(false);
				}
			}

			switch(startModule)
			{
				case "Combat":
					OpenCombat(startValue);
					break;
				case "Travel":
					OpenTravel(startValue);
					break;
				case "Inventory":
					break;
				case "Dialogue":
					OpenDialogue(startValue);
					break;
			}
		}

		#region old stuff waiting to be removed

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

			//toDo: quick fix, needs a character generator module and a party management module
			GameManager.instance.PlayerCharacterId = GameManager.instance.CharacterList.Count - 1;
			GameManager.instance.PlayerParty.Add(GameManager.instance.PlayerCharacterId);
		}
		
		#endregion
		
		public void ChangeModule(GAMEMODULES module)
		{
			foreach(var gameModule in GameModules)
			{
				if (gameModule.ModuleType == module)
				{
					if (CurrentGameModule != null)
					{
						CurrentGameModule.Pause();
						CurrentGameModule.gameObject.SetActive(false);
					}
					gameModule.gameObject.SetActive(true);
					gameModule.Resume();
					CurrentGameModule = gameModule;
				}
			}
		}

		public void OpenCombat(string encounterName)
		{
			ChangeModule(GAMEMODULES.Combat);
			Combat.CombatManager.instance.EncounterName = encounterName;
			Combat.CombatManager.instance.StartTheGame();
		}

		public void OpenDialogue(string dialogName)
		{
			ChangeModule(GAMEMODULES.Dialogue);
			Dialogue.DialogueManager.instance.DialogueStartNode = dialogName;
			Dialogue.DialogueManager.instance.StartDialogue();
		}

		public void OpenInventory(Inventory.InventoryMode mode)
		{
			ChangeModule(GAMEMODULES.Inventory);
			Inventory.InventoryManager.instance.Mode = mode;
		}

		public void OpenTravel(string mapName)
		{
			ChangeModule(GAMEMODULES.Travel);
			Travel.TravelManager.instance.LoadMap(mapName);
		}

	}
}
