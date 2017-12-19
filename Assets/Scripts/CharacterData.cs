using UnityEngine;
using System.Collections;

namespace DarkTrails
{

    #region CharSystemEnums

    public enum STATS
    {
        Strength = 0,
        Agility,
        Endurance,
        Intelligence,
        Willpower,
        StatCount
    }

    public enum SKILLS
    {
        Melee = 0,
        Ranged,
        Dodge,
        Shield,
        Mysticism,
        Crafting,
        Herbalism,
        Leadership,
        Trading,
        Healing,
        Persuasion,
        NatureLore,
        SkillCount
    }

    public enum EQUIP
    {
        BodyArmor = 0,
        Helmet,
        Boots,
        MainHand,
        OffHand,
        EquipCount
    }

    #endregion

    public class CharacterData
    {

        #region Primary Stats

        public int[] Stats = new int[(int)STATS.StatCount];
        public int[] StatBonuses = new int[(int)STATS.StatCount];

        #endregion

        #region Skills

        public int[] Skills = new int[(int)SKILLS.SkillCount];
        public int[] SkillBonuses = new int[(int)SKILLS.SkillCount];


        public int GetSkillTotal(int skillIndex)
        {
            return Skills[skillIndex] + SkillBonuses[skillIndex];
        }

		#endregion

		#region Equipments

		public Item[] Equipments = new Item[(int)EQUIP.EquipCount];

		#endregion

		#region Secondary Stats

		public string Name;

        public int Level;
        public int CurrentExperiencePoints;
        public int ExperiencePointsToNextLevel;

        public int MaxHitPoints
        {
            get
            {
                return (Stats[(int)STATS.Endurance] + (Stats[(int)STATS.Strength] / 2));
            }
        }

        public int MaxActionPoints
        {
            get
            {
                return 5 + (Stats[(int)STATS.Agility] / 2);
            }
        }

        public int Initiative
        {
            get
            {
                return 3 + (Stats[(int)STATS.Agility] / 2);
            }
        }

        public int HealingRate
        {
            get
            {
                return (Stats[(int)STATS.Endurance] / 3);
            }
        }

        public int HitPointsPerLevel
        {
            get
            {
                return (Stats[(int)STATS.Endurance] / 3);
            }
        }

        public int SkillPointsPerLevel
        {
            get
            {
                return (Stats[(int)STATS.Intelligence] * 2);
            }
        }

        public int CriticalChance;
        public int ArmorClass
        {
            get
            {
                int bodyArmor = 0;
                if (Equipments[(int)EQUIP.BodyArmor] != null)
                    bodyArmor = ((Armor)Equipments[(int)EQUIP.BodyArmor]).ArmorValue;

                int helmet = 0;
                if (Equipments[(int)EQUIP.Helmet] != null)
                    helmet = ((Armor)Equipments[(int)EQUIP.Helmet]).ArmorValue;

                int boots = 0;
                if (Equipments[(int)EQUIP.Boots] != null)
                    boots = ((Armor)Equipments[(int)EQUIP.Boots]).ArmorValue;


                return bodyArmor + helmet + boots;
            }
        }
        public int BaseDamage
        {
            get
            {
                return (Stats[(int)STATS.Strength] / 2);
            }
        }

		public void CopyFrom(CharacterData data)
		{
			for (int i = 0; i < (int)STATS.StatCount; i++)
			{
				Stats[i] = data.Stats[i];
				StatBonuses[i] = data.StatBonuses[i];
			}

			Level = data.Level;
			Name = data.Name;

			CurrentExperiencePoints = data.CurrentExperiencePoints;
			ExperiencePointsToNextLevel = data.ExperiencePointsToNextLevel;

			CriticalChance = data.CriticalChance;

			for (int i = 0; i < (int)SKILLS.SkillCount; i++)
			{
				Skills[i] = data.Skills[i];
				SkillBonuses[i] = data.SkillBonuses[i];
			}

			for (int i = 0; i < (int)EQUIP.EquipCount; i++)
			{
				Equipments[i] = data.Equipments[i];
			}
		}

        #endregion
    }

}