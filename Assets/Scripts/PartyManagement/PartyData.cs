using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails.PartyManagement
{

    public class PartyData
    {
        public string PartyName;
        public List<int> Roster;
        public int MainCharacterId;
        public int InventoryId;

        public int CartCount;
        public int RationsLeft;
        public float Morale;
        public int CashAmount;


        public PartyData()
        {
            Roster = new List<int>();
        }

        public int CalculateWeeklyWages()
        {
            return Roster.Count * 100;
        }

        public int CalculateDailyFoodBurn()
        {
            return (Roster.Count * 3) + (CartCount * 5);
        }

        public float CalculateWeightCapacity()
        {
            return (Roster.Count * 5f) + (CartCount * 50f);
        }

        public Character.CharacterData GetCharacterFromRoster(int val)
        {
            return Character.CharacterManager.instance.CharacterList[Roster[val]];
        }

        public int PartyMaxSize()
        {
            //Character.CharacterManager.instance.CharacterList[MainCharacterId].PartySize;
            return 5;
        }
    }
}
