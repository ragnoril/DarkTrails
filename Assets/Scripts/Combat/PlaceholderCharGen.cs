using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.Combat
{
	public class PlaceholderCharGen : MonoBehaviour
	{
		public Text titleText;
		public string titleName;
		public Color color;

		Renderer Renderer;
		Animator Animator;

		public GameObject SwordObject;
		public GameObject AxeObject;
		public GameObject SpearObject;
		public GameObject ShieldObject;

        public SpriteColorFX.SpriteColorRamp Ramp;
        public SpriteColorFX.SpriteColorOutline Outline;
        private float _rampTimer;

        // Use this for initialization
        void Start()
		{ 
			titleText.color = color;
			titleText.text = titleName;
            titleText.gameObject.SetActive(false);

            //for 2d
            //SetZOrder();
            //Ramp = GetComponentInChildren<SpriteColorFX.SpriteColorRamp>();
            //Outline = GetComponentInChildren<SpriteColorFX.SpriteColorOutline>();

            //for 3d 
            Animator = GetComponent<Animator>();
        }

		// Update is called once per frame
		void Update()
		{
            //for 2d
            /*
            if (_rampTimer >= 0f)
            {
                _rampTimer -= Time.deltaTime;
            }
            else if (_rampTimer < 0f)
            {
                Ramp.enabled = false;
            }
            */
		}

        public void SetZOrder()
        {
            var agent = GetComponent<Agent>();
            var sprite = GetComponentInChildren<SpriteRenderer>();
            var canvas = GetComponentInChildren<Canvas>();

            sprite.sortingOrder = CombatManager.instance.mapManager.mapHeight - agent.y;
            canvas.sortingOrder = CombatManager.instance.mapManager.mapHeight - agent.y;
        }

		public void SetItems(int weapon, int shield)
		{
			SwordObject.SetActive(false);
			AxeObject.SetActive(false);
			SpearObject.SetActive(false);
			if (weapon == 0)
			{
				SwordObject.SetActive(true);
			}
			else if (weapon == 1)
			{
				SpearObject.SetActive(true);
			}
			if (weapon == 2)
			{
				AxeObject.SetActive(true);
			}

			if (shield == 0)
				ShieldObject.SetActive(false);
			else
				ShieldObject.SetActive(true);

		}

		public void Attack()
		{
            //for 3d
			if (SwordObject.activeSelf)
				Animator.SetTrigger("Atack_0");
			else if (SpearObject.activeSelf)
				Animator.SetTrigger("Atack_1");
			else
				Animator.SetTrigger("Atack_2");
               
		}

		public void Dodge()
		{
			Animator.SetTrigger("Gd");
		}

		public void GotHit()
		{
            Animator.SetTrigger("Gd_1");
            /*
            Ramp.enabled = true;
            _rampTimer = 1f;
            */
		}

		public void Die()
		{
			Animator.SetTrigger("Die");
		}

		public void Run()
		{
            Animator.SetTrigger("Run");
            //SetZOrder();
		}

		public void Idle()
		{
			Animator.SetTrigger("Idle");
		}

        public void GotSelected(bool val)
        {
            //Outline.enabled = val;
        }

	}
}
