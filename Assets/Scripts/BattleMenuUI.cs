using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleMenuUI : MonoBehaviour
{
    public Dropdown CharacterListSelector;
    public GameObject TeamA;
    public GameObject TeamB;


    // Use this for initialization
    void Start ()
    {
        CharacterListSelector.options.Clear();
        List<string> options = new List<string>();
        foreach (var chr in BattleManager.instance.CharacterList)
        {
            options.Add(chr.Name);
        }
        CharacterListSelector.AddOptions(options);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void CreateNewCharacter()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharGen");
    }

    public void ExportCharacterList()
    {
        BattleManager.instance.SaveCharacterList();
    }

    public void PickCharacter(int teamId)
    {
        GameObject go = new GameObject();
        Text txt = go.AddComponent<Text>();
        txt.font = CharacterListSelector.GetComponentInChildren<Text>().font;
        txt.fontSize = 15;
        txt.color = Color.black;
        
        txt.text = BattleManager.instance.CharacterList[CharacterListSelector.value].Name;
        if (teamId == 0)
        {
            BattleManager.instance.TeamA.Add(CharacterListSelector.value);
            go.transform.SetParent(TeamA.transform);
        }
        else if (teamId == 1)
        {
            BattleManager.instance.TeamB.Add(CharacterListSelector.value);
            go.transform.SetParent(TeamB.transform);
        }
    }

    public void GoToBattle()
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene("CombatScene");
	}
}
