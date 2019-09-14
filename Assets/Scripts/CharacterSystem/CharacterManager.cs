using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace DarkTrails.Character
{
    public enum CharacterScreenMode
    {
        Show = 0,
        Create,
        LevelUp
    }

    public class CharacterManager : BaseModule
    {
        private static CharacterManager _instance;
        public static CharacterManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<CharacterManager>();
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

        public List<CharacterData> CharacterList = new List<CharacterData>();
        public CharacterData CurrentCharacter;

        public CharacterScreenMode Mode;

        public UIManager UiManager;

        public GAMEMODULES PreviousModule;

        public int PointsToSpendForStats;
        public int PointsToSpendForSkills;

        private CharacterData _newCharacter;

        // Start is called before the first frame update
        void Start()
        {
            ModuleType = GAMEMODULES.Character;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public override void Initialize(string filename)
        {
            UiManager = GameObject.FindObjectOfType<UIManager>();

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
                    if (equipVal == -1 || equipVal >= Inventory.InventoryManager.instance.ItemList.Count)
                    {
                        //charData.Equipments[equipId] == null;
                    }
                    else
                    {
                        charData.Equipments[equipId] = Inventory.InventoryManager.instance.ItemList[equipVal];
                    }
                }
                CharacterList.Add(charData);
            }

            //toDo: quick fix, needs a character generator module and a party management module
            //CreatePlayerParty();
        }

        public override void Pause()
        {
            base.Pause();
        }

        public override void Resume()
        {
            base.Resume();
        }

        public override void Quit()
        {
            base.Quit();
        }

        public void ShowCharacterInfo()
        {
            if (Mode == CharacterScreenMode.Show)
            {
                UiManager.SetCharacterData(CurrentCharacter);
                PointsToSpendForStats = CurrentCharacter.PointsToSpendForStats;
                PointsToSpendForSkills = CurrentCharacter.PointsToSpendForSkills;
                bool statsVal = false;
                bool skillsVal = false;
                if (PointsToSpendForStats > 0)
                {
                    statsVal = true;
                }
                if (PointsToSpendForSkills > 0)
                {
                    skillsVal = true;
                }

                UiManager.SetEditMode(false, false, statsVal, skillsVal);
            }
            else if (Mode == CharacterScreenMode.Create)
            {
                CurrentCharacter = new CharacterData();
                CurrentCharacter.PointsToSpendForSkills = 50;
                CurrentCharacter.PointsToSpendForStats = 30;
                PointsToSpendForStats = CurrentCharacter.PointsToSpendForStats;
                PointsToSpendForSkills = CurrentCharacter.PointsToSpendForSkills;
                UiManager.SetCharacterData(CurrentCharacter);
                UiManager.SetEditMode(true);
            }

        }

        public void AddCharacterToList()
        {
            this.CharacterList.Add(this.CurrentCharacter);
        }

    }
}