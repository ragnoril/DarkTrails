using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.OverWorld
{

    public class UIManager : MonoBehaviour
    {


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OpenInventory()
        {
            //Inventory.InventoryManager.instance.SetInventoryCharacter(GameManager.instance.CharacterList[GameManager.instance.PlayerCharacterId], GameManager.instance.PlayerInventory);
            //Inventory.InventoryManager.instance.SetA(GameManager.instance.PlayerCharacterId, GameManager.instance.PlayerInventoryId);
            Inventory.InventoryManager.instance.SetA(PartyManagement.PartyManager.instance.PlayerParty.MainCharacterId, PartyManagement.PartyManager.instance.PlayerParty.InventoryId);
            GameManager.instance.OpenInventory(Inventory.InventoryMode.Personal);
            
        }

        public void OpenCharacter()
        {
            GameManager.instance.OpenCharacter(Character.CharacterScreenMode.Show, Character.CharacterManager.instance.CharacterList[PartyManagement.PartyManager.instance.PlayerParty.MainCharacterId]);

        }

        public void OpenCharacterLevelUp()
        {
            GameManager.instance.OpenCharacter(Character.CharacterScreenMode.Create, new Character.CharacterData());

        }

        public void OpenCombat()
        {
            GameManager.instance.OpenCombat("first_attack");
        }

        public void OpenPartyManagement()
        {
            GameManager.instance.OpenParty();
        }
    }
}