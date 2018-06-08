using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

namespace DarkTrails
{

	public class DialogueCommandsManager : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		[YarnCommand("set_dialog_image")]
		public void SetDialougeImage(string imageName)
		{
			Debug.Log("dialog_image_command: " + imageName);
		}

		[YarnCommand("start_combat")]
		public void StartCombat(string encounterName)
		{
			Debug.Log("start_combat_command: " + encounterName);
		}

		[YarnCommand("start_combat")]
		public void StartCombat(string encounterName, string winDialogue, string loseDialogue)
		{
			GameManager.instance.StartCombat(encounterName, winDialogue, loseDialogue);
		}

	}
}
