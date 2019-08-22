using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails.Character
{
    public class UIManager : MonoBehaviour
    {
        public List<UI.StatsUIObject> Stats;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void FillStats()
        {
            Stats[0].Name = "Strength";
            Stats[0].MaxValue = 10;
            Stats[0].MinValue = 1;
            Stats[0].UpdateUI();

            Stats[1].Name = "Strength";
            Stats[1].MaxValue = 10;
            Stats[1].MinValue = 1;
            Stats[1].UpdateUI();

            Stats[2].Name = "Strength";
            Stats[2].MaxValue = 10;
            Stats[2].MinValue = 1;
            Stats[2].UpdateUI();

            Stats[3].Name = "Strength";
            Stats[3].MaxValue = 10;
            Stats[3].MinValue = 1;
            Stats[3].UpdateUI();

            Stats[4].Name = "Strength";
            Stats[4].MaxValue = 10;
            Stats[4].MinValue = 1;
            Stats[4].UpdateUI();
        }
    }
}
