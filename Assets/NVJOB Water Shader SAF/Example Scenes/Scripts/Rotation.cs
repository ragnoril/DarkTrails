﻿using UnityEngine;


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*

public class Rotation : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public float rotSpeedX = 0.5f;
    public float rotLengthX = 40;
    [Space(10)]
    public float rotSpeedY = 2.5f;
    [Space(10)]
    public float rotSpeedZ = -0.4f;
    public float rotLengthZ = 80;

    //--------------

    Transform tr;
    Vector3 rotationStart;


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void Awake()
    {
        //--------------

        tr = transform;
        rotationStart = tr.eulerAngles;

        //--------------
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void LateUpdate()
    {
        //--------------

        float timeTime = Time.time;

        float rtx = rotationStart.x + (((rotLengthX * 0.5f) - Mathf.PingPong(timeTime, rotLengthX)) * rotSpeedX);
        float rty = rotationStart.y + (rotSpeedY * timeTime);
        float rtz = rotationStart.z + (((rotLengthZ * 0.5f) - Mathf.PingPong(timeTime, rotLengthZ)) * rotSpeedZ);

        tr.rotation = Quaternion.Euler(new Vector3(rtx, rty, rtz));

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
*/