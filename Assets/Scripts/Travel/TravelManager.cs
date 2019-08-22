using DarkTrails;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace DarkTrails.Travel
{
    public enum TRAVELMODES
    {
        GridBased = 0,
        NodeBased,
        PixelBased,
        ModeCount
    }

	public class TravelManager : BaseModule
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

        public TRAVELMODES TravelMode;
		public MapData CurrentMap;
		public AStar PathFinder;
		private LineRenderer linePathway;
		public TravelPartyAgent PlayerParty;
		public GameObject PartyPrefab;
        public GameObject MapPrefab;
        public Transform MapsTransform;

		public TravelConfirmUI TravelConfirmPanel;
		private Vector3 _targetTravelPosition;
		private bool _isMapSelectable;
        public bool IsPlayerMoving;

		public TravelCamera TravelCam;

        private Dictionary<string, MapData> LoadedMaps;
        public bool IsPaused;
        public bool ShowMapGizmos;

        private float _unitsToPixels = 100f;

		// Use this for initialization
		void Start()
		{
			ModuleType = GAMEMODULES.Travel;
		}

		public void InitializePlayerParty()
		{
			//toDo: another quick fix :(.
			if (PlayerParty == null)
			{
				GameObject go = GameObject.Instantiate(PartyPrefab, Vector3.zero, Quaternion.identity);
				PlayerParty = go.GetComponent<TravelPartyAgent>();
				go.transform.SetParent(this.transform);
				go.transform.position = new Vector3(CurrentMap.PlayerX, CurrentMap.PlayerY, 0f);
			}
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
            switch(TravelMode)
            {
                case TRAVELMODES.GridBased:
                    UpdateGridBasedTravel();
                    break;
                case TRAVELMODES.NodeBased:
                    UpdateNodeBasedTravel();
                    break;
                case TRAVELMODES.PixelBased:
                    UpdatePixelBasedTravel();
                    break;
                default:
                    // for now lets keep it this way.
                    UpdateGridBasedTravel();
                    break;
            }

			
		}

        public void UpdateNodeBasedTravel()
        {

        }

        public void UpdatePixelBasedTravel()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) * _unitsToPixels;
                _targetTravelPosition = pos;


                int mX = (int)(pos.x + (CurrentMap.MapWidth / 2)) / CurrentMap.GridSize;
                int mY = (int)(Mathf.Abs(pos.y - (CurrentMap.MapHeight / 2))) / CurrentMap.GridSize;

                Vector3 playerPos = PlayerParty.transform.position * _unitsToPixels;

                int pX = (int)(playerPos.x + (CurrentMap.MapWidth / 2)) / CurrentMap.GridSize;
                int pY = (int)(Mathf.Abs(playerPos.y - (CurrentMap.MapHeight / 2))) / CurrentMap.GridSize;

                //Debug.Log("pos: " + pos.ToString() + " mx: " + mX.ToString() + " my: " + mY.ToString() + " px: " + pX.ToString() + " py: " + pY.ToString());

                ClearPathway();
                PathFinder.SetStartNode(pX, pY);
                PathFinder.SetGoalNode(mX, mY);
                PathFinder.StartSearch(CreateUpdatedMap(-1, -1));
                PathFinder.GetPath();
                int dist = PathFinder.GetGoalScore();

                if (dist > 0)
                    SendPartyTraveling();

                IsPlayerMoving = true;
            }

            if (IsPlayerMoving)
            {
                UpdateAI();
            }
        }

        public void UpdateAI()
        {
            switch (TravelMode)
            {
                case TRAVELMODES.GridBased:
                    UpdateGridBasedAI();
                    break;
                case TRAVELMODES.NodeBased:
                    UpdateNodeBasedAI();
                    break;
                case TRAVELMODES.PixelBased:
                    UpdatePixelBasedAI();
                    break;
                default:
                    // for now lets keep it this way.
                    UpdateGridBasedAI();
                    break;
            }
        }

        public void UpdateGridBasedAI()
        {

        }

        public void UpdateNodeBasedAI()
        {

        }

        public void UpdatePixelBasedAI()
        {
            if (IsPlayerMoving)
            {
                //ai works, else don't
            }
        }

        public void UpdateGridBasedTravel()
        {
            if (Input.GetMouseButtonDown(0) && _isMapSelectable)
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) * _unitsToPixels;
                _targetTravelPosition = pos;
                

                int mX = (int)(pos.x + (CurrentMap.MapWidth / 2)) / CurrentMap.GridSize;
                int mY = (int)(Mathf.Abs(pos.y - (CurrentMap.MapHeight / 2))) / CurrentMap.GridSize;

                Vector3 playerPos = PlayerParty.transform.position * _unitsToPixels;

                int pX = (int)(playerPos.x + (CurrentMap.MapWidth / 2)) / CurrentMap.GridSize;
                int pY = (int)(Mathf.Abs(playerPos.y - (CurrentMap.MapHeight / 2))) / CurrentMap.GridSize;

                Debug.Log("pos: "+ pos.ToString() + " mx: " + mX.ToString() + " my: " + mY.ToString() + " px: " + pX.ToString() + " py: " + pY.ToString() );

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
				float mapx = ((x * CurrentMap.GridSize) - (CurrentMap.MapWidth / 2f)) / _unitsToPixels;
				float mapy = ((CurrentMap.MapHeight / 2f) - (y * CurrentMap.GridSize)) / _unitsToPixels;

				Vector3 pos = new Vector3(mapx, mapy, 0f);
				path.Add(pos);
			}
            var lastPos = _targetTravelPosition / _unitsToPixels;
            lastPos.z = 0f;
            path.Add(lastPos);

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

        public void TravelActionOpenScene(string sceneName)
        {
            Debug.Log("Node Action: Open Scene - " + sceneName);
        }

        public void TravelActionOpenDialog(string dialogName)
		{
			Debug.Log("Node Action: Open Dialog - " + dialogName);
			GameManager.instance.OpenDialogue(dialogName);
		}

		public void TravelActionOpenCombat(string combatName)
		{
			Debug.Log("Node Action: Open Combat - " + combatName);
			GameManager.instance.OpenCombat(combatName);
		}

        public void TravelActionEnableNode(string mapName, string nodeName)
        {
            LoadedMaps[mapName].SetNodeState(nodeName, true);
        }

        public void TravelActionDisableNode(string mapName, string nodeName)
        {
            LoadedMaps[mapName].SetNodeState(nodeName, false);
        }

        public void TravelActionEnableNodeById(string mapName, int id)
        {
            LoadedMaps[mapName].SetNodeStateById(id, true);
        }

        public void TravelActionDisableNodeById(string mapName, int id)
        {
            var map = LoadedMaps[mapName];
            map.SetNodeStateById(id, false);
        }

        void ClearPathway()
		{
			linePathway.positionCount = 0;
		}

		void MakePathway()
		{
			//pathWay.Clear();

			int lastPath = PathFinder.finalPath.Count - 1;
            linePathway.positionCount = PathFinder.finalPath.Count;
            for (int i = lastPath; i > 0; i--)
			{
				int x = PathFinder.finalPath[i].x;
				int y = PathFinder.finalPath[i].y;
				float mapx = ((x * CurrentMap.GridSize) - (CurrentMap.MapWidth / 2f)) / _unitsToPixels;
				float mapy = ((CurrentMap.MapHeight / 2f) - (y * CurrentMap.GridSize)) / _unitsToPixels;

				Vector3 pos = new Vector3(mapx, mapy, -1f);
				linePathway.SetPosition(lastPath - i, pos);
			}
            var lastPos = _targetTravelPosition / _unitsToPixels;
            lastPos.z = -1f;
            linePathway.SetPosition(lastPath, lastPos);
            var firstPos = PlayerParty.transform.position;
            firstPos.z = -1f;
            linePathway.SetPosition(0, firstPos);

            //int dist = PathFinder.GetGoalScore();
		}

		public override void Initialize(string filename)
		{
			string filePath = Application.dataPath + "/" + filename;

			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			XmlNode root = doc.SelectSingleNode("MapList");
			XmlNodeList mapList = root.SelectNodes(".//Map");

			foreach (XmlNode map in mapList)
			{
				string mapName = map.Attributes["name"].Value;
				string mapFileName = map.Attributes["filename"].Value;

				GameManager.instance.MapList.Add(mapName, mapFileName);
			}

            LoadedMaps = new Dictionary<string, MapData>();
		}

		public void LoadMap(string mapName)
		{
            if (!LoadedMaps.ContainsKey(mapName))
            {
                string filename = GameManager.instance.MapList[mapName];
                GameObject goMap = GameObject.Instantiate(MapPrefab);
                goMap.transform.SetParent(MapsTransform);
                CurrentMap = goMap.GetComponent<MapData>();
                CurrentMap.LoadMap(filename);
                linePathway = GetComponent<LineRenderer>();
                PathFinder = new AStar();
                PathFinder.SetMapSize(CurrentMap.GridWidth, CurrentMap.GridHeight);
                PathFinder.isDiagonalMovementAllowed = true;
                PathFinder.isNodeCostEnabled = true;
                LoadedMaps.Add(mapName, CurrentMap);

                var cam = TravelCam.GetComponent<TravelCamera>();
                cam.CalculateMapBounds(CurrentMap.MapBackgroundObject);

                _isMapSelectable = true;
            }
            else
            {
                if (CurrentMap != null)
                {
                    CurrentMap.gameObject.SetActive(false);
                }

                CurrentMap = LoadedMaps[mapName];
                CurrentMap.gameObject.SetActive(true);

                PathFinder = new AStar();
                PathFinder.SetMapSize(CurrentMap.GridWidth, CurrentMap.GridHeight);
                PathFinder.isDiagonalMovementAllowed = true;
                PathFinder.isNodeCostEnabled = true;

                var cam = TravelCam.GetComponent<TravelCamera>();
                cam.CalculateMapBounds(CurrentMap.MapBackgroundObject);

                _isMapSelectable = true;
            }
			InitializePlayerParty();
		}

		public override void Pause()
		{
			base.Pause();
            IsPaused = true;
		}

		public override void Resume()
		{
			base.Resume();
            IsPaused = false;
			if (TravelCam == null)
				TravelCam = Camera.main.GetComponent<TravelCamera>();
		}

		public override void Quit()
		{
			base.Quit();
		}

        void OnDrawGizmos()
        {

            // unfortunately tilemap is empty when this function is working so every tile is green.
            if (ShowMapGizmos)
            {
                float halfWidth = CurrentMap.GridWidth / 2;
                float halfHeight = CurrentMap.GridHeight / 2;
                Gizmos.color = Color.green;
                for (int i = 0; i < CurrentMap.GridWidth; i++)
                {
                    for (int j = 0; j < CurrentMap.GridHeight; j++)
                    {

                        Vector3 start = new Vector3((i - halfWidth - 0.5f) / 3.2f, (j - halfHeight - 0.5f) / 3.2f, 0f);

                        Gizmos.DrawLine(start, start + new Vector3(1f/3.2f, 0f, 0f));
                        Gizmos.DrawLine(start, start + new Vector3(0f, 1f / 3.2f, 0f));
                        Gizmos.DrawLine(start + new Vector3(1f / 3.2f, 0f, 0f), start + new Vector3(1f / 3.2f, 1f / 3.2f, 0f));
                        Gizmos.DrawLine(start + new Vector3(0f, 1f / 3.2f, 0f), start + new Vector3(1f / 3.2f, 1f / 3.2f, 0f));

                    }
                }
            }

        }
    }
}
