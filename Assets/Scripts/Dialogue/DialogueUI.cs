using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;

namespace DarkTrails.Dialogue
{

	public class DialogueUI : Yarn.Unity.DialogueUIBehaviour
	{
		public GameObject UIContainer;

		public Text MainText;
		public List<Button> OptionButtons;
		private Yarn.OptionChooser SetSelectedOption;

		public GameObject OptionPrefab;
		public Transform OptionsParent;

        public GameObject ImagePanel;
        public Image ImageObject;

        /// A UI element that appears after lines have finished appearing
        public GameObject ContinuePrompt;

        private bool _isStarted;

		public override IEnumerator RunCommand(Command command)
		{
            //todo: make it CmdArgs[] not cmdValue
			// "Perform" the command
			Debug.Log("Command: " + command.text);
			string cmdName = command.text.Substring(0, command.text.IndexOf(" "));
			//string cmdValue = (command.text.Substring(command.text.IndexOf(" "))).TrimStart(' ');
            string[] cmdArgs = (command.text.Substring(command.text.IndexOf(" "))).TrimStart(' ').Split(' ');
            for(int i = 0; i < cmdArgs.Length; i++)
            {
                if (cmdArgs[i][0]=='$')
                {
                    string str = ParseVariable(cmdArgs[i]);
                    cmdArgs[i] = str;
                }
            }
            /*
            foreach (var arg in cmdArgs)
                Debug.Log(arg);
            Debug.Log("name: " + cmdName);
            Debug.Log("val: " + cmdValue);
            */
            switch (cmdName)
			{
				case "open_travel":
					DialogueManager.instance.OpenTravel(cmdArgs[0]);
					break;
				case "open_combat":
					DialogueManager.instance.OpenCombat(cmdArgs[0]);
					break;
				case "open_inventory":
					break;
                case "roll_dice":
                    DialogueManager.instance.RollDice(int.Parse(cmdArgs[0]));
                    break;
                case "set_dialog_image":
                    DialogueManager.instance.SetDialougeImage(cmdArgs[0]);
                    break;
                case "close_dialog_image":
                    DialogueManager.instance.CloseDialougeImage();
                    break;
                case "enable_map_node":
                    DialogueManager.instance.EnableMapNode(cmdArgs[0], cmdArgs[1]);
                    break;
                case "disable_map_node":
                    DialogueManager.instance.DisableMapNode(cmdArgs[0], cmdArgs[1]);
                    break;
                case "enable_map_node_by_id":
                    DialogueManager.instance.EnableMapNodeById(cmdArgs[0], int.Parse(cmdArgs[1]));
                    break;
                case "disable_map_node_by_id":
                    DialogueManager.instance.DisableMapNodeById(cmdArgs[0], int.Parse(cmdArgs[1]));
                    break;
            }
			yield break;
		}

		public override IEnumerator RunLine(Line line)
		{
            string str = CheckVars(line.text);

            MainText.gameObject.SetActive(true);
            MainText.text = MainText.text.Replace("<color=white>", "");
            MainText.text = MainText.text.Replace("</color>", "");
            MainText.text += "<color=white>" + str + "</color>" + "\n";

			yield return new WaitForSeconds(0.01f);
			while (Input.anyKeyDown == false)
			{
				yield return null;
			}
		}

		void ClearOptionButtons()
		{
			for (int j = 0; j < OptionButtons.Count; j++)
			{
				Destroy(OptionButtons[j].gameObject);
			}
			OptionButtons.Clear();
		}

		public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
		{
			ClearOptionButtons();

			// Display each option in a button, and make it visible
			int i = 0;
			foreach (var optionString in optionsCollection.options)
			{
				GameObject go = GameObject.Instantiate(OptionPrefab, Vector3.zero, Quaternion.identity);
				go.transform.parent = OptionsParent;
				Button option = go.GetComponent<Button>();
				go.GetComponentInChildren<Text>().text = optionString;
				int x = i;
				Debug.Log("button_" + x.ToString() + ": " + optionString);
				option.onClick.AddListener(delegate { SetOption(x); });
				OptionButtons.Add(option);
				i++;
			}

			// Record that we're using it
			SetSelectedOption = optionChooser;

			// Wait until the chooser has been used and then removed (see SetOption below)
			while (SetSelectedOption != null)
			{
				yield return null;
			}
		}

		/// Called when the dialogue system has started running.
		public override IEnumerator DialogueStarted()
		{
			Debug.Log("Dialogue starting!");

			// Enable the dialogue controls.
			if (UIContainer != null)
				UIContainer.SetActive(true);

			MainText.text = "";
			yield break;
		}

		/// Called when the dialogue system has finished running.
		public override IEnumerator DialogueComplete()
		{
			Debug.Log("Complete!");

			// Hide the dialogue interface.
			if (UIContainer != null)
				UIContainer.SetActive(false);

			yield break;
		}

		/// Called by buttons to make a selection.
		public void SetOption(int selectedOption)
		{
			// Call the delegate to tell the dialogue system that we've
			// selected an option.
			SetSelectedOption(selectedOption);

			MainText.text = "";
			// Now remove the delegate so that the loop in RunOptions will exit
			SetSelectedOption = null;

			ClearOptionButtons();
		}

        //For example, in my dialog I have "Hey there [$charName_1]", and in my "Handle other variables" section, I have:
        string CheckVars(string input)
		{
			string output = string.Empty;
			bool checkingVar = false;
			string currentVar = string.Empty;

			int index = 0;
			while (index < input.Length)
			{
				if (input[index] == '[')
				{
					checkingVar = true;
					currentVar = string.Empty;
				}
				else if (input[index] == ']')
				{
					checkingVar = false;
					output += ParseVariable(currentVar);
					currentVar = string.Empty;
				}
				else if (checkingVar)
				{
					currentVar += input[index];
				}
				else
				{
					output += input[index];
				}
				index += 1;
			}

			return output;
		}

		string ParseVariable(string varName)
		{
            var variableStorage = this.GetComponent<DialogueVariableManager>();
            //Check YarnSpinner's variable storage first
            if (variableStorage.GetValue(varName) != Yarn.Value.NULL)
			{
				return variableStorage.GetValue(varName).AsString;
			}

			//Handle other variables here
			if (varName == "$time")
			{
				return Time.time.ToString();
			}

			//If no variables are found, return the variable name
			return varName;
		}

		void Start()
		{
            //StartDialogue(GameManager.instance.DialogueStartNode);
            _isStarted = false;
		}

		// Use this for initialization
		public void StartDialogue(string dialogueFile, string startNode)
		{
            _isStarted = true;
			//GameManager.instance;
			string filePath = Application.dataPath + "/" + dialogueFile;
			var runner = GetComponent<Yarn.Unity.DialogueRunner>();
			runner.StopAllCoroutines();
			runner.Stop();
			runner.Clear();
			runner.startNode = startNode;
			string text = System.IO.File.ReadAllText(filePath);
			runner.AddScript(text);
			
			runner.StartDialogue(startNode);
		}

        public void ContinueDialogue(string startNode)
        {
            if (_isStarted == false)
            {
                StartDialogue(DialogueManager.instance.DialogueFile, startNode);
            }
            else
            {
                var runner = GetComponent<Yarn.Unity.DialogueRunner>();
                runner.startNode = startNode;
                runner.StartDialogue(startNode);
            }
        }

		// Update is called once per frame
		void Update()
		{

		}
	}
}
