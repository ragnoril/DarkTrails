using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTrails.UI;

namespace DarkTrails.PartyManagement
{
    public class UIManager : MonoBehaviour
    {
        public GameObject RosterAgentPrefab;
        public List<RosterAgent> Roster;
        public Transform RosterTransform;


        public StatsUIObject WeeklyWages;
        public StatsUIObject FoodBurn;
        public StatsUIObject RationLeft;
        public StatsUIObject CashAmount;
        public StatsUIObject Morale;
        public StatsUIObject InventorySize;
        public StatsUIObject CartAmount;

        // Start is called before the first frame update
        void Start()
        {
            Roster = new List<RosterAgent>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ClearRoster()
        {

            foreach (Transform child in RosterTransform)
            {
                Destroy(child.gameObject);
            }

            Roster = new List<RosterAgent>();
        }

        public void InitRoster()
        {
            ClearRoster();

            foreach(int child in PartyManager.instance.PlayerParty.Roster)
            {
                GameObject go = GameObject.Instantiate(RosterAgentPrefab);
                go.transform.SetParent(RosterTransform);
                go.GetComponent<RectTransform>().localScale = Vector3.one;

                RosterAgent rosterAgent = go.GetComponent<RosterAgent>();
                rosterAgent.CharacterId = child;
                rosterAgent.InventoryId = PartyManager.instance.PlayerParty.InventoryId;
                rosterAgent.RosterId = Roster.Count;
                rosterAgent.NameText.text = Character.CharacterManager.instance.CharacterList[child].Name;
                Roster.Add(rosterAgent);
            }
        }

        public void InitUI()
        {
            WeeklyWages.Name = "Weekly Wages: ";
            WeeklyWages.Value = PartyManager.instance.PlayerParty.CalculateWeeklyWages();
            WeeklyWages.SetStyle(0);
            WeeklyWages.EnableEditMode(false);
            WeeklyWages.TooltipText = "";

            FoodBurn.Name = "Daily Food Consumption: ";
            FoodBurn.Value = PartyManager.instance.PlayerParty.CalculateDailyFoodBurn();
            FoodBurn.SetStyle(0);
            FoodBurn.EnableEditMode(false);
            FoodBurn.TooltipText = "";

            RationLeft.Name = "Rations Left: ";
            RationLeft.Value = PartyManager.instance.PlayerParty.RationsLeft;
            RationLeft.SetStyle(0);
            RationLeft.EnableEditMode(false);
            RationLeft.TooltipText = "";

            CashAmount.Name = "Silver: ";
            CashAmount.Value = PartyManager.instance.PlayerParty.CashAmount;
            CashAmount.SetStyle(0);
            CashAmount.EnableEditMode(false);
            CashAmount.TooltipText = "";

            Morale.Name = "Morale: ";
            Morale.Value = (int)PartyManager.instance.PlayerParty.Morale;
            Morale.SetStyle(0);
            Morale.EnableEditMode(false);
            Morale.TooltipText = "";

            InventorySize.Name = "Carry Weight: ";
            InventorySize.Value = (int)PartyManager.instance.PlayerParty.CalculateWeightCapacity();
            InventorySize.SetStyle(0);
            InventorySize.EnableEditMode(false);
            InventorySize.TooltipText = "";

            CartAmount.Name = "Cart Amount: ";
            CartAmount.Value = (int)PartyManager.instance.PlayerParty.CartCount;
            CartAmount.SetStyle(0);
            CartAmount.EnableEditMode(false);
            CartAmount.TooltipText = "";

            InitRoster();
        }

        public void CloseUI()
        {
            ClearRoster();

            switch (PartyManager.instance.PreviousModule)
            {
                case GAMEMODULES.OverWorld:
                    GameManager.instance.OpenOverWorld("");
                    break;
                case GAMEMODULES.Travel:
                    GameManager.instance.OpenTravel("");
                    break;
                case GAMEMODULES.Dialogue:
                    GameManager.instance.OpenDialogue("");
                    break;
                case GAMEMODULES.Combat:
                    GameManager.instance.OpenCombat("");
                    break;
                default:
                    GameManager.instance.ChangeModule(PartyManager.instance.PreviousModule);
                    break;
            }
        }
    }
}
