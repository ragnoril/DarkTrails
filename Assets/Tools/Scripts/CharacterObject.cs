using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.Tools
{
    public class CharacterObject : MonoBehaviour
    {

        public int CharacterId;
        public Text NameText;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetCharacter(int id)
        {
            CharacterId = id;
            NameText.text = CharGenManager.instance.CharacterList[id].Name;
        }

        public void DeleteCharacter()
        {
            CharGenManager.instance.CharacterList.RemoveAt(CharacterId);
            Destroy(this.gameObject);
        }

        public void EditCharacter()
        {
            CharGenManager.instance.EditCharacterIndex = CharacterId;
            CharGenManager.instance.EditCharacter();
        }
    }
}
