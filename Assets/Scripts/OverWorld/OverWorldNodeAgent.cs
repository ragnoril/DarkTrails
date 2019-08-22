using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkTrails.OverWorld
{
    public enum ActionType
    {
        None = 0,
        OpenMap,
        OpenDialog,
        OpenScene,
        OpenCombat
    };

    public class OverWorldNodeAgent : MonoBehaviour
    {
        public string Name;
        public ActionType Action;
        public string ActionValue;
        public float x, y;

        private bool _isAlreadyActive;

        // Start is called before the first frame update
        void Start()
        {
            _isAlreadyActive = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerExit(Collider collision)
        {
            if (OverWorldManager.instance.IsPaused) return;

            var player = collision.gameObject.GetComponent<OverWorldPartyAgent>();
            if (player != null)
            {
                _isAlreadyActive = false;
                if (player.LastNodeAgent == this)
                    player.LastNodeAgent = null;
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            var player = collision.gameObject.GetComponent<OverWorldPartyAgent>();
            if (player != null)
            {
                //toDo: another quick fix. there should be a menu/panel for  this. 
                //actually there is. make it working.
                if (player.LastNodeAgent == this) return;
                if (_isAlreadyActive) return;
                _isAlreadyActive = true;

                player.LastNodeAgent = this;
                if (Action == ActionType.OpenCombat)
                {
                    OverWorldManager.instance.OverWorldActionOpenCombat(ActionValue);
                }
                else if (Action == ActionType.OpenDialog)
                {
                    OverWorldManager.instance.OverWorldActionOpenDialog(ActionValue);
                }
                else if (Action == ActionType.OpenMap)
                {
                    OverWorldManager.instance.OverWorldActionOpenMap(ActionValue);
                }
                else if (Action == ActionType.OpenScene)
                {
                    OverWorldManager.instance.OverWorldActionOpenScene(ActionValue);
                }
            }
        }
    }
}
