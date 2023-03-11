using System;
using UnityEngine;

namespace Wonnasmith
{
    public class UIMatchPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject matchPanel;
        [SerializeField] private TMPro.TMP_Text messageText;

        private const string message1 = "The game will begin in";
        private const string message2 = "seconds...";


        private void OnEnable()
        {
            GameManager.MatchWaiting += OnMatchWaiting;
            GameManager.TourStart += OnTourStart;

            MatchController.TimeDownChange += OnTimeDownChange;
        }
        private void OnDisable()
        {
            GameManager.MatchWaiting -= OnMatchWaiting;
            GameManager.TourStart -= OnTourStart;

            MatchController.TimeDownChange -= OnTimeDownChange;
        }


        private void OnMatchWaiting()
        {
            matchPanel.SetActiveNullCheck(true);
        }


        private void OnTourStart()
        {
            matchPanel.SetActiveNullCheck(false);
        }


        private void OnTimeDownChange(float currentTimeDown)
        {
            if (messageText == null)
            {
                return;
            }

            messageText.text = message1 + " " + (int)currentTimeDown + " " + message2;
        }
    }
}