using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails.UI
{

    public enum TileCursorEffectType
    {
        None,
        Rotating,
        Swinging,
        InOut,
        Disappear,
        RotatingDisappear
    }

    public class TileCursorEffect : MonoBehaviour
    {
        public TileCursorEffectType EffectType;
        public float EffectSpeed;
        public float EffectSize;
        public bool stop;

        float _timer;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _timer += Time.deltaTime;
            if (EffectType == TileCursorEffectType.Rotating)
            {
                transform.RotateAround(transform.position, new Vector3(0, 1, 0), EffectSpeed);
            }
            else if (EffectType == TileCursorEffectType.Swinging)
            {
                float sign = (EffectSize / 2f) - Mathf.PingPong(Time.time, EffectSize);
                transform.RotateAround(transform.position, new Vector3(0, 1, 0), sign * EffectSpeed);
            }
            else if (EffectType == TileCursorEffectType.InOut)
            {
                float val = Mathf.PingPong(_timer, EffectSize) + EffectSpeed;
                Debug.Log(val);
                transform.localScale = new Vector3(val, val, transform.localScale.z);
            }
            else if (EffectType == TileCursorEffectType.Disappear)
            {
                transform.localScale = new Vector3(Mathf.PingPong(Time.deltaTime, 1) * EffectSize, Mathf.PingPong(Time.time, 1) * EffectSize, transform.localScale.z);
                if (transform.localScale.x < 0.1f)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (EffectType == TileCursorEffectType.RotatingDisappear)
            {
                transform.RotateAround(transform.position, new Vector3(0, 1, 0), EffectSpeed);
                transform.localScale = new Vector3(Mathf.PingPong(Time.time, 1) * EffectSize, Mathf.PingPong(Time.time, 1) * EffectSize, transform.localScale.z);
                if (transform.localScale.x < 0.1f)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
