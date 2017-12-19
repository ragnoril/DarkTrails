using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DarkTrails;
using System.Xml;
using System.IO;

public class BattleManager : MonoBehaviour
{
    private static BattleManager _instance;

    public static BattleManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<BattleManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }

    }

    public List<CharacterData> CharacterList = new List<CharacterData>();
	public List<Item> ItemList = new List<Item>();


	public List<int> TeamA;
    public List<int> TeamB;

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

    // Use this for initialization
    void Start ()
    {
		LoadItemList();
		LoadCharacterList();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void LoadItemList()
    {
        string filePath = Application.dataPath + "/ItemList.xml";
        
        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);

        XmlNode root = doc.SelectSingleNode("ItemList");
        XmlNodeList itemList = root.SelectNodes(".//Item");

        foreach(XmlNode itm in itemList)
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

	public void LoadCharacterList()
	{
		string filePath = Application.dataPath + "/CharacterList.xml";

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

	public void SaveCharacterList()
    {
        string filePath = Application.dataPath + "/CharacterList_" + System.DateTime.Now.Ticks.ToString() + ".xml";

        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = xmlDoc.CreateElement("CharacterList");
        xmlDoc.AppendChild(rootNode);

        foreach (CharacterData chr in CharacterList)
        {
            XmlNode charNode = xmlDoc.CreateElement("Character");
            XmlAttribute attributeName = xmlDoc.CreateAttribute("name");
            attributeName.Value = chr.Name;
            XmlAttribute attributeLevel = xmlDoc.CreateAttribute("level");
            attributeLevel.Value = chr.Level.ToString();

            charNode.Attributes.Append(attributeName);
            charNode.Attributes.Append(attributeLevel);

            XmlNode statsNode = xmlDoc.CreateElement("Stats");
            for (int i =0; i < chr.Stats.Length; i++)
            {
                XmlNode statNode = xmlDoc.CreateElement("Stat");

                XmlAttribute statIdAttr = xmlDoc.CreateAttribute("id");
                statIdAttr.Value = i.ToString();

                XmlAttribute statNameAttr = xmlDoc.CreateAttribute("name");
                STATS statNameEnum = (STATS)i;
                statNameAttr.Value = statNameEnum.ToString();

                XmlAttribute statValAttr = xmlDoc.CreateAttribute("value");
                statValAttr.Value = chr.Stats[i].ToString();


                statNode.Attributes.Append(statIdAttr);
                statNode.Attributes.Append(statNameAttr);
                statNode.Attributes.Append(statValAttr);

                statsNode.AppendChild(statNode);
            }
            charNode.AppendChild(statsNode);

            XmlNode skillsNode = xmlDoc.CreateElement("Skills");
            for (int i = 0; i < chr.Skills.Length; i++)
            {
                XmlNode skillNode = xmlDoc.CreateElement("Skill");

                XmlAttribute skillIdAttr = xmlDoc.CreateAttribute("id");
                skillIdAttr.Value = i.ToString();

                XmlAttribute skillNameAttr = xmlDoc.CreateAttribute("name");
                SKILLS skillNameEnum = (SKILLS)i;
                skillNameAttr.Value = skillNameEnum.ToString();

                XmlAttribute skillValAttr = xmlDoc.CreateAttribute("value");
                skillValAttr.Value = chr.Skills[i].ToString();
                

                skillNode.Attributes.Append(skillIdAttr);
                skillNode.Attributes.Append(skillNameAttr);
                skillNode.Attributes.Append(skillValAttr);

                skillsNode.AppendChild(skillNode);
            }
            charNode.AppendChild(skillsNode);

			XmlNode equipmentsNode = xmlDoc.CreateElement("Equipments");
			for (int i = 0; i < chr.Equipments.Length; i++)
			{
				XmlNode equipNode = xmlDoc.CreateElement("Equipment");

				XmlAttribute equipIdAttr = xmlDoc.CreateAttribute("id");
				equipIdAttr.Value = i.ToString();

				XmlAttribute equipValAttr = xmlDoc.CreateAttribute("value");
				equipValAttr.Value = ItemList.IndexOf(chr.Equipments[i]).ToString();


				equipNode.Attributes.Append(equipIdAttr);
				equipNode.Attributes.Append(equipValAttr);

				equipmentsNode.AppendChild(equipNode);
			}
			charNode.AppendChild(equipmentsNode);

			rootNode.AppendChild(charNode);
        }

        xmlDoc.Save(filePath);
    }


}
