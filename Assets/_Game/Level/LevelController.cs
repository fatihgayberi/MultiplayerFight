using System;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class LevelController : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerController.CharacterDead += OnCharacterDead;
            AIController.CharacterDead += OnCharacterDead;
        }
        private void OnDisable()
        {
            PlayerController.CharacterDead -= OnCharacterDead;
            AIController.CharacterDead -= OnCharacterDead;
        }


        private void OnCharacterDead(bool isDeadMasterClient)
        {
            if (isDeadMasterClient)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    GameManager.Instance.SetState(GameState.GAME_TOUR_LOSE);
                }

                if (!PhotonNetwork.IsMasterClient)
                {
                    GameManager.Instance.SetState(GameState.GAME_TOUR_WIN);
                }
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    GameManager.Instance.SetState(GameState.GAME_TOUR_WIN);
                }

                if (!PhotonNetwork.IsMasterClient)
                {
                    GameManager.Instance.SetState(GameState.GAME_TOUR_LOSE);
                }
            }
        }
    }
}