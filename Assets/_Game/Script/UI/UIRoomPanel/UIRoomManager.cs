using System;
using UnityEngine;

namespace Wonnasmith
{
    public class UIRoomManager : MonoBehaviour
    {
        public delegate void UIRoomManagerRoomCreateButtonClick();
        public static event /*UIRoomManager.*/UIRoomManagerRoomCreateButtonClick RoomCreateButtonClick;


        public void _BUTTON_RoomCreate()
        {
            RoomCreateButtonClick?.Invoke();
        }
    }
}