using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.Combat
{
    public class AgentVisualizer : MonoBehaviour
    {
        public Text titleText;
        public string titleName;
        public Color color;

        Renderer Renderer;
        Animator Animator;



        // Start is called before the first frame update
        void Start()
        {
            titleText.color = color;
            titleText.text = titleName;
            titleText.gameObject.SetActive(false);

            Animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetItems(int weapon, int shield)
        {
        }

        public void Attack()
        {
        }

        public void Dodge()
        {
        }

        public void GotHit()
        {
        }

        public void Die()
        {
        }

        public void Run()
        {
        }

        public void Idle()
        {
        }

        public void GotSelected(bool val)
        {
        }
    }
}
