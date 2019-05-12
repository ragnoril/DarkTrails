using System.Collections;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DarkTrails.Travel
{
	public class MapData : MonoBehaviour
	{
		public GameObject NodeAgentPrefab;
		public Sprite MapBackgroundImage;
		public SpriteRenderer MapBackgroundObject;
		public int GridWidth, GridHeight;
		public int GridSize;
		public int MapWidth, MapHeight;
        public float PlayerX, PlayerY;
		public List<int> MapGrid = new List<int>();
		public List<TravelNodeAgent> Nodes = new List<TravelNodeAgent>();

		public void LoadMap(string filename)
		{
			string filePath = Application.dataPath + "/" + filename;
			XmlDocument doc = new XmlDocument();
			doc.Load(filePath);

			XmlNode root = doc.SelectSingleNode("Map");
			XmlNode mapInfo = root.SelectSingleNode("MapInfo");
			MapWidth = int.Parse(mapInfo.Attributes["mapWidth"].Value);
			MapHeight = int.Parse(mapInfo.Attributes["mapHeight"].Value);
			float halfWidth = MapWidth / 2;
			float halfHeight = MapHeight / 2;

            PlayerX = (float.Parse(mapInfo.Attributes["playerX"].Value) - halfWidth) / -100f;
            PlayerY = (float.Parse(mapInfo.Attributes["playerY"].Value) - halfHeight) / -100f;

            MapBackgroundImage = (Sprite)Resources.Load(mapInfo.Attributes["mapImage"].Value, typeof(Sprite));
			MapBackgroundObject.sprite = MapBackgroundImage;

			XmlNode mapGrid = root.SelectSingleNode("Grid");
			GridWidth = int.Parse(mapGrid.Attributes["gridWidth"].Value);
			GridHeight = int.Parse(mapGrid.Attributes["gridHeight"].Value);
			GridSize = int.Parse(mapGrid.Attributes["gridSize"].Value);
			string[] gridTxt = mapGrid.InnerText.Split(',');
			for (int i = 0; i < (GridWidth * GridHeight); i++)
			{
				MapGrid.Add(int.Parse(gridTxt[i]));
			}

			XmlNode nodeMain = root.SelectSingleNode("Nodes");
			XmlNodeList nodes = nodeMain.SelectNodes("Node");
			foreach (XmlNode node in nodes)
			{
				GameObject go = GameObject.Instantiate(NodeAgentPrefab, Vector3.zero, Quaternion.identity);
				TravelNodeAgent nodeAgent = go.GetComponent<TravelNodeAgent>();
				nodeAgent.Action = (ActionType)Enum.Parse(typeof(ActionType), node.Attributes["actionType"].Value);
				nodeAgent.ActionValue = node.Attributes["actionValue"].Value;
				nodeAgent.Name = node.Attributes["name"].Value;
                //old formula
                /*
				nodeAgent.x = float.Parse(node.Attributes["x"].Value);
				nodeAgent.y = float.Parse(node.Attributes["y"].Value);
                */
                //new formula
                nodeAgent.x = (float.Parse(node.Attributes["x"].Value) - (MapWidth/2)) / 100f;
                nodeAgent.y = (float.Parse(node.Attributes["y"].Value) - (MapHeight/2)) / -100f;
                nodeAgent.NodeIconSprite = (Sprite)Resources.Load(node.Attributes["iconName"].Value, typeof(Sprite));
				nodeAgent.UpdateNode();
				go.transform.SetParent(this.transform);
                if (node.Attributes["State"].Value == "0")
                {
                    nodeAgent.gameObject.SetActive(false);
                }
                Nodes.Add(nodeAgent);
			}
		}

        public void SetNodeState(string nodeName, bool state)
        {
            int id = 0;
            foreach(var node in Nodes)
            {
                if (node.Name == nodeName)
                {
                    break;
                }
                id++;
            }

            if (id < Nodes.Count)
            {
                Nodes[id].gameObject.SetActive(state);
            }
        }

        public void SetNodeStateById(int id, bool state)
        {
            Nodes[id].gameObject.SetActive(state);
        }

    }
}
