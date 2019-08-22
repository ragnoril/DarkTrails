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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            this.TitleText.transform.LookAt(Camera.main.transform);
        }

        public void SetTitle(string title)
        {
            TitleText.text = title;
            TitleShadowText.text = title;
        }
    }
}
