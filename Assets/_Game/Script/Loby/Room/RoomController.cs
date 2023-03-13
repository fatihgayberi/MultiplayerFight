using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;

namespace Wonnasmith
{
    public class RoomController : MonoBehaviourPunCallbacks
    {
        public static event LobyManager.LobyManagerBackToLoby BackToLoby;


        public delegate void RoomControllerRoomFulled();
        public static event RoomControllerRoomFulled RoomFulled;

        public delegate void RoomControllerRoomLeft();
        public static event RoomControllerRoomLeft RoomLeft;

        [SerializeField] private PhotonView roomPhotonView;

        public override void OnEnable()
        {
            base.OnEnable();

            GameManager.TourPrepare += OnTourPrepare;

            UIGameMainPanelController.BackToLoby += OnBackToLoby;
            RoomController.BackToLoby += OnBackToLoby;
        }
        public override void OnDisable()
        {
            base.OnDisable();

            GameManager.TourPrepare -= OnTourPrepare;

            UIGameMainPanelController.BackToLoby -= OnBackToLoby;
            RoomController.BackToLoby -= OnBackToLoby;
        }


        private void Start()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                RoomFulled?.Invoke();
            }
        }


        private void OnApplicationQuit()
        {
            roomPhotonView.RPC("PunRPC_OnBackToLoby", RpcTarget.All);
        }


        private void OnBackToLoby()
        {
            if (roomPhotonView == null)
            {
                return;
            }

            roomPhotonView.RPC("PunRPC_OnBackToLoby", RpcTarget.All);
        }

        [PunRPC]
        public void PunRPC_OnBackToLoby()
        {
            PhotonNetwork.LeaveRoom();
        }


        private void OnTourPrepare()
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }


        public override void OnLeftRoom()
        {
            base.OnLeftRoom();

            RoomLeft?.Invoke();
        }
    }
}