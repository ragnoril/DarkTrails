using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails.Tools
{

    public class CharGenUIManager : MonoBehaviour
    {

        public void ExportCharacterList()
        {
            CharGenManager.instance.SaveCharacterList();
        }

        public void CreateNewCharacter()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("CharGen");
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}