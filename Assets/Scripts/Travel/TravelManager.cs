using DarkTrails;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelManager : MonoBehaviour
{
	private static TravelManager _instance;
	public static TravelManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<TravelManager>();
				//DontDestroyOnLoad(_instance.gameObject);
			}

			return _instance;
		}

	}

	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			//DontDestroyOnLoad(this);
		}
		else
		{
			if (this != _instance)
				Destroy(this.gameObject);
		}
	}

	public MapData CurrentMap;
	public AStar PathFinder;
	private LineRenderer linePathway;
	public TravelPartyAgent PlayerParty;
	public GameObject PartyPrefab;

	public TravelConfirmUI TravelConfirmPanel;
	private Vector3 _targetTravelPosition;
	private bool _isMapSelectable;

	public TravelCamera TravelCam;

	private float _unitsToPixels = 100f;

	// Use this for initialization
	void Start()
	{
		CurrentMap.LoadMap();
		linePathway = GetComponent<LineRenderer>();
		PathFinder = new AStar();
		PathFinder.SetMapSize(CurrentMap.GridWidth, CurrentMap.GridHeight);
		PathFinder.isDiagonalMovementAllowed = true;
		PathFinder.isNodeCostEnabled = true;

		_isMapSelectable = true;

		InitializePlayerParty();
	}

	public void InitializePlayerParty()
	{
		GameObject go = GameObject.Instantiate(PartyPrefab, Vector3.zero, Quaternion.identity);
		PlayerParty = go.GetComponent<TravelPartyAgent>();
		go.transform.SetParent(this.transform);
		go.transform.position = new Vector3(0, 0, 0f);
	}

	public List<int> CreateUpdatedMap(int x, int y)
	{
		List<int> updatedList = new List<int>();

		for (int i = 0; i < CurrentMap.MapGrid.Count; i++)
		{
			
			if (CurrentMap.MapGrid[i] > 0)
				updatedList.Add(CurrentMap.MapGrid[i]);
			else
				updatedList.Add(-1);
		}

		return updatedList;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && _isMapSelectable)
		{
			var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) * _unitsToPixels;
			_targetTravelPosition = pos;
			Debug.Log(pos);

			int mX = (int)(pos.x + (CurrentMap.MapWidth / 2)) / CurrentMap.GridSize;
			int mY = (int)(Mathf.Abs(pos.y - (CurrentMap.MapHeight / 2))) / CurrentMap.GridSize;

			Vector3 playerPos = PlayerParty.transform.position * _unitsToPixels;

			int pX = (int)(playerPos.x + (CurrentMap.MapWidth / 2)) / CurrentMap.GridSize;
			int pY = (int)(Mathf.Abs(playerPos.y - (CurrentMap.MapHeight / 2))) / CurrentMap.GridSize;

			ClearPathway();
			PathFinder.SetStartNode(pX, pY);
			PathFinder.SetGoalNode(mX, mY);
			PathFinder.StartSearch(CreateUpdatedMap(-1, -1));
			PathFinder.GetPath();
			//int dist = pathFinder.GetGoalScore();

			//if (dist <= selectedAgent.curActionPoints)
			MakePathway();

			TravelConfirmPanel.gameObject.SetActive(true);
			TravelConfirmPanel.TitleText.text = "Traveling will cost " + PathFinder.GetGoalScore() + " points. Do you want to start traveling?";
			_isMapSelectable = false;
		}
	}

	public void SendPartyTraveling()
	{
		ClearPathway();
		List<Vector3> path = new List<Vector3>();

		int lastPath = PathFinder.finalPath.Count - 1;
		for (int i = lastPath; i > 0; i--)
		{
			int x = PathFinder.finalPath[i].x;
			int y = PathFinder.finalPath[i].y;
			float mapx = ((x * CurrentMap.GridSize) - (CurrentMap.MapWidth / 2f)) / 100f;
			float mapy = ((CurrentMap.MapHeight / 2f) - (y * CurrentMap.GridSize)) / 100f;

			Vector3 pos = new Vector3(mapx, mapy, 0f);
			path.Add(pos);
		}
		_targetTravelPosition.z = 0f;
		//path.Add(_targetTravelPosition);

		PlayerParty.SetTargetPath(path);
		PlayerParty.StartMoving();
		TravelCam.StartFollowing(PlayerParty.gameObject);
		_isMapSelectable = true;
	}

	public void ResetTraveling()
	{
		ClearPathway();
		_targetTravelPosition = Vector3.zero;
		_isMapSelectable = true;
	}

	public void TravelActionOpenMap(string mapName)
	{
		Debug.Log("Node Action: Open Map - " + mapName);
	}

	public void TravelActionOpenDialog(string dialogName)
	{
		Debug.Log("Node Action: Open Dialog - " + dialogName);
	}

	public void TravelActionOpenCombat(string combatName)
	{
		Debug.Log("Node Action: Open Combat - " + combatName);
		//GameManager.instance.StartCombat(encounterName, winDialogue, loseDialogue);
	}

	void ClearPathway()
	{
		linePathway.positionCount = 0;
	}

	void MakePathway()
	{
		//pathWay.Clear();
		
		int lastPath = PathFinder.finalPath.Count - 1;
		linePathway.positionCount = PathFinder.finalPath.Count - 1;
		for (int i = lastPath; i > 0; i--)
		{
			int x = PathFinder.finalPath[i].x;
			int y = PathFinder.finalPath[i].y;
			float mapx = ((x * CurrentMap.GridSize) - (CurrentMap.MapWidth / 2f)) / 100f;
			float mapy = ((CurrentMap.MapHeight / 2f) - (y * CurrentMap.GridSize)) / 100f;

			Vector3 pos = new Vector3(mapx, mapy, -1f);
			linePathway.SetPosition(lastPath - i, pos);
		}

		int dist = PathFinder.GetGoalScore();
		

		

	}
}
