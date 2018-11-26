using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.Travel
{
	public class TravelConfirmUI : MonoBehaviour
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
			TravelManager.instance.SendPartyTraveling();
			this.gameObject.SetActive(false);
		}

		public void CancelTravel()
		{
			TravelManager.instance.ResetTraveling();
			this.gameObject.SetActive(false);
		}
	}
}
