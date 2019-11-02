using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.PartyManagement
{

    public class RosterAgent : MonoBehaviour
    {
        public int RosterId;
        public int CharacterId;
        public int InventoryId;

        public Text NameText; 

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
            Inventory.InventoryManager.instance.SetA(this.CharacterId, this.InventoryId);
            GameManager.instance.OpenInventory(Inventory.InventoryMode.Personal);
        }

        public void OpenCharacterInfo()
        {
            GameManager.instance.OpenCharacter(Character.CharacterScreenMode.Show, Character.CharacterManager.instance.CharacterList[this.CharacterId]);
        }
    }
}
