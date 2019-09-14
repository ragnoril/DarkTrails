using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace DarkTrails.OverWorld
{
    public class OverWorldManager : BaseModule
    {
        private static OverWorldManager _instance;
        public static OverWorldManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<OverWorldManager>();
                    //DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }

        }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                //DontDestroyOnLoad(this);
            }
            else
            {
                if (this != _instance)
                    Destroy(this.gameObject);
            }
        }

        public OverWorldCamera OverWorldCamera;
        public bool IsPaused;

        public Dictionary<string, OverWorldSceneData> LoadedScenes;
        public OverWorldSceneData CurrentScene;

        public GameObject PartyPrefab;
        public OverWorldPartyAgent PlayerParty;

        public bool IsPlayerMoving;


        // Start is called before the first frame update
        void Start()
        {
            ModuleType = GAMEMODULES.OverWorld;
            IsPlayerMoving = false;
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
                {
                    PlayerParty.GoTo(hitInfo.point);
                    IsPlayerMoving = true;
                }
            }
        }

        public override void Initialize(string filename)
        {
            string filePath = Application.dataPath + "/" + filename;

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode root = doc.SelectSingleNode("SceneList");
            XmlNodeList sceneList = root.SelectNodes(".//Scene");

            foreach (XmlNode scene in sceneList)
            {
                string sceneName = scene.Attributes["name"].Value;
                string scenePrefabName = scene.Attributes["prefabName"].Value;

                GameManager.instance.SceneList.Add(sceneName, scenePrefabName);
            }

            LoadedScenes = new Dictionary<string, OverWorldSceneData>();
        }

        public override void Pause()
        {
            base.Pause();
            IsPaused = true;
        }

        public override void Resume()
        {
            base.Resume();
            IsPaused = false;
            if (OverWorldCamera == null)
                OverWorldCamera = Camera.main.GetComponent<OverWorldCamera>();
        }

        public override void Quit()
        {
            base.Quit();
        }

        public void LoadScene(string sceneName)
        {
            if (!LoadedScenes.ContainsKey(sceneName))
            {
                GameObject go = GameObject.Instantiate((GameObject)Resources.Load(GameManager.instance.SceneList[sceneName]));
                go.transform.SetParent(this.transform);
                CurrentScene = go.GetComponent<OverWorldSceneData>();
                LoadedScenes.Add(sceneName, CurrentScene);
            }
            else
            {
                if (CurrentScene != null)
                {
                    CurrentScene.gameObject.SetActive(false);
                }

                CurrentScene = LoadedScenes[sceneName].GetComponent<OverWorldSceneData>();
                CurrentScene.gameObject.SetActive(true); 
            }
            InitializePlayerParty();
        }

        public void InitializePlayerParty()
        {
            //toDo: another quick fix :(.
            if (PlayerParty == null)
            {
                GameObject go = GameObject.Instantiate(PartyPrefab, Vector3.zero, Quaternion.identity);
                PlayerParty = go.GetComponent<OverWorldPartyAgent>();
                go.transform.SetParent(this.transform);
                PlayerParty.SetPosition(CurrentScene.PlayerPosition.position);
            }
        }

        public void OverWorldActionOpenMap(string mapName)
        {
            Debug.Log("Node Action: Open Map - " + mapName);
        }

        public void OverWorldActionOpenScene(string sceneName)
        {
            Debug.Log("Node Action: Open Scene - " + sceneName);
        }

        public void OverWorldActionOpenDialog(string dialogName)
        {
            Debug.Log("Node Action: Open Dialog - " + dialogName);
            GameManager.instance.OpenDialogue(dialogName);
        }

        public void OverWorldActionOpenCombat(string combatName)
        {
            Debug.Log("Node Action: Open Combat - " + combatName);
            GameManager.instance.OpenCombat(combatName);
        }

        public void OverWorldActionEnableNode(string sceneName, string nodeName)
        {
            LoadedScenes[sceneName].SetNodeState(nodeName, true);
        }

        public void OverWorldActionDisableNode(string sceneName, string nodeName)
        {
            LoadedScenes[sceneName].SetNodeState(nodeName, false);
        }

        public void OverWorldActionEnableNodeById(string sceneName, int id)
        {
            LoadedScenes[sceneName].SetNodeStateById(id, true);
        }

        public void OverWorldActionDisableNodeById(string sceneName, int id)
        {
            var scene = LoadedScenes[sceneName];
            scene.SetNodeStateById(id, false);
        }

    }
}
