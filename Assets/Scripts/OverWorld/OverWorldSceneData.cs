using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails.OverWorld
{
    public class OverWorldSceneData : MonoBehaviour
    {

        public Transform PlayerPosition;
        public List<OverWorldNodeAgent> Nodes = new List<OverWorldNodeAgent>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void SetNodeState(string nodeName, bool state)
        {
            int id = 0;
            foreach (var node in Nodes)
            {
                if (node.Name == nodeName)
                {
                    break;
                }
                id++;
            }

            if (id < Nodes.Count)
            {
                Nodes[id].gameObject.SetActive(state);
            }
        }

        public void SetNodeStateById(int id, bool state)
        {
            Nodes[id].gameObject.SetActive(state);
        }
    }
}
