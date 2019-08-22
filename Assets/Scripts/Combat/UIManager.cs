using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.Combat
{
	public class UIManager : MonoBehaviour
	{
		public Text NameText;
		public Text HPText;
		public Text APText;
		public Text WeaponText;
		public Text ArmorText;

		public GameObject EndPanel;
		public Text EndText;

        public GameObject LogPanel;
        public Text LogText;
        private float _logTimer;

        public Text HitChanceText;
        private float _hitChanceTimer;


		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
            if (_logTimer > 0f)
            {
                _logTimer -= Time.deltaTime;
            }
            else if (_logTimer <= 0f && LogPanel.gameObject.activeSelf)
            {
                ShowCombatLog(false);
            }

            if (_hitChanceTimer > 0f)
            {
                _hitChanceTimer -= Time.deltaTime;
            }
            else if (_hitChanceTimer <= 0f && HitChanceText.gameObject.activeSelf)
            {
                HitChanceText.gameObject.SetActive(false);
            }
        }

		public void EndTurn()
		{
            if (CombatManager.instance.selectedAgent.doneMoving && CombatManager.instance.selectedAgent.teamId == 0)
                CombatManager.instance.PassTurn();
		}

		public void UpdateAgentData()
		{
			if (CombatManager.instance.selectedAgent == null) return;

			NameText.text = "Name: " + CombatManager.instance.selectedAgent.name;
            HPText.text = "HP: " + CombatManager.instance.selectedAgent.curHitPoints + " / " + CombatManager.instance.selectedAgent.maxHitPoints;
            APText.text = "AP: " + CombatManager.instance.selectedAgent.curActionPoints + " / " + CombatManager.instance.selectedAgent.maxActionPoints;
            WeaponText.text = "Damage: " + CombatManager.instance.selectedAgent.minDamage + " - " + CombatManager.instance.selectedAgent.maxDamage;
			ArmorText.text = "AC: " + CombatManager.instance.selectedAgent.armorClass;
		}

		public void EndTheGame()
		{
			if (CombatManager.instance.WhoWon == 0)
			{
				switch(GameManager.instance.EncounterList[CombatManager.instance.EncounterName].WinState.ModuleName)
				{
					case "Combat":
						GameManager.instance.OpenCombat(GameManager.instance.EncounterList[CombatManager.instance.EncounterName].WinState.Value);
						break;
					case "Inventory":

						break;
					case "Travel":
						GameManager.instance.OpenTravel(GameManager.instance.EncounterList[CombatManager.instance.EncounterName].WinState.Value);
						break;
					case "Dialogue":
						GameManager.instance.OpenDialogue(GameManager.instance.EncounterList[CombatManager.instance.EncounterName].WinState.Value);
						break;
				}
			}
			else if (CombatManager.instance.WhoWon == 1)
			{
				switch (GameManager.instance.EncounterList[CombatManager.instance.EncounterName].LoseState.ModuleName)
				{
					case "Combat":
						GameManager.instance.OpenCombat(GameManager.instance.EncounterList[CombatManager.instance.EncounterName].LoseState.Value);
						break;
					case "Inventory":

						break;
					case "Travel":
						GameManager.instance.OpenTravel(GameManager.instance.EncounterList[CombatManager.instance.EncounterName].LoseState.Value);
						break;
					case "Dialogue":
						GameManager.instance.OpenDialogue(GameManager.instance.EncounterList[CombatManager.instance.EncounterName].LoseState.Value);
						break;
				}
			}
		}

        public void ClearCombatLog()
        {
            LogText.text = "";
            ShowCombatLog(false);
        }

        public void ShowCombatLog(bool val)
        {
            LogPanel.gameObject.SetActive(val);
        }

        public void AddCombatLog(string msg)
        {
            LogText.text += msg + "\n";
            _logTimer = 3f;
            ShowCombatLog(true);
        }

        public void ShowHitChance(int val, Vector3 pos)
        {
            _hitChanceTimer = 5f;
            HitChanceText.rectTransform.position = Camera.main.WorldToScreenPoint(pos + new Vector3(.65f,0,0));
            HitChanceText.text = "% " + val.ToString();
            HitChanceText.gameObject.SetActive(true);

        }

        public void ShowMouseOverStats(Agent agent, Vector3 pos)
        {

        }
	}
}