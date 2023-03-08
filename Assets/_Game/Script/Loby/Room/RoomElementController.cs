using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

namespace Wonnasmith
{
    public class RoomElementController : MonoBehaviour
    {
        public static event RoomManager.RoomManagerJoinRoomButtonClick JoinRoomButtonClick;

        [SerializeField] private bool _isAvailableRoomElement = true;
        [SerializeField] private TMPro.TMP_Text roomPlayerCountText;
        [SerializeField] private TMPro.TMP_Text roomNameText;


        public void RoomElementReset()
        {
            SetRoomPlayerCountTextMessage("0/2");
            SetRoomName("Room_?");
        }


        public void SetRoomPlayerCountTextMessage(RoomInfo roomInfo)
        {
            if (roomInfo == null)
            {
                return;
            }


            if (roomPlayerCountText == null)
            {
                return;
            }

            roomPlayerCountText.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
        }


        public void SetRoomPlayerCountTextMessage(string playerCountMessage)
        {
            if (roomPlayerCountText == null)
            {
                return;
            }

            roomPlayerCountText.text = playerCountMessage;
        }


        public void SetRoomName(RoomInfo roomInfo)
        {
            if (roomInfo == null)
            {
                return;
            }

            if (roomNameText == null)
            {
                return;
            }

            roomNameText.text = roomInfo.Name;
        }


        public void SetRoomName(string name)
        {
            if (roomNameText == null)
            {
                return;
            }

            roomNameText.text = name;
        }


        public bool GetIsAvailableRoomElement() { return _isAvailableRoomElement; }
        public void SetIsAvailableRoomElement(bool isAvailableRoomElement) { _isAvailableRoomElement = isAvailableRoomElement; }


        public void _BUTTON_JoinRoom()
        {
            JoinRoomButtonClick?.Invoke(this);
        }
    }
}