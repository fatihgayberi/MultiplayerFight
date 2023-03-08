using UnityEngine;
using WonnasmithTools;
using Photon.Pun;
using Photon.Realtime;
using System;

namespace Wonnasmith
{
    public class LobyManager : MonoBehaviourPunCallbacks
    {
        private static LobyManager Instance;

        private int _currentRoomMasterID = int.MaxValue;

        public override void OnEnable()
        {
            base.OnEnable();

            SceneManager.LoadedSceneManager += OnLoadedSceneManager;

            UIGameMainPanelController.Back2LobyButtonClick += OnUIGameMainPanelController;
        }
        public override void OnDisable()
        {
            base.OnDisable();

            SceneManager.LoadedSceneManager -= OnLoadedSceneManager;

            UIGameMainPanelController.Back2LobyButtonClick -= OnUIGameMainPanelController;
        }


        private void Awake()
        {
            if (Instance != null)
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
            Debug.Log("Start");

            PhotonNetwork.ConnectUsingSettings();
        }

        private void OnLoadedSceneManager(SceneManager.ScneType scneType)
        {
            if (scneType == SceneManager.ScneType.LobyScene)
            {
                Debug.Log("OnLoadedSceneManager:::" + scneType, gameObject);

                // PhotonNetwork.JoinLobby();
            }
        }

        private void OnUIGameMainPanelController()
        {
            Debug.Log("OnUIGameMainPanelController");

            if (PhotonNetwork.LocalPlayer.ActorNumber == PhotonNetwork.CurrentRoom.masterClientId)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;

                Debug.Log("CurrentRoom.IsOpen1111111" + PhotonNetwork.CurrentRoom.IsOpen);
            }

            PhotonNetwork.LeaveRoom();

            Debug.Log("CurrentRoom.IsOpen2222222" + PhotonNetwork.CurrentRoom.IsOpen);

            SceneManager.Instance.SceneLoader(SceneManager.ScneType.LobyScene);

            Debug.Log("CurrentRoom.IsOpen3333333" + PhotonNetwork.CurrentRoom.IsOpen);
        }


        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");

            PhotonNetwork.JoinLobby();
        }


        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");

            _currentRoomMasterID = PhotonNetwork.LocalPlayer.ActorNumber;

            SceneManager.Instance.SceneLoader(SceneManager.ScneType.GameScene);
        }
    }
}