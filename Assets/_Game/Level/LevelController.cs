using System;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class LevelController : MonoBehaviour
    {
        private void OnEnable()
        {
            CharacterBase.CharacterDead += OnCharacterDead;
        }
        private void OnDisable()
        {
            CharacterBase.CharacterDead -= OnCharacterDead;
        }


        private void OnCharacterDead(bool isDeadMasterClient)
        {
            if (isDeadMasterClient)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    GameManager.Instance.SetState(GameState.GAME_WIN);
                }
                else
                {
                    GameManager.Instance.SetState(GameState.GAME_LOSE);
                }
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    GameManager.Instance.SetState(GameState.GAME_LOSE);
                }
                else
                {
                    GameManager.Instance.SetState(GameState.GAME_WIN);
                }
            }
        }
    }
}