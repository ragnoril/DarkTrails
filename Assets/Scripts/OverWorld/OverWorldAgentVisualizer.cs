using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.OverWorld
{
    public class OverWorldAgentVisualizer : MonoBehaviour
    {
        public Text TitleText;
        public Text TitleShadowText;
        public Animator Animator;
        public GameObject SelectionCircle;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            this.TitleText.transform.LookAt(Camera.main.transform);
            //this.TitleShadowText.transform.LookAt(Camera.main.transform);
        }

        public void SetTitle(string title)
        {
            TitleText.text = title;
            //TitleShadowText.text = title;
        }

        public void Walking()
        {
            Animator.SetBool("isWalking", true);
        }

        public void StopWalking()
        {
            Animator.SetBool("isWalking", false);
        }

        void OnMouseOver()
        {
            //Debug.Log("111" + this.gameObject.name);
            SelectionCircle.SetActive(true);
        }

        void OnMouseExit()
        {
            //Debug.Log("222" + this.gameObject.name);
            SelectionCircle.SetActive(false);
        }
    }
}
