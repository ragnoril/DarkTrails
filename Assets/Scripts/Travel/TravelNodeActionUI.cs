using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelNodeActionUI : MonoBehaviour
{
	public Text TitleText;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void ConfirmTravel()
	{
		
		this.gameObject.SetActive(false);
	}

	public void CancelTravel()
	{
		
		this.gameObject.SetActive(false);
	}
}
