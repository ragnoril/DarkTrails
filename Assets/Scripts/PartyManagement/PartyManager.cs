using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace DarkTrails.PartyManagement
{
    public class PartyManager : BaseModule
    {
        private static PartyManager _instance;
        public static PartyManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<PartyManager>();
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

        public List<PartyData> PartyList;
        public PartyData PlayerParty;
        public UIManager UI;

        public GAMEMODULES PreviousModule;

        // Start is called before the first frame update
        void Start()
        {
            ModuleType = GAMEMODULES.Party;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Initialize(string filename)
        {
            PlayerParty = new PartyData();
            PartyList = new List<PartyData>();

            string filePath = Application.dataPath + "/" + filename;

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            
            XmlNode root = doc.SelectSingleNode("PartyList");
            XmlNodeList partyList = root.SelectNodes(".//Party");

            string startPartyName = root.Attributes["start"].Value;

            foreach (XmlNode party in partyList)
            {
                PartyData partyData = new PartyData();

                string partyName = party.Attributes["name"].Value;
                int charId = int.Parse(party.Attributes["characterId"].Value);
                int invId = int.Parse(party.Attributes["inventoryId"].Value);

                partyData.PartyName = partyName;
                partyData.MainCharacterId = charId;
                partyData.InventoryId = invId;

                XmlNodeList charList = party.SelectNodes(".//Character");
                foreach (XmlNode chr in charList)
                {
                    int chrId = int.Parse(chr.Attributes["id"].Value);
                    partyData.Roster.Add(chrId);
                }

                PartyList.Add(partyData);

                if (startPartyName == partyName)
                {
                    PlayerParty = partyData;
                }
            }
        }

        public void SetMainCharacter(int characterId, int inventoryId)
        {
            PlayerParty.MainCharacterId = characterId;
            PlayerParty.InventoryId = inventoryId;
            if (!PlayerParty.Roster.Contains(characterId))
                PlayerParty.Roster.Add(characterId);
        }

        public void AddNewCharacterToParty(int characterId)
        {
            if (!PlayerParty.Roster.Contains(characterId))
                PlayerParty.Roster.Add(characterId);
        }

        public void RemoveCharacterFromParty(int characterId)
        {
            if (PlayerParty.MainCharacterId == characterId)
                return;

            if (PlayerParty.Roster.Contains(characterId))
                PlayerParty.Roster.Remove(characterId);
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

        public void ShowPartyInfo()
        {
            UI.InitUI();
        }
    }
}
