﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

namespace DarkTrails.Dialogue
{
	public class DialogueManager : BaseModule
	{
		private static DialogueManager _instance;
		public static DialogueManager instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = GameObject.FindObjectOfType<DialogueManager>();
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

		public DialogueUI DialogueUI;
		public string DialogueFile = "";
		public string DialogueStartNode = "";

		// Use this for initialization
		void Start()
		{
			ModuleType = GAMEMODULES.Dialogue;
		}

		// Update is called once per frame
		void Update()
		{

		}

		public override void Initialize(string filename)
		{
			DialogueFile = filename;
			//DialogueStartNode = "start";
			DialogueUI = GameObject.FindObjectOfType<DialogueUI>();
		}

		public override void Pause()
		{
			base.Pause();
		}

		public override void Resume()
		{
			base.Resume();
			StartDialogue();
		}

		public override void Quit()
		{
			base.Quit();
		}

		public void StartDialogue()
		{
			DialogueUI.StartDialogue(DialogueFile, DialogueStartNode);
		}

		[YarnCommand("set_dialog_image")]
		public void SetDialougeImage(string imageName)
		{
			Debug.Log("dialog_image_command: " + imageName);
		}

		[YarnCommand("open_combat")]
		public void OpenCombat(string encounterName)
		{
			Debug.Log("open_combat_command: " + encounterName);
			GameManager.instance.OpenCombat(encounterName);
		}

		[YarnCommand("open_travel")]
		public void OpenTravel(string mapName)
		{
			//GameManager.instance.StartCombat(encounterName, winDialogue, loseDialogue);
			Debug.Log("open_travel_command: " + mapName);
			GameManager.instance.OpenTravel(mapName);
		}

	}
}