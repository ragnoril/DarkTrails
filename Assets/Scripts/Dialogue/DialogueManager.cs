using System;
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
        public DialogueVariableManager VariableManager;
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
			DialogueStartNode = "Start";
			DialogueUI = GameObject.FindObjectOfType<DialogueUI>();
            VariableManager = DialogueUI.GetComponent<DialogueVariableManager>();
		}

		public override void Pause()
		{
			base.Pause();
		}

		public override void Resume()
		{
			base.Resume();
		}

		public override void Quit()
		{
			base.Quit();
		}

		public void StartDialogue()
		{
			DialogueUI.StartDialogue(DialogueFile, DialogueStartNode);
		}

        public void ContinueDialogue()
        {
            DialogueUI.ContinueDialogue(DialogueStartNode);
        }

		[YarnCommand("set_dialog_image")]
		public void SetDialougeImage(string imageName)
		{
            DialogueUI.ImagePanel.SetActive(true);
            DialogueUI.ImageObject.sprite = (Sprite)Resources.Load(imageName, typeof(Sprite));
        }

        [YarnCommand("close_dialog_image")]
        public void CloseDialougeImage()
        {
            DialogueUI.ImagePanel.SetActive(false);
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

        [YarnCommand("roll_dice")]
        public void RollDice(int maxRnd)
        {
            int roll = UnityEngine.Random.Range(0, maxRnd);
            VariableManager.SetValue("$roll_result", new Yarn.Value(roll));
        }

        [YarnCommand("enable_map_node")]
        public void EnableMapNode(string mapName, string nodeName)
        {
            Travel.TravelManager.instance.TravelActionEnableNode(mapName, nodeName);
        }

        [YarnCommand("disable_map_node")]
        public void DisableMapNode(string mapName, string nodeName)
        {
            Travel.TravelManager.instance.TravelActionDisableNode(mapName, nodeName);
        }

        [YarnCommand("enable_map_node_by_id")]
        public void EnableMapNodeById(string mapName, int nodeId)
        {
            Debug.Log("enable_travel_node: " + nodeId.ToString());
            Travel.TravelManager.instance.TravelActionEnableNodeById(mapName, nodeId);
        }

        [YarnCommand("disable_map_node_by_id")]
        public void DisableMapNodeById(string mapName, int nodeId)
        {
            Debug.Log("disable_travel_node: " + nodeId.ToString());
            Travel.TravelManager.instance.TravelActionDisableNodeById(mapName, nodeId);
        }

        [YarnCommand("open_overworld")]
        public void OpenOverWorld(string sceneName)
        {
            //GameManager.instance.StartCombat(encounterName, winDialogue, loseDialogue);
            Debug.Log("open_overworld_command: " + sceneName);
            GameManager.instance.OpenOverWorld(sceneName);
        }

        [YarnCommand("enable_overworld_node")]
        public void EnableOverWorldNode(string sceneName, string nodeName)
        {
            OverWorld.OverWorldManager.instance.OverWorldActionEnableNode(sceneName, nodeName);
        }

        [YarnCommand("disable_overworld_node")]
        public void DisableOverWorldNode(string sceneName, string nodeName)
        {
            OverWorld.OverWorldManager.instance.OverWorldActionDisableNode(sceneName, nodeName);
        }

        [YarnCommand("enable_overworld_node_by_id")]
        public void EnableOverWorldNodeById(string sceneName, int nodeId)
        {
            OverWorld.OverWorldManager.instance.OverWorldActionEnableNodeById(sceneName, nodeId);
        }

        [YarnCommand("disable_overworld_node_by_id")]
        public void DisableOverWorldNodeById(string sceneName, int nodeId)
        {
            OverWorld.OverWorldManager.instance.OverWorldActionDisableNodeById(sceneName, nodeId);
        }

        [YarnCommand("quit_game")]
        public void QuitGame()
        {
            Debug.Log("quit game");
            Application.Quit();
        }

    }
}
