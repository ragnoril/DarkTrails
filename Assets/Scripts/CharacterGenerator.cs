using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DarkTrails;
using System.Collections.Generic;

namespace DarkTrails.UI
{
    public enum CHARGENSTAGE
    {
        Stats = 0,
        Skills,
        Traits,
		Items,
        Naming,
        StageCount
    };

    public class CharacterGenerator : PointsBaseWindow
    {
        public CHARGENSTAGE stage;
        public GameObject[] stagePanels;

        public CharacterData character;

        public int StatPointsToSpent;
        public int SkillPointsToSpent;

        public StatsUIObject[] Stats; //= new StatsUIObject[(int)STATS.StatCount];
        public StatsUIObject[] Skills; //= new StatsUIObject[(int)SKILLS.SkillCount];

        public Text StatPointsLeftText;
        public Text SkillPointsLeftText;
        public Text TitleText;

        public Text LevelText;
        public Text HpText;
        public Text ApText;
        public Text InitText;
        public Text HrText;
        public Text MdText;
        public Text HpPerLvlText;
        public Text SkillPerLvlText;
        public InputField NameField;

		public Dropdown MainHandDropdown;
		public Dropdown OffHandDropdown;
		public Dropdown BodyArmorDropdown;
		public Dropdown HelmetDropdown;
		public Dropdown BootsDropdown;


		void Awake()
        {
            
        }

        // Use this for initialization
        void Start()
        {
            character = new CharacterData();
            character.Level = 1;
            PointsHave = StatPointsToSpent;

            StatPointsLeftText.text = (PointsHave - PointsSpent).ToString();
            //SkillPointsLeftText.text = (SkillPointsHave - SkillPointsSpent).ToString();

            foreach(var obj in Stats)
            {
                obj.PointsCalc = this;
            }
            foreach (var obj in Skills)
            {
                obj.PointsCalc = this;
            }

            UpdateStats();
			UpdateEquipments();
        }

        // Update is called once per frame
        void Update()
        {


        }

		public void UpdateEquipments()
		{
			List<string> mainHand = new List<string>();
			List<string> offHand = new List<string>();
			List<string> bodyArmor = new List<string>();
			List<string> helmet = new List<string>();
			List<string> boots = new List<string>();

			foreach (Item item in GameManager.instance.ItemList)
			{
				if (item.ItemType == ItemType.Weapon)
				{
					mainHand.Add(item.ItemName);
				}
				else if (item.ItemType == ItemType.Armor)
				{
					if (((Armor)item).ArmorIndex == 0)
					{
						bodyArmor.Add(item.ItemName);
					}
					else if (((Armor)item).ArmorIndex == 1)
					{
						helmet.Add(item.ItemName);
					}
					else if (((Armor)item).ArmorIndex == 2)
					{
						boots.Add(item.ItemName);
					}
				}
				else if (item.ItemType == ItemType.Shield)
				{
					offHand.Add(item.ItemName);
				}

			}

			MainHandDropdown.AddOptions(mainHand);
			OffHandDropdown.AddOptions(offHand);
			BodyArmorDropdown.AddOptions(bodyArmor);
			HelmetDropdown.AddOptions(helmet);
			BootsDropdown.AddOptions(boots);
		}

		public void CalculateItems()
		{
			int mainHandId = GetItemId(MainHandDropdown.options[MainHandDropdown.value].text);
			int offHandId = GetItemId(OffHandDropdown.options[OffHandDropdown.value].text);
			int bodyArmorId = GetItemId(BodyArmorDropdown.options[BodyArmorDropdown.value].text);
			int helmetId = GetItemId(HelmetDropdown.options[HelmetDropdown.value].text);
			int bootsId = GetItemId(BootsDropdown.options[BootsDropdown.value].text);
			
			character.Equipments[(int)EQUIP.MainHand] = GameManager.instance.ItemList[mainHandId];
			character.Equipments[(int)EQUIP.OffHand] = GameManager.instance.ItemList[mainHandId];
			character.Equipments[(int)EQUIP.BodyArmor] = GameManager.instance.ItemList[bodyArmorId];
			character.Equipments[(int)EQUIP.Boots] = GameManager.instance.ItemList[bootsId];
			character.Equipments[(int)EQUIP.Helmet] = GameManager.instance.ItemList[helmetId];
		}

		public int GetItemId(string itemName)
		{
			for(int i = 0; i < GameManager.instance.ItemList.Count; i++)
			{
				Item item = GameManager.instance.ItemList[i];
				if (item.ItemName == itemName)
				{
					return i;
				}
			}

			return -1;
		}

        public override void UpdateStats()
        {
            if (stage == CHARGENSTAGE.Stats)
            {
                for (int i = 0; i < (int)STATS.StatCount; i++)
                {
                    character.Stats[i] = Stats[i].Value;
                }

                LevelText.text = "Level: " + character.Level.ToString();
                HpText.text = "HP: " + character.MaxHitPoints.ToString();
                ApText.text = "AP: " + character.MaxActionPoints.ToString();
                InitText.text = "Init: " + character.Initiative.ToString();
                HrText.text = "HR: " + character.HealingRate.ToString();
                MdText.text = "MD: " + character.BaseDamage.ToString();
                HpPerLvlText.text = "HpPerLvl: " + character.HitPointsPerLevel.ToString();
                SkillPerLvlText.text = "SpPerLvl: " + character.SkillPointsPerLevel.ToString();

                StatPointsLeftText.text = (PointsHave - PointsSpent).ToString();
            }
            else if (stage == CHARGENSTAGE.Skills)
            {
                PointsHave = SkillPointsToSpent;
                SkillPointsLeftText.text = (PointsHave - PointsSpent).ToString();
            }
        }

        public void CalculateSkills()
        {
            SkillPointsToSpent = 10 + character.Stats[(int)STATS.Intelligence] + (character.SkillPointsPerLevel * character.Level);
            PointsHave = SkillPointsToSpent;

            character.Skills[(int)SKILLS.Melee] = (character.Stats[(int)STATS.Strength] * 2) + character.Stats[(int)STATS.Agility];
            character.Skills[(int)SKILLS.Ranged] = (character.Stats[(int)STATS.Agility] * 2) + character.Stats[(int)STATS.Strength];
            character.Skills[(int)SKILLS.Dodge] = (character.Stats[(int)STATS.Agility] * 3);
            character.Skills[(int)SKILLS.Shield] = (character.Stats[(int)STATS.Strength] + character.Stats[(int)STATS.Agility]) * 2;
            character.Skills[(int)SKILLS.Mysticism] = (character.Stats[(int)STATS.Intelligence] + character.Stats[(int)STATS.Willpower]) * 2;
            character.Skills[(int)SKILLS.Crafting] = (character.Stats[(int)STATS.Intelligence] * 4);
            character.Skills[(int)SKILLS.Leadership] = (character.Stats[(int)STATS.Willpower] * 2) + character.Stats[(int)STATS.Intelligence];
            character.Skills[(int)SKILLS.Herbalism] = (character.Stats[(int)STATS.Intelligence] * 3);
            character.Skills[(int)SKILLS.Healing] = (character.Stats[(int)STATS.Intelligence] + character.Stats[(int)STATS.Agility]);
            character.Skills[(int)SKILLS.Persuasion] = (character.Stats[(int)STATS.Intelligence] * 2) + character.Stats[(int)STATS.Willpower];
            character.Skills[(int)SKILLS.Trading] = (character.Stats[(int)STATS.Intelligence] * 3) + character.Stats[(int)STATS.Willpower];
            character.Skills[(int)SKILLS.NatureLore] = character.Stats[(int)STATS.Strength] + character.Stats[(int)STATS.Agility] + character.Stats[(int)STATS.Endurance] + character.Stats[(int)STATS.Willpower] + character.Stats[(int)STATS.Willpower];

            for (int i = 0; i < (int)SKILLS.SkillCount; i++)
            {
                Skills[i].Value = character.Skills[i];
            }

        }

        public void UpdateSkills()
        {

            for (int i = 0; i < (int)SKILLS.SkillCount; i++)
            {
                character.Skills[i] = Skills[i].Value;
            }
            
        }

        public void ChangeLevel(int change)
        {
            character.Level += change;

            SkillPointsToSpent = 10 + character.Stats[(int)STATS.Intelligence] + (character.SkillPointsPerLevel * (character.Level - 1));
            //PointsHave = SkillPointsToSpent;

            UpdateStats();
        }

        public void NextStage()
        {
            stagePanels[(int)stage].SetActive(false);
            if (stage == CHARGENSTAGE.Stats)
            {
                //stats finished
                PointsSpent = 0;
                stage = CHARGENSTAGE.Skills;
                CalculateSkills();

            }
            else if (stage == CHARGENSTAGE.Skills)
            {
                //skills finished
                stage = CHARGENSTAGE.Items;
                UpdateSkills();
            }
            else if (stage == CHARGENSTAGE.Traits)
            {
                //traits finished
                stage = CHARGENSTAGE.Items;

            }
			else if (stage == CHARGENSTAGE.Items)
			{
				//items finished
				stage = CHARGENSTAGE.Naming;
				CalculateItems();
			}
			else if (stage == CHARGENSTAGE.Naming)
            {
                // naming finished
                // chargen ended.
                character.Name = NameField.text;
				//BattleManager.instance.CharacterList.Add(character);
				//UnityEngine.SceneManagement.SceneManager.LoadScene("BattleMenu");
				GameManager.instance.CharacterList.Add(character);
				GameManager.instance.PlayerCharacterId = GameManager.instance.CharacterList.Count - 1;
				GameManager.instance.PlayerParty.Add(GameManager.instance.PlayerCharacterId);
				//GameManager.instance.StartDialogue();
			}
            stagePanels[(int)stage].SetActive(true);

            TitleText.text = stage.ToString();

        }
    }
}
