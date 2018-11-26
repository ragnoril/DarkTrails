namespace DarkTrails.Combat
{
	public struct EncounterData
	{
		public int[] CharacterIds;
		public StateChange WinState;
		public StateChange LoseState;
	}

	public struct StateChange
	{
		public string ModuleName;
		public string Value;
	}

}
