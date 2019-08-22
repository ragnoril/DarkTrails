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
            Inventory.InventoryManager.instance.SetA(GameManager.instance.PlayerCharacterId, GameManager.instance.PlayerInventoryId);
            GameManager.instance.OpenInventory(Inventory.InventoryMode.Personal);
            
        }
    }
}