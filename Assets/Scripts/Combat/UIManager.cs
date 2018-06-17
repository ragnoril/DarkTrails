﻿using System.Collections;
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


		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		public void EndTurn()
		{
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
				DarkTrails.GameManager.instance.DialogueStartNode = DarkTrails.GameManager.instance.WinDialogue;
				DarkTrails.GameManager.instance.StartDialogue();
			}
			else if (CombatManager.instance.WhoWon == 1)
			{
				DarkTrails.GameManager.instance.DialogueStartNode = DarkTrails.GameManager.instance.LoseDialogue;
				DarkTrails.GameManager.instance.StartDialogue();
			}
		}
	}
}