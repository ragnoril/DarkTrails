using DarkTrails;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryAgent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public Text NameText;
	public Item ItemData; 

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("CLick");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("111"+this.ItemData.ItemName);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			//Debug.Log(this.gameObject.name);
		}
	}

	public void UpdateItemData()
	{
		NameText.text = ItemData.ItemName;
	}

	
	
}
