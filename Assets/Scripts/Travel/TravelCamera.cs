using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelCamera : MonoBehaviour
{

	public int CameraMoveSpeed = 100;
	public int ScrollArea = 25;
	public int ScrollSpeed = 25;
	public bool EnableBorderMove;
	private bool _isFollowing;
	private GameObject _followTarget;

	// Use this for initialization
	void Start()
	{
		EnableBorderMove = true;
		_isFollowing = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (!_isFollowing)
		{
			var translation = Vector3.zero;

			translation += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * CameraMoveSpeed;

			translation += transform.up * Input.GetAxis("Vertical") * Time.deltaTime * CameraMoveSpeed;
			translation.z = 0f;


			if (EnableBorderMove)
			{
				// Move camera if mouse pointer reaches screen borders
				if (Input.mousePosition.x < ScrollArea)
				{
					translation += transform.right * -ScrollSpeed * Time.deltaTime;
				}

				if (Input.mousePosition.x >= Screen.width - ScrollArea)
				{
					translation += transform.right * ScrollSpeed * Time.deltaTime;
				}

				if (Input.mousePosition.y < ScrollArea)
				{
					translation += transform.up * -ScrollSpeed * Time.deltaTime;
				}

				if (Input.mousePosition.y > Screen.height - ScrollArea)
				{
					translation += transform.up * ScrollSpeed * Time.deltaTime;
				}
			}

			transform.position += translation;
		}
		else
		{
			Vector3 newPos = Vector3.MoveTowards(transform.position, _followTarget.transform.position, 0.05f);
			newPos.z = -5f;
			transform.position = newPos;


			if (Vector2.Distance(_followTarget.transform.position, transform.position) < 0.1f)
			{
				_isFollowing = false;
				
			}
		}
	}

	public void StartFollowing(GameObject target)
	{
		_followTarget = target;
		_isFollowing = true;
	}
}
