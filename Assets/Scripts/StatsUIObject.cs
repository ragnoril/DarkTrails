using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DarkTrails.UI
{
    /*
    public enum POINTTYPE
    {
        StatPoint = 0,
        SkillPoint,
        PointTypeCount
    }
    */
    public class StatsUIObject : MonoBehaviour
    {


        public PointsBaseWindow PointsCalc;

        public Button IncButton;
        public Button DecButton;

        public Text NameText;
        public Text ValueText;

        public string Name;
        public int Value;
        public int MaxValue;
        public int MinValue;

        //public POINTTYPE PointType;

        // Use this for initialization
        void Start()
        {
            NameText.text = Name;
            ValueText.text = Value.ToString();

        }

        void Update()
        {
            if (Value == MaxValue && IncButton.interactable)
                IncButton.interactable = false;

            if (Value < MaxValue && !IncButton.interactable)
                IncButton.interactable = true;

            if (Value == MinValue && DecButton.interactable)
                DecButton.interactable = false;

            if (Value > MinValue && !DecButton.interactable)
                DecButton.interactable = true;
        }

        public void ChangeValue(int change)
        {
            /*
            if (PointType == POINTTYPE.StatPoint)
                PointsCalc.StatPointsSpent += change;
            else if (PointType == POINTTYPE.SkillPoint)
                PointsCalc.SkillPointsSpent += change;
            */
            PointsCalc.PointsSpent += change;


            Value += change;
            if (Value > MaxValue)
            {
                change -= (Value - MaxValue);
                Value = MaxValue;
            }

            if (Value < MinValue)
            {
                change += (MinValue - Value);
                Value = MinValue;
            }

            ValueText.text = Value.ToString();

            PointsCalc.UpdateStats();
        }



    }
}
