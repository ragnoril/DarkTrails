using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DarkTrails.OverWorld
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class OverWorldPartyAgent : MonoBehaviour
    {
        public float MoveSpeed;
        private NavMeshAgent _navMeshAgent;

        public OverWorldNodeAgent LastNodeAgent = null;

        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = MoveSpeed;
        }

        // Update is called once per frame
        void Update()
        {
            if (OverWorldManager.instance.IsPlayerMoving && _navMeshAgent.remainingDistance < 0.01f)
            {
                OverWorldManager.instance.IsPlayerMoving = false;
                OverWorldManager.instance.OverWorldCamera.IsFollowing = false;
            }
        }

        public void GoTo(Vector3 position)
        {
            _navMeshAgent.destination = position;
            OverWorldManager.instance.OverWorldCamera.FollowTarget = this.transform;
            OverWorldManager.instance.OverWorldCamera.IsFollowing = true;
        }

        public void ChangeMoveSpeed(float val)
        {
            MoveSpeed = val;
            _navMeshAgent.speed = MoveSpeed;
        }

        public void SetPosition(Vector3 position)
        {
            if (_navMeshAgent == null)
            {
                _navMeshAgent = this.GetComponent<NavMeshAgent>();
                _navMeshAgent.speed = MoveSpeed;
            }
            _navMeshAgent.enabled = false;
            transform.position = position;
            _navMeshAgent.enabled = true;

            /*
            OverWorldManager.instance.OverWorldCamera.FollowTarget = this.transform;
            float oldFollow = OverWorldManager.instance.OverWorldCamera.FollowDistance;
            OverWorldManager.instance.OverWorldCamera.FollowDistance = 1f;
            OverWorldManager.instance.OverWorldCamera.IsFollowing = true;
            OverWorldManager.instance.OverWorldCamera.FollowDistance = oldFollow;
            */
        }
    }
}
