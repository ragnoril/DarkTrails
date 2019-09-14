using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.Character
{
    public class UIManager : MonoBehaviour
    {
        public List<StatsUIObject> Stats;
        public List<StatsUIObject> Skills;
        public List<StatsUIObject> SecondaryStats;

        public InputField CharacterName;
        public Text TooltipText;

        public GameObject NextPortrait;
        public GameObject PreviousPortrait;

        public GameObject CloseButton;
        public GameObject UpdateButton;
        public GameObject CreateButton;

        public Text StatsTitle;
        public Text SkillsTitle;

        public CharacterData BaseCharacter;
        public bool UpdateImmediately;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void FillStats()
        {
            Stats[0].Name = "Strength";
            Stats[0].MaxValue = 10;
            Stats[0].MinValue = 1;
            Stats[0].PointSpent = 0;
            Stats[0].UpdateUI();

            Stats[1].Name = "Agility";
            Stats[1].MaxValue = 10;
            Stats[1].MinValue = 1;
            Stats[1].PointSpent = 0;
            Stats[1].UpdateUI();

            Stats[2].Name = "Endurance";
            Stats[2].MaxValue = 10;
            Stats[2].MinValue = 1;
            Stats[2].PointSpent = 0;
            Stats[2].UpdateUI();

            Stats[3].Name = "Intelligence";
            Stats[3].MaxValue = 10;
            Stats[3].MinValue = 1;
            Stats[3].PointSpent = 0;
            Stats[3].UpdateUI();

            Stats[4].Name = "Willpower";
            Stats[4].MaxValue = 10;
            Stats[4].MinValue = 1;
            Stats[4].PointSpent = 0;
            Stats[4].UpdateUI();

            for (int i = 0; i < (int)STATS.StatCount; i++)
            {
                Stats[i].SetStyle(1);
                Stats[i].Index = i;
            }
            
        }

        public void FillSkills()
        {
            Skills[0].Name = "Melee";
            Skills[0].MaxValue = 100;
            Skills[0].MinValue = 0;
            Skills[0].PointSpent = 0;
            Skills[0].UpdateUI();

            Skills[1].Name = "Ranged";
            Skills[1].MaxValue = 100;
            Skills[1].MinValue = 0;
            Skills[1].PointSpent = 0;
            Skills[1].UpdateUI();

            Skills[2].Name = "Dodge";
            Skills[2].MaxValue = 100;
            Skills[2].MinValue = 0;
            Skills[2].PointSpent = 0;
            Skills[2].UpdateUI();

            Skills[3].Name = "Shield";
            Skills[3].MaxValue = 100;
            Skills[3].MinValue = 0;
            Skills[3].PointSpent = 0;
            Skills[3].UpdateUI();

            Skills[4].Name = "Mysticism";
            Skills[4].MaxValue = 100;
            Skills[4].MinValue = 0;
            Skills[4].PointSpent = 0;
            Skills[4].UpdateUI();

            Skills[5].Name = "Crafting";
            Skills[5].MaxValue = 100;
            Skills[5].MinValue = 0;
            Skills[5].PointSpent = 0;
            Skills[5].UpdateUI();

            Skills[6].Name = "Herbalism";
            Skills[6].MaxValue = 100;
            Skills[6].MinValue = 0;
            Skills[6].PointSpent = 0;
            Skills[6].UpdateUI();

            Skills[7].Name = "Leadership";
            Skills[7].MaxValue = 100;
            Skills[7].MinValue = 0;
            Skills[7].PointSpent = 0;
            Skills[7].UpdateUI();

            Skills[8].Name = "Trading";
            Skills[8].MaxValue = 100;
            Skills[8].MinValue = 0;
            Skills[8].PointSpent = 0;
            Skills[8].UpdateUI();

            Skills[9].Name = "Healing";
            Skills[9].MaxValue = 100;
            Skills[9].MinValue = 0;
            Skills[9].PointSpent = 0;
            Skills[9].UpdateUI();

            Skills[10].Name = "Persuasion";
            Skills[10].MaxValue = 100;
            Skills[10].MinValue = 0;
            Skills[10].PointSpent = 0;
            Skills[10].UpdateUI();

            Skills[11].Name = "NatureLore";
            Skills[11].MaxValue = 100;
            Skills[11].MinValue = 0;
            Skills[11].PointSpent = 0;
            Skills[11].UpdateUI();

            for (int i = 0; i < (int)SKILLS.SkillCount; i++)
            {
                Skills[i].SetStyle(1);
                Skills[i].Index = i;
            }
        }    

        public void FillSecondaryStats()
        {
            SecondaryStats[0].Name = "Level: ";
            SecondaryStats[0].Value = 0;
            SecondaryStats[0].MinValue = int.MinValue;
            SecondaryStats[0].MaxValue = int.MaxValue;

            SecondaryStats[1].Name = "Current Exp: ";
            SecondaryStats[1].Value = 0;
            SecondaryStats[1].MinValue = int.MinValue;
            SecondaryStats[1].MaxValue = int.MaxValue;

            SecondaryStats[2].Name = "Needed Exp: ";
            SecondaryStats[2].Value = 0;
            SecondaryStats[2].MinValue = int.MinValue;
            SecondaryStats[2].MaxValue = int.MaxValue;
            
            SecondaryStats[3].Name = "Hit Points: ";
            SecondaryStats[3].Value = 0;
            SecondaryStats[3].MinValue = int.MinValue;
            SecondaryStats[3].MaxValue = int.MaxValue;
            
            SecondaryStats[4].Name = "Action Points: ";
            SecondaryStats[4].Value = 0;
            SecondaryStats[4].MinValue = int.MinValue;
            SecondaryStats[4].MaxValue = int.MaxValue;
            
            SecondaryStats[5].Name = "Initiative: ";
            SecondaryStats[5].Value = 0;
            SecondaryStats[5].MinValue = int.MinValue;
            SecondaryStats[5].MaxValue = int.MaxValue;
            
            SecondaryStats[6].Name = "Healing Rate: ";
            SecondaryStats[6].Value = 0;
            SecondaryStats[6].MinValue = int.MinValue;
            SecondaryStats[6].MaxValue = int.MaxValue;
            
            SecondaryStats[7].Name = "HP Per Level: ";
            SecondaryStats[7].Value = 0;
            SecondaryStats[7].MinValue = int.MinValue;
            SecondaryStats[7].MaxValue = int.MaxValue;
            
            SecondaryStats[8].Name = "Skills Per Level: ";
            SecondaryStats[8].Value = 0;
            SecondaryStats[8].MinValue = int.MinValue;
            SecondaryStats[8].MaxValue = int.MaxValue;
            
            SecondaryStats[9].Name = "Critical Chance: ";
            SecondaryStats[9].Value = 0;
            SecondaryStats[9].MinValue = int.MinValue;
            SecondaryStats[9].MaxValue = int.MaxValue;
            
            SecondaryStats[10].Name = "Armor Class: ";
            SecondaryStats[10].Value = 0;
            SecondaryStats[10].MinValue = int.MinValue;
            SecondaryStats[10].MaxValue = int.MaxValue;
            
            SecondaryStats[11].Name = "Base Damage: ";
            SecondaryStats[11].Value = 0;
            SecondaryStats[11].MinValue = int.MinValue;
            SecondaryStats[11].MaxValue = int.MaxValue;
            
            for(int i = 0; i < SecondaryStats.Count; i ++)
            {
                SecondaryStats[i].EnableEditMode(false);
                SecondaryStats[i].SetStyle(0);
                SecondaryStats[i].Index = i;
                SecondaryStats[i].UpdateUI();
            }
        }

        public void SetCharacterData(CharacterData character)
        {
            FillStats();
            FillSkills();
            FillSecondaryStats();

            BaseCharacter = new CharacterData();
            BaseCharacter.CopyFrom(character);

            CharacterName.text = BaseCharacter.Name;

            for (int i = 0; i < (int)STATS.StatCount; i++)
            {
                Stats[i].Value = BaseCharacter.Stats[i];
                Stats[i].MinValue = BaseCharacter.Stats[i];
                Stats[i].UpdateUI();
            }

            for (int i = 0; i < (int)SKILLS.SkillCount; i++)
            {
                Skills[i].Value = BaseCharacter.Skills[i];
                Skills[i].MinValue = BaseCharacter.Skills[i];
                Skills[i].UpdateUI();
            }

            SecondaryStats[0].Value = BaseCharacter.Level;
            SecondaryStats[1].Value = BaseCharacter.CurrentExperiencePoints;
            SecondaryStats[2].Value = BaseCharacter.ExperiencePointsToNextLevel;
            SecondaryStats[3].Value = BaseCharacter.MaxHitPoints;
            SecondaryStats[4].Value = BaseCharacter.MaxActionPoints;
            SecondaryStats[5].Value = BaseCharacter.Initiative;
            SecondaryStats[6].Value = BaseCharacter.HealingRate;
            SecondaryStats[7].Value = BaseCharacter.HitPointsPerLevel;
            SecondaryStats[8].Value = BaseCharacter.SkillPointsPerLevel;
            SecondaryStats[9].Value = BaseCharacter.CriticalChance;
            SecondaryStats[10].Value = BaseCharacter.ArmorClass;
            SecondaryStats[11].Value = BaseCharacter.BaseDamage;

            for (int i = 0; i < SecondaryStats.Count; i++)
            {
                SecondaryStats[i].UpdateUI();
            }
        }

        public void SetEditMode(bool value)
        {
            SetEditMode(value, value, value, value);
        }

        public void SetEditMode(bool portraitValue, bool nameValue, bool statsValue, bool skillsValue)
        {
            CharacterName.interactable = nameValue;

            NextPortrait.SetActive(portraitValue);
            PreviousPortrait.SetActive(portraitValue);

            for (int i = 0; i < (int)STATS.StatCount; i++)
            {
                Stats[i].EnableEditMode(statsValue);
                Stats[i].PointType = POINTTYPE.StatPoint;
            }

            for (int i = 0; i < (int)SKILLS.SkillCount; i++)
            {
                Skills[i].EnableEditMode(skillsValue);
                Skills[i].PointType = POINTTYPE.SkillPoint;
            }


            if (CharacterManager.instance.Mode == CharacterScreenMode.Create)
            {
                CloseButton.SetActive(false);
                UpdateButton.SetActive(false);
                CreateButton.SetActive(true);
            }
            else if (statsValue || skillsValue)
            {
                CloseButton.SetActive(false);
                UpdateButton.SetActive(true);
                CreateButton.SetActive(false);
            }
            else
            {
                CloseButton.SetActive(true);
                UpdateButton.SetActive(false);
                CreateButton.SetActive(false);
            }
        }

        public void CheckIfPointLeft()
        {
            UpdateCharacterData();

            bool value = false;
            if (CharacterManager.instance.PointsToSpendForStats > 0)
                value = true;
            for (int i = 0; i < (int)STATS.StatCount; i++)
            {
                Stats[i].IncButton.interactable = value;
            }

            value = false;
            if (CharacterManager.instance.PointsToSpendForSkills > 0)
                value = true;
            for (int i = 0; i < (int)SKILLS.SkillCount; i++)
            {
                Skills[i].IncButton.interactable = value;
            }
        }

        public void UpdateSkills(int index, int change)
        {
            switch(index)
            {
                case (int)STATS.Strength:
                    Skills[(int)SKILLS.Melee].Value += (change * 2);
                    Skills[(int)SKILLS.Ranged].Value += change;
                    Skills[(int)SKILLS.Shield].Value += (change * 2);
                    break;
                case (int)STATS.Agility:
                    Skills[(int)SKILLS.Melee].Value += change;
                    Skills[(int)SKILLS.Ranged].Value += (change * 2);
                    Skills[(int)SKILLS.Dodge].Value += (change * 3);
                    Skills[(int)SKILLS.Shield].Value += (change * 2);
                    Skills[(int)SKILLS.Healing].Value += change;
                    Skills[(int)SKILLS.NatureLore].Value += change;
                    break;
                case (int)STATS.Endurance:
                    Skills[(int)SKILLS.NatureLore].Value += change;
                    break;
                case (int)STATS.Intelligence:
                    Skills[(int)SKILLS.Mysticism].Value = (change * 2); 
                    Skills[(int)SKILLS.Crafting].Value = (change * 4);
                    Skills[(int)SKILLS.Leadership].Value = change;
                    Skills[(int)SKILLS.Herbalism].Value = (change * 3);
                    Skills[(int)SKILLS.Healing].Value = change;
                    Skills[(int)SKILLS.Persuasion].Value = (change * 2);
                    Skills[(int)SKILLS.Trading].Value = (change * 3);
                    Skills[(int)SKILLS.NatureLore].Value = change;
                    break;
                case (int)STATS.Willpower:
                    Skills[(int)SKILLS.Mysticism].Value += (change * 2);
                    Skills[(int)SKILLS.Leadership].Value += (change * 2);
                    Skills[(int)SKILLS.Persuasion].Value += change;
                    Skills[(int)SKILLS.Trading].Value += change;
                    Skills[(int)SKILLS.NatureLore].Value += change;
                    break;
            }

            /*
            Skills[(int)SKILLS.Melee].Value = (Stats[(int)STATS.Strength].Value * 2) + Stats[(int)STATS.Agility].Value + Skills[(int)SKILLS.Melee].PointSpent;
            Skills[(int)SKILLS.Ranged].Value = (Stats[(int)STATS.Agility].Value * 2) + Stats[(int)STATS.Strength].Value + Skills[(int)SKILLS.Ranged].PointSpent;
            Skills[(int)SKILLS.Dodge].Value = (Stats[(int)STATS.Agility].Value * 3) + Skills[(int)SKILLS.Dodge].PointSpent;
            Skills[(int)SKILLS.Shield].Value = ((Stats[(int)STATS.Strength].Value + Stats[(int)STATS.Agility].Value) * 2) + Skills[(int)SKILLS.Shield].PointSpent;
            Skills[(int)SKILLS.Mysticism].Value = ((Stats[(int)STATS.Intelligence].Value + Stats[(int)STATS.Willpower].Value) * 2) + Skills[(int)SKILLS.Mysticism].PointSpent;
            Skills[(int)SKILLS.Crafting].Value = (Stats[(int)STATS.Intelligence].Value * 4) + Skills[(int)SKILLS.Crafting].PointSpent;
            Skills[(int)SKILLS.Leadership].Value = (Stats[(int)STATS.Willpower].Value * 2) + Stats[(int)STATS.Intelligence].Value + Skills[(int)SKILLS.Leadership].PointSpent;
            Skills[(int)SKILLS.Herbalism].Value = (Stats[(int)STATS.Intelligence].Value * 3) + Skills[(int)SKILLS.Herbalism].PointSpent;
            Skills[(int)SKILLS.Healing].Value = Stats[(int)STATS.Intelligence].Value + Stats[(int)STATS.Agility].Value + Skills[(int)SKILLS.Healing].PointSpent;
            Skills[(int)SKILLS.Persuasion].Value = (Stats[(int)STATS.Intelligence].Value * 2) + Stats[(int)STATS.Willpower].Value + Skills[(int)SKILLS.Persuasion].PointSpent;
            Skills[(int)SKILLS.Trading].Value = (Stats[(int)STATS.Intelligence].Value * 3) + Stats[(int)STATS.Willpower].Value + Skills[(int)SKILLS.Melee].PointSpent;
            Skills[(int)SKILLS.NatureLore].Value = Stats[(int)STATS.Endurance].Value + Stats[(int)STATS.Intelligence].Value + Stats[(int)STATS.Willpower].Value + Stats[(int)STATS.Agility].Value + Skills[(int)SKILLS.NatureLore].PointSpent;
            */
            for (int i = 0; i < (int)SKILLS.SkillCount; i++)
            {
                Skills[i].UpdateUI();
            }
        }

        public void UpdateCharacterData()
        {
            //todo: here we must do something :'(
            if (CharacterName.interactable)
                BaseCharacter.Name = CharacterName.text;

            for (int i = 0; i < (int)STATS.StatCount; i++)
            {
                BaseCharacter.Stats[i] = Stats[i].Value;
            }

            for (int i = 0; i < (int)SKILLS.SkillCount; i++)
            {
                BaseCharacter.Skills[i] = Skills[i].Value;
            }

            SecondaryStats[0].Value = BaseCharacter.Level;
            SecondaryStats[1].Value = BaseCharacter.CurrentExperiencePoints;
            SecondaryStats[2].Value = BaseCharacter.ExperiencePointsToNextLevel;
            SecondaryStats[3].Value = BaseCharacter.MaxHitPoints;
            SecondaryStats[4].Value = BaseCharacter.MaxActionPoints;
            SecondaryStats[5].Value = BaseCharacter.Initiative;
            SecondaryStats[6].Value = BaseCharacter.HealingRate;
            SecondaryStats[7].Value = BaseCharacter.HitPointsPerLevel;
            SecondaryStats[8].Value = BaseCharacter.SkillPointsPerLevel;
            SecondaryStats[9].Value = BaseCharacter.CriticalChance;
            SecondaryStats[10].Value = BaseCharacter.ArmorClass;
            SecondaryStats[11].Value = BaseCharacter.BaseDamage;

            for (int i = 0; i < SecondaryStats.Count; i++)
            {
                SecondaryStats[i].UpdateUI();
            }
        }

        public void CreateAndCloseUI()
        {
            if (CharacterManager.instance.PointsToSpendForSkills != 0 || CharacterManager.instance.PointsToSpendForStats != 0)
            {
                TooltipText.text = "You have to spent all your points!";
            }
            else if (CharacterName.text == "")
            {
                TooltipText.text = "You have to give a name to your character!";
            }
            else
            {
                BaseCharacter.PointsToSpendForSkills = CharacterManager.instance.PointsToSpendForSkills;
                BaseCharacter.PointsToSpendForStats = CharacterManager.instance.PointsToSpendForStats;
                CharacterManager.instance.CurrentCharacter.CopyFrom(BaseCharacter);
                CharacterManager.instance.AddCharacterToList();
                CloseUI();
            }
        }

        public void SaveAndCloseUI()
        {
            BaseCharacter.PointsToSpendForSkills = CharacterManager.instance.PointsToSpendForSkills;
            BaseCharacter.PointsToSpendForStats = CharacterManager.instance.PointsToSpendForStats;

            CharacterManager.instance.CurrentCharacter.CopyFrom(BaseCharacter);

            CloseUI();
        }

        public void CloseUI()
        {
            switch (CharacterManager.instance.PreviousModule)
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
                    GameManager.instance.ChangeModule(CharacterManager.instance.PreviousModule);
                    break;
            }
        }
    }
}
