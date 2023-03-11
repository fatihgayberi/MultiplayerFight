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

        public delegate void LobyManagerMasterClientLeft(Room room);
        public static event LobyManagerMasterClientLeft MasterClientLeft;

        private static LobyManager Instance;


        public override void OnEnable()
        {
            base.OnEnable();

            SceneManager.LoadedSceneManager += OnLoadedSceneManager;

            UIGameMainPanelController.BackToLoby += OnBackToLoby;
            RoomController.BackToLoby += OnBackToLoby;
        }
        public override void OnDisable()
        {
            base.OnDisable();

            SceneManager.LoadedSceneManager -= OnLoadedSceneManager;

            UIGameMainPanelController.BackToLoby -= OnBackToLoby;
            RoomController.BackToLoby -= OnBackToLoby;
        }


        private void Awake()
        {
            if (Instance != null & Instance != this)
            {
                Debug.Log("Destroy::", gameObject);

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

        }


        private void OnBackToLoby()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("IsMasterClient::LEFT:::YESSSSSSSSSSS", gameObject);

                Room room = PhotonNetwork.CurrentRoom;

                photonView.RPC("MasterClientLeft", RpcTarget.Others);//, room);

                PhotonNetwork.CurrentRoom.IsOpen = false;
            }

            if (PhotonNetwork.CurrentRoom != null)
            {
                PhotonNetwork.LeaveRoom();

                SceneManager.Instance.SceneLoader(SceneManager.ScneType.LobyScene);
            }
        }


        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");

            PhotonNetwork.JoinLobby();
        }


        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");

            SceneManager.Instance.SceneLoader(SceneManager.ScneType.GameScene);
        }
    }
}