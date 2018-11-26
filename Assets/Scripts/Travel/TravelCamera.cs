using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails.Travel
{
	public class TravelCamera : MonoBehaviour
	{

		public int CameraMoveSpeed = 100;
		public int ScrollSpeed = 25;
		public bool EnableBorderMove;
		private bool _isFollowing;
		private GameObject _followTarget;

        private float rightBound;
        private float leftBound;
        private float topBound;
        private float bottomBound;

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
                    if (Input.mousePosition.x < (Screen.width * 0.9f))
					{
						translation += transform.right * -ScrollSpeed * Time.deltaTime;
					}

                    if (Input.mousePosition.x >= (Screen.width * 0.1f))
                    {
                        translation += transform.right * ScrollSpeed * Time.deltaTime;
                    }

					if (Input.mousePosition.y < (Screen.height * 0.9f))
					{
						translation += transform.up * -ScrollSpeed * Time.deltaTime;
					}

                    if (Input.mousePosition.y > (Screen.height * 0.1f))
                    {
                        translation += transform.up * ScrollSpeed * Time.deltaTime;
                    }
				}

				transform.position += translation;

                var pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);
                pos.y = Mathf.Clamp(pos.y, bottomBound, topBound);
                transform.position = pos;
            }
			else
			{
				Vector3 newPos = Vector3.MoveTowards(transform.position, _followTarget.transform.position, 0.05f);
				newPos.z = -5f;
				transform.position = newPos;

                var pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);
                pos.y = Mathf.Clamp(pos.y, bottomBound, topBound);
                transform.position = pos;


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

        public void CalculateMapBounds(SpriteRenderer spriteBounds)
        {
            float vertExtent = Camera.main.orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;
            
            leftBound = (float)(horzExtent - spriteBounds.sprite.bounds.size.x / 2.0f);
            rightBound = (float)(spriteBounds.sprite.bounds.size.x / 2.0f - horzExtent);
            bottomBound = (float)(vertExtent - spriteBounds.sprite.bounds.size.y / 2.0f);
            topBound = (float)(spriteBounds.sprite.bounds.size.y / 2.0f - vertExtent);
        }
	}
}
