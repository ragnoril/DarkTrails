using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWorldCamera : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes Axes = RotationAxes.MouseXAndY;
    public float SensitivityX = 15f;
    public float SensitivityY = 15f;

    public float MinimumX = -360f;
    public float MaximumX = 360f;

    public float MinimumY = -60f;
    public float MaximumY = 60f;

    public float CameraMoveSpeed = 1.0f;
    public float CameraZoomSpeed = 1.0f;

    public bool LockHeight = false;
    public bool IsFollowing = true;
    public Transform FollowTarget;
    public float FollowSpeed = 1.5f;
    public float FollowDistance = 5f;

    private float _rotationY = 0f;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetUserInput();

        if (IsFollowing && FollowTarget != null)
        {
            /*
            if ((transform.position - FollowTarget.position).magnitude < FollowDistance)
            {
                IsFollowing = false;
                return;
            }
            */
            Vector3 startPos = transform.position;
            startPos.y = 0f;
            Vector3 targetPos = (FollowTarget.transform.position - (transform.forward.normalized * FollowDistance));
            targetPos.y = 0f;
            Vector3 newPos = Vector3.MoveTowards(startPos, targetPos, FollowSpeed);
            newPos.y = transform.position.y;
            transform.position = newPos;
            
        }
    }

    void GetUserInput()
    {
        var translation = Vector3.zero;

        if (Input.GetMouseButton(1))
        {
            if (Axes == RotationAxes.MouseXAndY)
            {
                float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * SensitivityX;

                _rotationY += Input.GetAxis("Mouse Y") * SensitivityY;
                _rotationY = Mathf.Clamp(_rotationY, MinimumY, MaximumY);

                transform.localEulerAngles = new Vector3(-_rotationY, rotationX, 0);
            }
            else if (Axes == RotationAxes.MouseX)
            {
                transform.Rotate(0, Input.GetAxis("Mouse X") * SensitivityX, 0);
            }
            else
            {
                _rotationY += Input.GetAxis("Mouse Y") * SensitivityY;
                _rotationY = Mathf.Clamp(_rotationY, MinimumY, MaximumY);

                transform.localEulerAngles = new Vector3(-_rotationY, transform.localEulerAngles.y, 0);
            }
        }

        var xAxisValue = Input.GetAxis("Horizontal");
        var zAxisValue = Input.GetAxis("Vertical");

        if (xAxisValue != 0f || zAxisValue != 0f)
        {
            IsFollowing = false;
        }
        var dir = transform.TransformDirection(new Vector3(xAxisValue, 0.0f, zAxisValue) * CameraMoveSpeed);
        dir.y = 0.0f;
        transform.position += dir;
        /*
        if (LockHeight)
        {
            var dir = transform.TransformDirection(new Vector3(xAxisValue, 0.0f, zAxisValue) * CameraMoveSpeed);
            dir.y = 0.0f;
            transform.position += dir;
        }
        else
        {
            //transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue) * CameraMoveSpeed);
            translation += (new Vector3(xAxisValue, 0.0f, zAxisValue) * CameraMoveSpeed);
        }
        */

        // Zoom in or out
        var zoomDelta = Input.GetAxis("Mouse ScrollWheel") * CameraMoveSpeed * Time.deltaTime;
        if (zoomDelta != 0)
        {
            translation += transform.forward * CameraZoomSpeed * zoomDelta;
        }
        transform.position += translation;
    }
}
