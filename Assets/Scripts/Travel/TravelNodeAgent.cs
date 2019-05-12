using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.Travel
{
	public enum ActionType
	{
        None = 0,
		OpenMap,
		OpenDialog,
		OpenCombat
	};

	public class TravelNodeAgent : MonoBehaviour
	{
		#region UI
		public Text NodeNameText;
		public Text NodeNameShadow;
		#endregion

		public Sprite NodeIconSprite;

		public string Name;
		public ActionType Action;
		public string ActionValue;
		public float x, y;

        public bool IsAlreadyActive;

		// Use this for initialization
		void Start()
		{
            IsAlreadyActive = false;

        }

		// Update is called once per frame
		void Update()
		{

		}

		public void UpdateNode()
		{
			NodeNameText.text = Name;
			NodeNameShadow.text = Name;
			this.GetComponent<SpriteRenderer>().sprite = NodeIconSprite;
			transform.position = new Vector3(x, y, 0f);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
            if (TravelManager.instance.IsPaused) return;

			var player = collision.gameObject.GetComponent<TravelPartyAgent>();
			if (player != null)
			{
                IsAlreadyActive = false;
				if (player.LastNodeAgent == this)
					player.LastNodeAgent = null;
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			var player = collision.gameObject.GetComponent<TravelPartyAgent>();
			if (player != null)
			{
				//toDo: another quick fix. there should be a menu/panel for  this. 
				//actually there is. make it working.
				if (player.LastNodeAgent == this) return;
                if (IsAlreadyActive) return;
                IsAlreadyActive = true;

				player.LastNodeAgent = this;
				if (Action == ActionType.OpenCombat)
				{
					TravelManager.instance.TravelActionOpenCombat(ActionValue);
				}
				else if (Action == ActionType.OpenDialog)
				{
					TravelManager.instance.TravelActionOpenDialog(ActionValue);
				}
				else if (Action == ActionType.OpenMap)
				{
					TravelManager.instance.TravelActionOpenMap(ActionValue);
				}
			}
		}

	}
}
