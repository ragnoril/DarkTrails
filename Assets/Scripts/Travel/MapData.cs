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
				nodeAgent.x = float.Parse(node.Attributes["x"].Value);
				nodeAgent.y = float.Parse(node.Attributes["y"].Value);
				nodeAgent.NodeIconSprite = (Sprite)Resources.Load(node.Attributes["iconName"].Value, typeof(Sprite));
				nodeAgent.UpdateNode();
				go.transform.SetParent(this.transform);
				Nodes.Add(nodeAgent);
			}

		}

	}
}
