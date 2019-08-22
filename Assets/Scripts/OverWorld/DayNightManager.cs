using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails.OverWorld
{

    public class DayNightManager : MonoBehaviour
    {

        public Light SunLight;
        public Light MoonLight;

        public float DawnTimer;
        public float DuskTimer;

        public float TimeOfDay;
        public float TimePassRate = 0.1f;

        private float SunAngle;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            TimeOfDay += Time.deltaTime * TimePassRate;
            if (TimeOfDay > 24f) TimeOfDay = 0f;

            UpdateSunLight();
            UpdateMoonLight();
        }

        public void UpdateMoonLight()
        {
            if ((TimeOfDay > DuskTimer + 1f) || (TimeOfDay < DawnTimer + 1f))
            {
                MoonLight.gameObject.SetActive(true);
            }
            else
            {
                MoonLight.gameObject.SetActive(false);
            }

        }

        public void UpdateSunLight()
        {
            float dayLight = 180f / (DuskTimer - DawnTimer);

            SunAngle = (TimeOfDay - DawnTimer) * dayLight;
            if (SunAngle > 360f) SunAngle = 0f;

            SunLight.transform.rotation = Quaternion.Euler(SunAngle, -90f, 0f);
            
        }
    }
}