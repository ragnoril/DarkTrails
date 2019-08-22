using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DarkTrails.UI
{

    public enum POINTTYPE
    {
        StatPoint = 0,
        SkillPoint,
        PointTypeCount
    }

    public class StatsUIObject : MonoBehaviour
    {
        public Button IncButton;
        public Button DecButton;

        public Text NameText;
        public Text ValueText;

        public string Name;
        public int Value;
        public int MaxValue;
        public int MinValue;

        public POINTTYPE PointType;

        // Use this for initialization
        void Start()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            NameText.text = Name;
            ValueText.text = Value.ToString();

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

            UpdateUI();
        }

        public void EnableEditMode(bool mode)
        {
            if (mode)
            {
                IncButton.gameObject.SetActive(true);
                DecButton.gameObject.SetActive(true);
            }
            else
            {
                IncButton.gameObject.SetActive(false);
                DecButton.gameObject.SetActive(false);
            }
        }



    }
}
