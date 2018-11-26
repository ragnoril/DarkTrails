using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;

namespace DarkTrails.Combat
{
	public class CombatManager : BaseModule
	{
		private static CombatManager _instance;

		public static CombatManager instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = GameObject.FindObjectOfType<CombatManager>();
					//DontDestroyOnLoad(_instance.gameObject);
				}

				return _instance;
			}
		}

		public string EncounterName;

		public GameObject[] agentPrefabs;
		public GameObject agentTilePrefab;
		public GameObject moveTilePrefab;
		public GameObject cantMoveTilePrefab;
		public Texture2D mouseCursor;
		public Texture2D attackCursor;
		public Texture2D selectCursor;

		public MapManager mapManager;
		public UIManager uiManager;

		public bool enableDiagonalMove;

		public List<Agent> teamPlayer;
		public List<Agent> teamEnemy;
		public List<Agent> initiativeList;
		public int turnIndex;
		public int turnId;

		public int WhoWon;
		private bool _hasBattleEnded;

		public List<GameObject> pathWay;
		public AStar pathFinder;

		public Agent selectedAgent;

		public GameObject agentTile;

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

		// Use this for initialization
		void Start()
		{
			ModuleType = GAMEMODULES.CombatTest;

            //StartTheGameTest();
		}

		public void StartTheGame()
		{
			//FillTeams();
			//GenerateBattleTeams();
			mapManager.StartMap();
			PrepareEncounter();
			CalculateInitiatives();

			agentTile = GameObject.Instantiate(agentTilePrefab, Vector3.zero, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
			turnIndex = -1;
			selectedAgent = null;
			/*
			selectedAgent = initiativeList[turnIndex];
			Vector3 tilePos = selectedAgent.transform.position;
			tilePos.y = 0.015f;
			agentTile = GameObject.Instantiate(agentTilePrefab, tilePos, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
			*/

		}

        public void StartTheGameTest()
        {
            
            //GenerateBattleTeams();
            mapManager.StartMap();
            FillTeams();
            //PrepareEncounter();
            CalculateInitiativesTest();

            agentTile = GameObject.Instantiate(agentTilePrefab, Vector3.zero, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            turnIndex = -1;
            selectedAgent = teamPlayer[0];
            /*
			selectedAgent = initiativeList[turnIndex];
			Vector3 tilePos = selectedAgent.transform.position;
			tilePos.y = 0.015f;
			agentTile = GameObject.Instantiate(agentTilePrefab, tilePos, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
			*/

        }


        public void PassTurn()
		{
			turnIndex += 1;
			if (turnIndex == initiativeList.Count)
			{
				turnIndex = 0;
				// initiatives won't be calculated every turn for now
				// maybe later there will be something for you to lose initiative etc.
				//CalculateInitiatives();
			}

			selectedAgent = initiativeList[turnIndex];
			selectedAgent.isTurnEnded = false;
            //selectedAgent.curActionPoints = selectedAgent.maxActionPoints;
            selectedAgent.curActionPoints = 10;
            Vector3 tilePos = selectedAgent.transform.position;
			//tilePos.y = 0.015f;
			agentTile.transform.position = tilePos;

			UpdateUI();

			if (selectedAgent.teamId == 1)  // enemy team
			{
				selectedAgent.MakeAIMove();
			}
		}

		public void UpdateUI()
		{
			uiManager.UpdateAgentData();
		}

		private void CalculateInitiatives()
		{
			initiativeList = new List<Agent>();

			foreach (Agent agent in teamPlayer)
			{
				int i = 0;
				for (i = 0; i < initiativeList.Count; i++)
				{
					if (agent.initiative > initiativeList[i].initiative)
						break;
				}

				initiativeList.Insert(i, agent);
			}

			foreach (Agent agent in teamEnemy)
			{
				int i = 0;
				for (i = 0; i < initiativeList.Count; i++)
				{
					if (agent.initiative > initiativeList[i].initiative)
						break;
				}

				initiativeList.Insert(i, agent);
			}

		}

        private void CalculateInitiativesTest()
        {
            initiativeList = new List<Agent>();

            
            foreach (Agent agent in teamPlayer)
            {              
                initiativeList.Add(agent);
            }

            foreach (Agent agent in teamEnemy)
            {
                initiativeList.Add(agent);
            }

        }

        private void PrepareEncounter()
		{
			foreach (int id in GameManager.instance.PlayerParty)
			{
				//int posX = (mapManager.mapWidth / 2) - (2 - teamPlayer.Count);
				//int posY = 0;
                int posX = 0;
                int posY = (mapManager.mapHeight / 2) - (2 - teamPlayer.Count);
                Vector3 pos = new Vector3(posX - mapManager.halfMapWidth, posY - mapManager.halfMapHeight, 0f);
				GameObject go = GameObject.Instantiate(agentPrefabs[0], pos, Quaternion.identity) as GameObject;
				go.transform.parent = transform;

				Agent goAgent = go.GetComponent<Agent>();
				goAgent.AssignCharacterData(GameManager.instance.CharacterList[id]);
				goAgent.x = posX;
				goAgent.y = posY;
				goAgent.teamId = 0;

				go.name = "Player_" + GameManager.instance.CharacterList[id].Name;

				teamPlayer.Add(goAgent);

				PlaceholderCharGen placeHolderChar = go.GetComponent<PlaceholderCharGen>();
				placeHolderChar.titleName = goAgent.CharacterName;
				//placeHolderChar.titleText.text = goAgent.CharacterName;
				//placeHolderChar.SetItems(goAgent.WeaponType, goAgent.ShieldType);
				goAgent.ModelAgent = placeHolderChar;
			}

			int[] encounterList = GameManager.instance.EncounterList[this.EncounterName].CharacterIds;
			foreach (int id in encounterList)
			{
				//int posX = (mapManager.mapWidth / 2) - (2 - teamEnemy.Count);
				//int posY = mapManager.mapHeight - 1;
                int posX = mapManager.mapWidth - 1;
                int posY = (mapManager.mapHeight / 2) - (2 - teamEnemy.Count);
                Vector3 pos = new Vector3(posX - mapManager.halfMapWidth, posY - mapManager.halfMapHeight, 0f);
				GameObject go = GameObject.Instantiate(agentPrefabs[1], pos, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
				go.transform.parent = transform;

				Agent goAgent = go.GetComponent<Agent>();
				goAgent.AssignCharacterData(GameManager.instance.CharacterList[id]);
				goAgent.x = posX;
				goAgent.y = posY;
				goAgent.teamId = 1;

				go.name = "Enemy_" + GameManager.instance.CharacterList[id].Name;

				teamEnemy.Add(goAgent);

				PlaceholderCharGen placeHolderChar = go.GetComponent<PlaceholderCharGen>();
				placeHolderChar.titleName = goAgent.CharacterName;
				//placeHolderChar.titleText.text = goAgent.CharacterName;
				//placeHolderChar.SetItems(goAgent.WeaponType, goAgent.ShieldType);
				goAgent.ModelAgent = placeHolderChar;
			}
		}

		private void GenerateBattleTeams()
		{
			foreach (int id in BattleManager.instance.TeamA)
			{
				int posX = (mapManager.mapWidth / 2) - (2 - teamPlayer.Count);
				int posY = 0;
				Vector3 pos = new Vector3(posX - mapManager.halfMapWidth, 0f, posY - mapManager.halfMapHeight);
				GameObject go = GameObject.Instantiate(agentPrefabs[0], pos, Quaternion.identity) as GameObject;
				go.transform.parent = transform;

				Agent goAgent = go.GetComponent<Agent>();
				goAgent.AssignCharacterData(BattleManager.instance.CharacterList[id]);
				goAgent.x = posX;
				goAgent.y = posY;
				goAgent.teamId = 0;

				go.name = "Player_" + BattleManager.instance.CharacterList[id].Name;

				teamPlayer.Add(goAgent);

				PlaceholderCharGen placeHolderChar = go.GetComponent<PlaceholderCharGen>();
				placeHolderChar.titleName = goAgent.CharacterName;
				//placeHolderChar.titleText.text = goAgent.CharacterName;
				placeHolderChar.SetItems(goAgent.WeaponType, goAgent.ShieldType);
				goAgent.ModelAgent = placeHolderChar;
			}

			foreach (int id in BattleManager.instance.TeamB)
			{
				int posX = (mapManager.mapWidth / 2) - (2 - teamEnemy.Count);
				int posY = mapManager.mapHeight - 1;
				Vector3 pos = new Vector3(posX - mapManager.halfMapWidth, 0f, posY - mapManager.halfMapHeight);
				GameObject go = GameObject.Instantiate(agentPrefabs[1], pos, Quaternion.Euler(0f, 180f, 0f)) as GameObject;
				go.transform.parent = transform;

				Agent goAgent = go.GetComponent<Agent>();
				goAgent.AssignCharacterData(BattleManager.instance.CharacterList[id]);
				goAgent.x = posX;
				goAgent.y = posY;
				goAgent.teamId = 1;

				go.name = "Enemy_" + BattleManager.instance.CharacterList[id].Name;

				teamEnemy.Add(goAgent);

				PlaceholderCharGen placeHolderChar = go.GetComponent<PlaceholderCharGen>();
				placeHolderChar.titleName = goAgent.CharacterName;
				//placeHolderChar.titleText.text = goAgent.CharacterName;
				placeHolderChar.SetItems(goAgent.WeaponType, goAgent.ShieldType);
				goAgent.ModelAgent = placeHolderChar;
			}
		}

		private void FillTeams()
		{
			for (int i = 0; i < 8; i++)
			{
				int teamid = i % 2;

				if (teamid == 0)
				{
                    //int posX = (mapManager.mapWidth / 2) - (2 - teamPlayer.Count);
                    //int posY = 0;
                    int posX = 0;
                    int posY = (mapManager.mapHeight / 2) - (2 - teamPlayer.Count);
                    Vector3 pos = new Vector3(posX - mapManager.halfMapWidth, posY - mapManager.halfMapHeight, 0f);
					GameObject go = GameObject.Instantiate(agentPrefabs[0], pos, Quaternion.identity) as GameObject;
					go.transform.parent = transform;

					Agent goAgent = go.GetComponent<Agent>();
					goAgent.x = posX;
					goAgent.y = posY;
					goAgent.teamId = teamid;
					//goAgent.initiative = UnityEngine.Random.Range(3, 10);


					teamPlayer.Add(goAgent);
				}
				else if (teamid == 1)
				{
					//int posX = (mapManager.mapWidth / 2) - (2 - teamEnemy.Count);
					//int posY = mapManager.mapHeight - 1;

                    int posX = mapManager.mapWidth - 1; 
                    int posY = (mapManager.mapHeight / 2) - (2 - teamEnemy.Count);
                    Vector3 pos = new Vector3(posX - mapManager.halfMapWidth, posY - mapManager.halfMapHeight, 0f);
					GameObject go = GameObject.Instantiate(agentPrefabs[1], pos, Quaternion.identity) as GameObject;
					go.transform.parent = transform;

					Agent goAgent = go.GetComponent<Agent>();
					goAgent.x = posX;
					goAgent.y = posY;
					goAgent.teamId = teamid;
					//goAgent.initiative = UnityEngine.Random.Range(3, 10);

					teamEnemy.Add(goAgent);
				}


			}
		}

		void ClearPathWay()
		{
			for (int i = 0; i < pathWay.Count; i++)
			{
				Destroy(pathWay[i]);

			}
			pathWay.Clear();
		}

		void MakePathWay()
		{
			//pathWay.Clear();
			int lastPath = pathFinder.finalPath.Count - 1;
			for (int i = lastPath; i > 0; i--)
			{
				GameObject cursorTile = moveTilePrefab;

				if (i <= selectedAgent.curActionPoints)
					cursorTile = moveTilePrefab;
				else
					cursorTile = cantMoveTilePrefab;

				Vector3 pos = new Vector3((float)(pathFinder.finalPath[lastPath - i].x - mapManager.halfMapWidth), (float)(pathFinder.finalPath[lastPath - i].y - mapManager.halfMapHeight), 0f);
				GameObject pathBase = GameObject.Instantiate(cursorTile, pos, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
				pathBase.gameObject.name = "PathWay_" + i.ToString();

				pathWay.Add(pathBase);

			}

			int dist = pathFinder.GetGoalScore();
			if (pathWay.Count < 1) return;

			if (pathWay[0].GetComponentInChildren<Text>() != null)
				pathWay[0].GetComponentInChildren<Text>().text = dist.ToString();


		}

		public List<int> CreateUpdatedMap(int x, int y)
		{
			List<int> updatedList = new List<int>();

			for (int i = 0; i < mapManager.Grids.Count; i++)
			{
				//toDo: another quick fix. travel map and combat map grids have different formats
				// things getting messy, need to fix that.
				if (mapManager.Grids[i] == 0)
					updatedList.Add(1);
				if (mapManager.Grids[i] == -1)
					updatedList.Add(0);
			}

			for (int i = 0; i < teamPlayer.Count; i++)
			{
				if (teamPlayer[i].isDead)
					continue;

				if (pathFinder.goalNode.x == teamPlayer[i].x && pathFinder.goalNode.y == teamPlayer[i].y)
					continue;

				updatedList[(teamPlayer[i].y * mapManager.mapWidth) + teamPlayer[i].x] = -2;
			}

			for (int i = 0; i < teamEnemy.Count; i++)
			{
				if (teamEnemy[i].isDead)
					continue;

				if (pathFinder.goalNode.x == teamEnemy[i].x && pathFinder.goalNode.y == teamEnemy[i].y)
					continue;

				updatedList[(teamEnemy[i].y * mapManager.mapWidth) + teamEnemy[i].x] = -3;
			}

			if ((x != -1) && (y != -1))
			{
				updatedList[(y * mapManager.mapWidth) + x] = 0;
			}

			return updatedList;
		}

        void MouseTest()
        {
            Debug.Log("pos: " + Camera.main.ScreenPointToRay(Input.mousePosition));
        }

		// Update is called once per frame
		void Update()
		{
            // if combat module is not the current game module, don't run update method
            //if (GameManager.instance.CurrentGameModule != this) return;

			Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);

			CheckIfTheBattleEnded();

			if (_hasBattleEnded)
			{
				EndTheGame();
				return;
			}
			if (turnIndex == -1) // first turn
			{
				PassTurn();
			}

			if (selectedAgent.teamId == 0) // players turn
			{
				if (selectedAgent == null) return;

                //PlayerUpdate3D();

                PlayerUpdate2D();
			}
			else if (turnId == 1) // enemy turn
			{

			}

		}

        private void PlayerUpdate2D()
        {
            GraphicRaycaster gr = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
            PointerEventData ped = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            ped.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            gr.Raycast(ped, raycastResults);

            if (raycastResults.Count > 0)
            {
                ClearPathWay();
                return;
            }

            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            /*
            Vector3 mPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int mapX = (int)Mathf.Round(mPoint.x + mapManager.halfMapWidth);
            int mapY = (int)Mathf.Round(mPoint.y + mapManager.halfMapHeight);

            Debug.Log("x: " + mapX + " y: " + mapY);
            */
            RaycastHit2D[] hitArray = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            foreach (var hitInfo in hitArray)
            {
                if (hitInfo.transform.tag == "Agent")
                {
                    //ClearPathWay();
                    Agent cursorAgent = hitInfo.transform.gameObject.GetComponent<Agent>();

                    ClearPathWay();

                    if (cursorAgent.teamId == 0)
                    {
                        Vector3 posEnd = new Vector3((float)(cursorAgent.x - mapManager.halfMapWidth), (float)(cursorAgent.y - mapManager.halfMapHeight), 0f);
                        GameObject pathEnd = GameObject.Instantiate(agentTilePrefab, posEnd, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
                        pathEnd.gameObject.name = "PathWay_End"; // + (pathFinder.finalPath.Count - 1).ToString();
                        pathWay.Add(pathEnd);
                        Cursor.SetCursor(selectCursor, Vector2.zero, CursorMode.Auto);
                    }
                    else if (cursorAgent.teamId == 1)
                    {
                        Vector3 posEnd = new Vector3((float)(cursorAgent.x - mapManager.halfMapWidth), (float)(cursorAgent.y - mapManager.halfMapHeight), 0f);
                        GameObject pathEnd = GameObject.Instantiate(cantMoveTilePrefab, posEnd, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
                        pathEnd.gameObject.name = "PathWay_End"; // + (pathFinder.finalPath.Count - 1).ToString();
                        pathWay.Add(pathEnd);
                        Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
                    }

                    break;
                }
                else if (hitInfo.transform.tag == "Ground")
                {
                    int mapX = (int)Mathf.Round(hitInfo.point.x + mapManager.halfMapWidth);
                    int mapY = (int)Mathf.Round(hitInfo.point.y + mapManager.halfMapHeight);

                    Debug.Log("Coords: " + mapX.ToString() + ",  " + mapY.ToString());
                    if (((mapX > -1 && mapX < mapManager.mapWidth) && (mapY > -1 && mapY < mapManager.mapHeight)) && (selectedAgent.doneMoving))
                    {

                        //Debug.Log(mapX.ToString() + ",  " + mapY.ToString());

                        ClearPathWay();
                        pathFinder.SetStartNode(selectedAgent.x, selectedAgent.y);
                        pathFinder.SetGoalNode(mapX, mapY);
                        pathFinder.StartSearch(CreateUpdatedMap(-1, -1));
                        pathFinder.GetPath();
                        //int dist = pathFinder.GetGoalScore();

                        //if (dist <= selectedAgent.curActionPoints)
                        MakePathWay();

                    }
                    else
                    {
                        ClearPathWay();
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (!selectedAgent.doneMoving) return;

                RaycastHit2D[] hitArray2 = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                Agent agent = IsClickedOnAgent(hitArray2);

                if (agent != null)
                {
                    if (Vector3.Distance(selectedAgent.transform.position, agent.transform.position) < 2f)
                    {
                        if (selectedAgent.curActionPoints >= 4)
                            selectedAgent.AttackTo(agent);
                    }
                    else
                    {
                        pathFinder.SetStartNode(selectedAgent.x, selectedAgent.y);
                        pathFinder.SetGoalNode(agent.x, agent.y);
                        pathFinder.StartSearch(CreateUpdatedMap(-1, -1));
                        pathFinder.GetPath();


                        if (pathFinder.GetGoalScore() > 1)
                        {
                            int newX = pathFinder.finalPath[1].x;
                            int newY = pathFinder.finalPath[1].y;

                            pathFinder.SetStartNode(selectedAgent.x, selectedAgent.y);
                            pathFinder.SetGoalNode(newX, newY);
                            pathFinder.StartSearch(CreateUpdatedMap(-1, -1));
                            pathFinder.GetPath();
                        }


                        int dist = pathFinder.GetGoalScore() + 4;

                        if (dist <= selectedAgent.curActionPoints)
                        {
                            selectedAgent.MovePathAndAttack(pathFinder.finalPath, agent);
                        }
                    }
                }
                else
                {
                    foreach (var hitInfo2 in hitArray2)
                    {
                        if (hitInfo2.transform.tag == "Ground" && selectedAgent.doneMoving)
                        {
                            int mapX = (int)Mathf.Round(hitInfo2.point.x + mapManager.halfMapWidth);
                            int mapY = (int)Mathf.Round(hitInfo2.point.y + mapManager.halfMapHeight);

                            Agent ag = GetAgentAt(mapX, mapY);

                            if (ag != null) return;

                            if ((mapX > -1 && mapX < mapManager.mapWidth) && (mapY > -1 && mapY < mapManager.mapHeight))
                            {
                                pathFinder.SetStartNode(selectedAgent.x, selectedAgent.y);
                                pathFinder.SetGoalNode(mapX, mapY);
                                pathFinder.StartSearch(CreateUpdatedMap(-1, -1));
                                pathFinder.GetPath();
                                int dist = pathFinder.GetGoalScore();

                                if (dist <= selectedAgent.curActionPoints)
                                {
                                    selectedAgent.MovePath(pathFinder.finalPath);
                                }
                            }
                        }
                    }
                }

                UpdateUI();
            }
        }

        private void PlayerUpdate3D()
        {
            GraphicRaycaster gr = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
            PointerEventData ped = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            ped.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            gr.Raycast(ped, raycastResults);

            if (raycastResults.Count > 0)
            {
                ClearPathWay();
                return;
            }

            RaycastHit[] hitArray = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
            foreach (var hitInfo in hitArray)
            {
                if (hitInfo.transform.tag == "Agent")
                {
                    //ClearPathWay();
                    Agent cursorAgent = hitInfo.transform.gameObject.GetComponent<Agent>();

                    ClearPathWay();

                    if (cursorAgent.teamId == 0)
                    {
                        Vector3 posEnd = new Vector3((float)(cursorAgent.x - mapManager.halfMapWidth), 0.015f, (float)(cursorAgent.y - mapManager.halfMapHeight));
                        GameObject pathEnd = GameObject.Instantiate(agentTilePrefab, posEnd, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
                        pathEnd.gameObject.name = "PathWay_End"; // + (pathFinder.finalPath.Count - 1).ToString();
                        pathWay.Add(pathEnd);
                        Cursor.SetCursor(selectCursor, Vector2.zero, CursorMode.Auto);
                    }
                    else if (cursorAgent.teamId == 1)
                    {
                        Vector3 posEnd = new Vector3((float)(cursorAgent.x - mapManager.halfMapWidth), 0.015f, (float)(cursorAgent.y - mapManager.halfMapHeight));
                        GameObject pathEnd = GameObject.Instantiate(cantMoveTilePrefab, posEnd, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
                        pathEnd.gameObject.name = "PathWay_End"; // + (pathFinder.finalPath.Count - 1).ToString();
                        pathWay.Add(pathEnd);
                        Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
                    }

                    break;
                }
                else if (hitInfo.transform.tag == "Ground")
                {
                    int mapX = (int)Mathf.Round(hitInfo.point.x + mapManager.halfMapWidth);
                    int mapY = (int)Mathf.Round(hitInfo.point.z + mapManager.halfMapHeight);

                    Debug.Log("Coords: " + mapX.ToString() + ",  " + mapY.ToString());
                    if (((mapX > -1 && mapX < mapManager.mapWidth) && (mapY > -1 && mapY < mapManager.mapHeight)) && (selectedAgent.doneMoving))
                    {

                        //Debug.Log(mapX.ToString() + ",  " + mapY.ToString());

                        ClearPathWay();
                        pathFinder.SetStartNode(selectedAgent.x, selectedAgent.y);
                        pathFinder.SetGoalNode(mapX, mapY);
                        pathFinder.StartSearch(CreateUpdatedMap(-1, -1));
                        pathFinder.GetPath();
                        //int dist = pathFinder.GetGoalScore();

                        //if (dist <= selectedAgent.curActionPoints)
                        MakePathWay();

                    }
                    else
                    {
                        ClearPathWay();
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (!selectedAgent.doneMoving) return;

                RaycastHit[] hitArray2 = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));

                Agent agent = null; // IsClickedOnAgent(hitArray2);

                if (agent != null)
                {
                    if (Vector3.Distance(selectedAgent.transform.position, agent.transform.position) < 2f)
                    {
                        if (selectedAgent.curActionPoints >= 4)
                            selectedAgent.AttackTo(agent);
                    }
                    else
                    {
                        pathFinder.SetStartNode(selectedAgent.x, selectedAgent.y);
                        pathFinder.SetGoalNode(agent.x, agent.y);
                        pathFinder.StartSearch(CreateUpdatedMap(-1, -1));
                        pathFinder.GetPath();


                        if (pathFinder.GetGoalScore() > 1)
                        {
                            int newX = pathFinder.finalPath[1].x;
                            int newY = pathFinder.finalPath[1].y;

                            pathFinder.SetStartNode(selectedAgent.x, selectedAgent.y);
                            pathFinder.SetGoalNode(newX, newY);
                            pathFinder.StartSearch(CreateUpdatedMap(-1, -1));
                            pathFinder.GetPath();
                        }


                        int dist = pathFinder.GetGoalScore() + 4;

                        if (dist <= selectedAgent.curActionPoints)
                        {
                            selectedAgent.MovePathAndAttack(pathFinder.finalPath, agent);
                        }
                    }
                }
                else
                {
                    foreach (var hitInfo2 in hitArray2)
                    {
                        if (hitInfo2.transform.tag == "Ground" && selectedAgent.doneMoving)
                        {
                            int mapX = (int)Mathf.Round(hitInfo2.point.x + mapManager.halfMapWidth);
                            int mapY = (int)Mathf.Round(hitInfo2.point.z + mapManager.halfMapHeight);

                            Agent ag = GetAgentAt(mapX, mapY);

                            if (ag != null) return;

                            if ((mapX > -1 && mapX < mapManager.mapWidth) && (mapY > -1 && mapY < mapManager.mapHeight))
                            {
                                pathFinder.SetStartNode(selectedAgent.x, selectedAgent.y);
                                pathFinder.SetGoalNode(mapX, mapY);
                                pathFinder.StartSearch(CreateUpdatedMap(-1, -1));
                                pathFinder.GetPath();
                                int dist = pathFinder.GetGoalScore();

                                if (dist <= selectedAgent.curActionPoints)
                                {
                                    selectedAgent.MovePath(pathFinder.finalPath);
                                }
                            }
                        }
                    }
                }

                UpdateUI();
            }
        }

        private Agent IsClickedOnAgent(RaycastHit2D[] hitArray2)
		{
			foreach (var hitInfo2 in hitArray2)
			{
				if (hitInfo2.transform.tag == "Agent")
				{
					return hitInfo2.transform.gameObject.GetComponent<Agent>();
				}
			}

			return null;
		}

		/*
		private void PassTurnToPlayer()
		{
			turnId = 0;

			Debug.Log("Players Turn");
			foreach (Agent agent in teamPlayer)
			{
				if (!agent.isDead)
				{
					agent.curActionPoints = agent.maxActionPoints;
				}
			}
		}
		*/

		public Agent GetAgentAt(int mapX, int mapY)
		{

			foreach (Agent agent in teamPlayer)
			{
				if (agent.isDead) continue;

				if (agent.x == mapX && agent.y == mapY)
					return agent;
			}

			foreach (Agent agent in teamEnemy)
			{
				if (agent.isDead) continue;

				if (agent.x == mapX && agent.y == mapY)
					return agent;
			}

			return null;
		}

		private void CheckIfTheBattleEnded()
		{
			int deadA = 0;
			int deadB = 0;

			//hasBattleEnded = false;

			for (int i = 0; i < teamPlayer.Count; i++)
			{
				if (teamPlayer[i].isDead)
					deadA++;
			}

			for (int i = 0; i < teamEnemy.Count; i++)
			{
				if (teamEnemy[i].isDead)
					deadB++;
			}

			if (teamPlayer.Count == deadA)
			{
				_hasBattleEnded = true;
				WhoWon = 1;
			}
			else if (teamEnemy.Count == deadB)
			{
				_hasBattleEnded = true;
				WhoWon = 0;
			}
		}

		public void EndTheGame()
		{
			if (CombatManager.instance.WhoWon == 0)
			{
				uiManager.EndText.text = "You have won the battle!";
			}
			else if (CombatManager.instance.WhoWon == 1)
			{
				uiManager.EndText.text = "You have lost the battle!";
			}

			uiManager.EndPanel.SetActive(true);
		}

		public override void Initialize(string filename)
		{
			mapManager = GameObject.FindObjectOfType<MapManager>();
			uiManager = GameObject.FindObjectOfType<UIManager>();

			pathWay = new List<GameObject>();
			pathFinder = new AStar();
			pathFinder.SetMapSize(mapManager.mapWidth, mapManager.mapHeight);
			pathFinder.isDiagonalMovementAllowed = enableDiagonalMove;
			pathFinder.isNodeCostEnabled = false;
			WhoWon = -1;
			_hasBattleEnded = false;

			Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);

			LoadEncounterData(filename);
		}

		public void LoadEncounterData(string filename)
		{

			string filePath = Application.dataPath + "/" + filename;

			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			XmlNode root = doc.SelectSingleNode("Encounters");
			XmlNodeList encounterList = root.SelectNodes(".//Encounter");

			foreach (XmlNode enc in encounterList)
			{
				EncounterData encData = new EncounterData();
				string encounterName = enc.Attributes["name"].Value;
				XmlNodeList chrList = enc.SelectNodes(".//Character");
				encData.CharacterIds = new int[chrList.Count];
				for (int i = 0; i < chrList.Count; i++)
				{
					XmlNode chr = chrList.Item(i);
					int chrid = int.Parse(chr.Attributes["id"].Value);
					encData.CharacterIds[i] = chrid;
				}

				XmlNode winNode = enc.SelectSingleNode("WinState");
				StateChange winState = new StateChange();
				winState.ModuleName = winNode.Attributes["moduleName"].Value;
				winState.Value = winNode.Attributes["value"].Value;
				encData.WinState = winState;

				XmlNode loseNode = enc.SelectSingleNode("LoseState");
				StateChange loseState = new StateChange();
				loseState.ModuleName = loseNode.Attributes["moduleName"].Value;
				loseState.Value = loseNode.Attributes["value"].Value;
				encData.LoseState = loseState;

				GameManager.instance.EncounterList.Add(encounterName, encData);
			}
		}

		public override void Pause()
		{
			base.Pause();

			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}

		public override void Resume()
		{
			base.Resume();

			Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);
		}

		public override void Quit()
		{
			base.Quit();
		}
	}
}
