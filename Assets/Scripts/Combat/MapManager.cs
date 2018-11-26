using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DarkTrails.Combat
{
	public class MapManager : MonoBehaviour
	{
		public List<int> Grids = new List<int>();

		//public GameObject gridObject;
		public GameObject blockObject;


		public int mapWidth;
		public int mapHeight;

		public int halfMapWidth;
		public int halfMapHeight;

		public bool showMapGizmos;

		// Use this for initialization
		public void StartMap()
		{
			/*
			mapWidth = 18;
			mapHeight = 34;
			*/

			halfMapHeight = mapHeight / 2;
			halfMapWidth = mapWidth / 2;

			for (int i = 0; i < (mapWidth * mapHeight); i++)
				Grids.Add(0);

			AddBlocksToMap(-1);

			RenderMap2D();
			////CombatManager.instance.StartTheGame();
		}

		void AddBlocksToMap(int blockCount)
		{
			if (blockCount == -1) blockCount = 2 + Random.Range(2, halfMapHeight);

			for (int i = 0; i < blockCount; i++)
			{
				int x = Random.Range(0, mapWidth);
				int y = Random.Range(1, mapHeight - 1);
				while (Grids[x + (y * mapWidth)] != 0)
				{
					x = Random.Range(0, mapWidth);
					y = Random.Range(1, mapHeight - 1);
				}

				Grids[x + (y * mapWidth)] = -1;
			}
		}


		void RenderMap()
		{
			for (int i = 0; i < mapWidth; i++)
			{
				for (int j = 0; j < mapHeight; j++)
				{
					if (Grids[i + (j * mapWidth)] == 0)
					{
						/*
						GameObject go = GameObject.Instantiate(gridObject, new Vector3(i - (float)halfMapWidth, 0.01f, j - (float)halfMapHeight), Quaternion.Euler(90f, 0f, 0f)) as GameObject;
						go.transform.parent = transform;
						*/
					}
					else if (Grids[i + (j * mapWidth)] == -1)
					{
						GameObject go = GameObject.Instantiate(blockObject, new Vector3(i - (float)halfMapWidth, 0.01f, j - (float)halfMapHeight), Quaternion.Euler(270f, 0f, 0f)) as GameObject;
						go.transform.parent = transform;
					}
				}
			}

		}

        void RenderMap2D()
        {
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (Grids[i + (j * mapWidth)] == 0)
                    {
                        /*
						GameObject go = GameObject.Instantiate(gridObject, new Vector3(i - (float)halfMapWidth, 0.01f, j - (float)halfMapHeight), Quaternion.Euler(90f, 0f, 0f)) as GameObject;
						go.transform.parent = transform;
						*/
                    }
                    else if (Grids[i + (j * mapWidth)] == -1)
                    {
                        GameObject go = GameObject.Instantiate(blockObject, new Vector3(i - (float)halfMapWidth, j - (float)halfMapHeight, 0f), Quaternion.Euler(0f, 0f, 0f)) as GameObject;
                        go.transform.parent = transform;
                    }
                }
            }

        }

        void OnDrawGizmos()
		{
            OnDrawGizmos2D();
            /*
            // unfortunately tilemap is empty when this function is working so every tile is green.
            if (showMapGizmos)
			{
				float halfWidth = mapWidth / 2;
				float halfHeight = mapHeight / 2;
				Gizmos.color = Color.green;
				for (int i = 0; i < mapWidth; i++)
				{
					for (int j = 0; j < mapHeight; j++)
					{
						Vector3 start = new Vector3(i - halfWidth - 0.5f, 0.01f, j - halfHeight - 0.5f);

						Gizmos.DrawLine(start, start + new Vector3(1f, 0f, 0f));
						Gizmos.DrawLine(start, start + new Vector3(0f, 0f, 1f));
						Gizmos.DrawLine(start + new Vector3(1f, 0f, 0f), start + new Vector3(1f, 0f, 1f));
						Gizmos.DrawLine(start + new Vector3(0f, 0f, 1f), start + new Vector3(1f, 0f, 1f));
                    }
				}
			}
            */
		}

        void OnDrawGizmos2D()
        {

            // unfortunately tilemap is empty when this function is working so every tile is green.
            if (showMapGizmos)
            {
                float halfWidth = mapWidth / 2;
                float halfHeight = mapHeight / 2;
                Gizmos.color = Color.green;
                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapHeight; j++)
                    {

                        Vector3 start = new Vector3(i - halfWidth - 0.5f, j - halfHeight - 0.5f, 0f);

                        Gizmos.DrawLine(start, start + new Vector3(1f, 0f, 0f));
                        Gizmos.DrawLine(start, start + new Vector3(0f, 1f, 0f));
                        Gizmos.DrawLine(start + new Vector3(1f, 0f, 0f), start + new Vector3(1f, 1f, 0f));
                        Gizmos.DrawLine(start + new Vector3(0f, 1f, 0f), start + new Vector3(1f, 1f, 0f));

                    }
                }
            }

        }

        // Update is called once per frame
        void Update()
		{
            
		}
	}
}
