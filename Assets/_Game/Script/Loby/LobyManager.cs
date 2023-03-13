using UnityEngine;
using WonnasmithTools;
using Photon.Pun;
using Photon.Realtime;
using System;

namespace Wonnasmith
{
    [DefaultExecutionOrder(-1)]
    public class LobyManager : MonoBehaviourPunCallbacks
    {
        public delegate void LobyManagerBackToLoby();

        public static LobyManager Instance;

        public override void OnEnable()
        {
            base.OnEnable();

            SceneManager.LoadedSceneManager += OnLoadedSceneManager;
        }
        public override void OnDisable()
        {
            base.OnDisable();

            SceneManager.LoadedSceneManager -= OnLoadedSceneManager;

        }

        private void Awake()
        {
            if (Instance != null & Instance != this)
            {
                Destroy(gameObject);
                return;
            }


            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }


        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }


        private void OnLoadedSceneManager(SceneManager.ScneType scneType)
        {
            if (scneType == SceneManager.ScneType.GameScene)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    GameManager.Instance.SetState(GameState.GAME_MATCH_WAIT);
                }
            }
        }


        public override void OnLeftRoom()
        {
            SceneManager.Instance.SceneLoader(SceneManager.ScneType.LobyScene);
        }


        public override void OnConnectedToMaster()
        {
            GameManager.Instance.SetState(GameState.GAME_PHOTON_CONNECTED);
            PhotonNetwork.JoinLobby();
        }


        public override void OnJoinedLobby()
        {
            GameManager.Instance.SetState(GameState.GAME_LOBBY_CONNECTED);
        }


        public override void OnJoinedRoom()
        {
            SceneManager.Instance.SceneLoader(SceneManager.ScneType.GameScene);
        }
    }
}