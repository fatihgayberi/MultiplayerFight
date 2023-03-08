using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Wonnasmith
{
    public class WonnaTest : MonoBehaviourPunCallbacks
    {
        // private void Start()
        // {
        //     PhotonNetwork.ConnectUsingSettings();
        // }


        // public override void OnConnectedToMaster()
        // {
        //     Debug.Log("OnConnectedToMaster");
        //     // servera girildi

        //     PhotonNetwork.JoinLobby();
        // }


        // public override void OnJoinedLobby()
        // {
        //     Debug.Log("OnJoinedLobby");

        //     PhotonNetwork.JoinOrCreateRoom("oda", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);

        //     // rastgele odaya girer eğer oda varsa tabi
        //     // PhotonNetwork.JoinRandomRoom();
        // }

        // [SerializeField] private GameObject player;
        // public override void OnJoinedRoom()
        // {
        //     Debug.Log("OnJoinedRoom");

        //     GameObject newPlayer = PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity);

        //     Debug.Log("yarattik");
        // }


        // public override void OnLeftLobby()
        // {
        //     Debug.Log("OnLeftLobby");
        // }


        // public override void OnLeftRoom()
        // {
        //     Debug.Log("OnLeftRoom");
        // }
    }
}
