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


        public override void OnEnable()
        {
            base.OnEnable();

            GameManager.TourPrepare += OnTourPrepare;
        }
        public override void OnDisable()
        {
            base.OnDisable();

            GameManager.TourPrepare -= OnTourPrepare;
        }


        private void OnTourPrepare()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }


        [PunRPC]
        public void MasterClientLeft()
        {
            Debug.Log("MasterClientLeft::::", gameObject);

            BackToLoby?.Invoke();
        }


        public override void OnLeftRoom()
        {
            base.OnLeftRoom();

            RoomLeft?.Invoke();
        }



        private void Start()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                RoomFulled?.Invoke();
            }
        }
    }
}