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

		// Use this for initialization
		void Start()
		{
			//Renderer = GetComponent<Renderer>();
			//Renderer.material.color = color;
			titleText.color = color;
			titleText.text = titleName;

			Animator = GetComponent<Animator>();
		}

		// Update is called once per frame
		void Update()
		{

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
		}

		public void Die()
		{
			Animator.SetTrigger("Die");
		}

		public void Run()
		{
			Animator.SetTrigger("Run");
		}

		public void Idle()
		{
			Animator.SetTrigger("Idle");
		}
	}
}
