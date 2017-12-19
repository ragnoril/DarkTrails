using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;

public class DialogueUI : Yarn.Unity.DialogueUIBehaviour
{
	public GameObject UIContainer;

	public Text MainText;
	public List<Button> OptionButtons;
	private Yarn.OptionChooser SetSelectedOption;

	public GameObject OptionPrefab;
	public Transform OptionsParent;


	public override IEnumerator RunCommand(Command command)
	{
		// "Perform" the command
		Debug.Log("Command: " + command.text);

		yield break;
	}

	public override IEnumerator RunLine(Line line)
	{
		MainText.gameObject.SetActive(true);
		MainText.text += line.text;

		yield return null;
	}

	public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
	{
		for (int j=0; j < OptionButtons.Count; j++)
		{
			Destroy(OptionButtons[j].gameObject);
		}

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
	}

	/*

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
	*/

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
