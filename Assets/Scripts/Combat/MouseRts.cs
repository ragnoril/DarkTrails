using UnityEngine;

namespace DarkTrails.Combat
{
	public class MouseRts : MonoBehaviour
	{
		public int LevelArea = 100;

		public int ScrollArea = 25;
		public int ScrollSpeed = 25;
		public int CameraMoveSpeed = 100;

		public int ZoomSpeed = 25;
		public int ZoomMin = 20;
		public int ZoomMax = 100;

		public int PanSpeed = 50;
		public int PanAngleMin = 25;
		public int PanAngleMax = 80;

		public int RotateSpeed = 100;

		private float xDeg;
		private float yDeg;
		private Quaternion fromRotation;
		private Quaternion toRotation;

		public bool EnableBorderMove;

		// Update is called once per frame
		void Update()
		{
			// Init camera translation for this frame.
			var translation = Vector3.zero;
			/*
			// Zoom in or out
			var zoomDelta = Input.GetAxis("Mouse ScrollWheel")*ZoomSpeed*Time.deltaTime;
			if (zoomDelta!=0)
			{
				//translation -= Camera.main.transform.up * ZoomSpeed * zoomDelta;
				translation.y -= ZoomSpeed * zoomDelta;
			}

			// Start panning camera if zooming in close to the ground or if just zooming out.

			float yAngle = GetComponent<Camera>().transform.eulerAngles.y;
			var pan = GetComponent<Camera>().transform.eulerAngles.x - zoomDelta * PanSpeed;
			pan = Mathf.Clamp(pan, PanAngleMin, PanAngleMax);
			if (zoomDelta < 0 || GetComponent<Camera>().transform.position.y < (ZoomMax / 2))
			{
				GetComponent<Camera>().transform.eulerAngles = new Vector3(pan, yAngle, 0);
			}
			*/

			// Move camera with arrow keys
			//translation += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

			translation += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * CameraMoveSpeed;

			translation += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * CameraMoveSpeed;
			translation.y = 0f;
			/*
			// Move camera with mouse
			if (Input.GetMouseButton(2)) // MMB
			{
				// Hold button and drag camera around
				translation -= new Vector3(Input.GetAxis("Mouse X") * DragSpeed * Time.deltaTime, 0, 
										   Input.GetAxis("Mouse Y") * DragSpeed * Time.deltaTime);
			}
			*/
			if ((Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftControl))) || Input.GetMouseButton(1))  // RMB
			{
				//float xAngle = GetComponent<Camera>().transform.eulerAngles.x;
				xDeg += Input.GetAxis("Mouse X") * CameraMoveSpeed;
				yDeg -= Input.GetAxis("Mouse Y") * CameraMoveSpeed;

				if (yDeg < PanAngleMin)
					yDeg = PanAngleMin;
				if (yDeg > PanAngleMax)
					yDeg = PanAngleMax;
				fromRotation = transform.rotation;
				toRotation = Quaternion.Euler(yDeg, xDeg, 0f);
				transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * RotateSpeed);

			}
			else
			{
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
						translation += transform.forward * -ScrollSpeed * Time.deltaTime;
					}

					if (Input.mousePosition.y > Screen.height - ScrollArea)
					{
						translation += transform.forward * ScrollSpeed * Time.deltaTime;
					}
				}

			}
			/*
			translation.y = 0f;

			if (Input.GetKey(KeyCode.KeypadMinus))
			{
				translation.y += (Time.deltaTime * ZoomSpeed);
			}
			if (Input.GetKey(KeyCode.KeypadPlus))
			{
				translation.y -= (Time.deltaTime * ZoomSpeed);
			}
			*/
			// Zoom in or out
			var zoomDelta = Input.GetAxis("Mouse ScrollWheel") * CameraMoveSpeed * Time.deltaTime;
			if (zoomDelta != 0)
			{
				translation += transform.forward * ZoomSpeed * zoomDelta;
				//translation.y -= (ZoomSpeed * zoomDelta * 2);
			}

			// Keep camera within level and zoom area
			var desiredPosition = transform.position + translation;
			if (desiredPosition.x < -LevelArea || LevelArea < desiredPosition.x)
			{
				translation.x = 0f;
			}
			if (desiredPosition.y < ZoomMin || ZoomMax < desiredPosition.y)
			{
				translation.y = 0f;
			}
			if (desiredPosition.z < -LevelArea || LevelArea < desiredPosition.z)
			{
				translation.z = 0f;
			}

			// Finally move camera parallel to world axis
			transform.position += translation;
		}
	}
}
