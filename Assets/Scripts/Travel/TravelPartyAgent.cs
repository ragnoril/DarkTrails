using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails.Travel
{
	public class TravelPartyAgent : MonoBehaviour
	{

		private List<Vector3> _pathToMove;
		private int _positionCount;
		public bool IsMoving;

		public TravelNodeAgent LastNodeAgent = null;

		// Use this for initialization
		void Start()
		{
			IsMoving = false;
			_pathToMove = new List<Vector3>();
			_positionCount = 0;
		}

		// Update is called once per frame
		void Update()
		{
			if (IsMoving)
			{
				Vector3 newPos = Vector3.MoveTowards(transform.position, _pathToMove[_positionCount], 0.05f);
				transform.position = newPos;

				if (Vector3.Distance(_pathToMove[_positionCount], transform.position) < 0.1f)
				{
					int mX = (int)(newPos.x + (TravelManager.instance.CurrentMap.MapWidth / 2)) / TravelManager.instance.CurrentMap.GridSize;
					int mY = (int)(Mathf.Abs(newPos.y - (TravelManager.instance.CurrentMap.MapHeight / 2))) / TravelManager.instance.CurrentMap.GridSize;


					if (_positionCount == (_pathToMove.Count - 1))
					{
						IsMoving = false;
					}
					else
					{
						_positionCount += 1;
					}
				}
			}
		}

		public void SetTargetPath(List<Vector3> path)
		{
			_pathToMove.Clear();
			foreach (var pos in path)
			{
				_pathToMove.Add(pos);
			}
		}

		public void StartMoving()
		{
			_positionCount = 0;
			IsMoving = true;
		}
	}
}
