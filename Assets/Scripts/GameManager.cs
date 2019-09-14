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

		//public List<Character.CharacterData> CharacterList = new List<Character.CharacterData>();
		public List<Item> ItemList = new List<Item>();
		public Dictionary<string, Combat.EncounterData> EncounterList = new Dictionary<string, Combat.EncounterData>();
		public Dictionary<string, string> MapList = new Dictionary<string, string>();
        public Dictionary<string, string> SceneList = new Dictionary<string, string>();

        public int PlayerCharacterId;
		public List<int> PlayerParty = new List<int>();
        public int PlayerInventoryId;

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
                    GameObject prefab = null;
                    foreach (var modulePrefab in ModulePrefabs)
                    {
                        if (modulePrefab.GetComponent<Character.CharacterManager>() != null)
                        {
                            prefab = modulePrefab;
                            break;
                        }
                    }

                    var go = GameObject.Instantiate(prefab);
                    go.transform.SetParent(this.transform);
                    Character.CharacterManager charSheet = go.GetComponent<Character.CharacterManager>();
                    charSheet.Initialize(modulefile);
                    GameModules.Add(charSheet);
                    charSheet.Pause();
                    go.SetActive(false);

                    // toDo: this is a quickfix.
                    CreatePlayerParty();

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
                    travel.TravelMode = Travel.TRAVELMODES.PixelBased;
					travel.Pause();
					go.SetActive(false);
				}
                else if (moduleType == "OverWorld")
                {
                    GameObject prefab = null;
                    foreach (var modulePrefab in ModulePrefabs)
                    {
                        if (modulePrefab.GetComponent<OverWorld.OverWorldManager>() != null)
                        {
                            prefab = modulePrefab;
                            break;
                        }
                    }

                    var go = GameObject.Instantiate(prefab);
                    go.transform.SetParent(this.transform);
                    OverWorld.OverWorldManager overWorld = go.GetComponent<OverWorld.OverWorldManager>();
                    overWorld.Initialize(modulefile);
                    GameModules.Add(overWorld);

                    overWorld.Pause();
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
                case "OverWorld":
                    OpenOverWorld(startValue);
                    break;
			}
		}

		#region old stuff waiting to be removed

        //todo: quick fix here too. 
        public void CreatePlayerParty()
        {
            GameManager.instance.PlayerCharacterId = Character.CharacterManager.instance.CharacterList.Count - 1;
            GameManager.instance.PlayerParty.Add(GameManager.instance.PlayerCharacterId);

            GameManager.instance.PlayerParty.Add(1);
            GameManager.instance.PlayerParty.Add(1);
            GameManager.instance.PlayerParty.Add(1);

            GameManager.instance.PlayerInventoryId = 0;

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
            if (encounterName != "")
            {
                Combat.CombatManager.instance.EncounterName = encounterName;
                Combat.CombatManager.instance.StartTheGame();
            }
		}

		public void OpenDialogue(string dialogueName)
		{
			ChangeModule(GAMEMODULES.Dialogue);
            if (dialogueName != "")
                Dialogue.DialogueManager.instance.DialogueStartNode = dialogueName;
            Dialogue.DialogueManager.instance.ContinueDialogue();
		}

		public void OpenInventory(Inventory.InventoryMode mode)
		{
			ChangeModule(GAMEMODULES.Inventory);
			Inventory.InventoryManager.instance.Mode = mode;
		}

		public void OpenTravel(string mapName)
		{
			ChangeModule(GAMEMODULES.Travel);
            if (mapName != "")
                Travel.TravelManager.instance.LoadMap(mapName);
		}

        public void OpenOverWorld(string sceneName)
        {
            ChangeModule(GAMEMODULES.OverWorld);
            if (sceneName != "")
                OverWorld.OverWorldManager.instance.LoadScene(sceneName);
        }

        public void OpenCharacter(Character.CharacterScreenMode mode, Character.CharacterData character)
        {
            Character.CharacterManager.instance.PreviousModule = CurrentGameModule.ModuleType;
            ChangeModule(GAMEMODULES.Character);
            Character.CharacterManager.instance.Mode = mode;
            Character.CharacterManager.instance.CurrentCharacter = character;
            Character.CharacterManager.instance.ShowCharacterInfo();

        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("MenuScene");
        }

	}
}
