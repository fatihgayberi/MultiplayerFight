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
            GameManager.LevelPrepare += OnLevelPrepare;
        }
        private void OnDisable()
        {
            GameManager.LevelPrepare -= OnLevelPrepare;
        }


        private void OnLevelPrepare()
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