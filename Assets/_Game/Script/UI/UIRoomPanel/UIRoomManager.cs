using System;
using UnityEngine;

namespace Wonnasmith
{
    public class UIRoomManager : MonoBehaviour
    {
        public delegate void UIRoomManagerRoomCreateButtonClick();
        public static event /*UIRoomManager.*/UIRoomManagerRoomCreateButtonClick RoomCreateButtonClick;

        [SerializeField] private TMPro.TMP_InputField playerNameInputField;


        private void OnEnable()
        {
            GameManager.TourPrepare += OnTourPrepare;
        }
        private void OnDisable()
        {
            GameManager.TourPrepare -= OnTourPrepare;
        }


        private void OnTourPrepare()
        {
            if (playerNameInputField != null)
            {
                Debug.Log("PlayerPrefsManager.GetPlayerName()::" + PlayerPrefsManager.GetPlayerName());

                playerNameInputField.text = PlayerPrefsManager.GetPlayerName();
            }
        }


        public void _BUTTON_RoomCreate()
        {
            RoomCreateButtonClick?.Invoke();
        }
    }
}