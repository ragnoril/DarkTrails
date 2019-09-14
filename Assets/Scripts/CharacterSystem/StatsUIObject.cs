using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace DarkTrails.Character
{

    public enum POINTTYPE
    {
        StatPoint = 0,
        SkillPoint,
        PointTypeCount
    }

    public class StatsUIObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Button IncButton;
        public Button DecButton;
        public GameObject ButtonPanel;

        public Text NameText;
        public Text ValueText;

        public string Name;
        public int Value;
        public int MaxValue;
        public int MinValue;
        public int PointSpent;
        public string TooltipText;

        public POINTTYPE PointType;
        public int Index;

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

            TooltipText = Name + " -> " + Value.ToString();
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

            PointSpent += change;

            ValueText.text = Value.ToString();

            UpdateUI();
            if (PointType == POINTTYPE.SkillPoint)
            {
                CharacterManager.instance.PointsToSpendForSkills -= change;
            }
            else if (PointType == POINTTYPE.StatPoint)
            {
                CharacterManager.instance.PointsToSpendForStats -= change;
                CharacterManager.instance.UiManager.UpdateSkills(this.Index, change);
            }
            CharacterManager.instance.UiManager.CheckIfPointLeft();
        }

        public void EnableEditMode(bool mode)
        {
            if (mode)
            {
                IncButton.gameObject.SetActive(true);
                DecButton.gameObject.SetActive(true);
                ButtonPanel.SetActive(true);
            }
            else
            {
                IncButton.gameObject.SetActive(false);
                DecButton.gameObject.SetActive(false);
                ButtonPanel.SetActive(false);
            }
        }

        public void SetStyle(int style)
        {
            if (style == 0) // wide mode , both textbox align opposite sides,
            {
                NameText.alignment = TextAnchor.MiddleLeft;
                ValueText.alignment = TextAnchor.MiddleRight;
            }
            else if (style == 1) // center mode, both textbox, align to the center
            {
                NameText.alignment = TextAnchor.MiddleCenter;
                ValueText.alignment = TextAnchor.MiddleCenter;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            CharacterManager.instance.UiManager.TooltipText.text = TooltipText;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CharacterManager.instance.UiManager.TooltipText.text = "";
        }
    }
}
