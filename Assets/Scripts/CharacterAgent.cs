using UnityEngine;
using System.Collections;

namespace DarkTrails
{
    public class CharacterAgent : MonoBehaviour
    {

        public CharacterData Character;


        /*
        		
        pathFinder = new AStar();
		pathFinder.isDiagonalMovementAllowed = false;
		pathFinder.SetMapSize(mapWidth, mapHeight);
        
        MapManager.instance.pathFinder.SetStartNode((int)transform.position.x, (int) transform.position.y);
		MapManager.instance.pathFinder.SetGoalNode((int) target.position.x, (int) target.position.y);
		MapManager.instance.pathFinder.StartSearch(MapManager.instance.tileMap);
		MapManager.instance.pathFinder.GetPath();

		if ((MapManager.instance.pathFinder.finalPath == null) || (MapManager.instance.pathFinder.finalPath.Count <= 1))
		{
			return;
		}
            
        */

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
