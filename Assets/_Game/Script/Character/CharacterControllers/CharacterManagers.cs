using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class CharacterManagers : MonoBehaviourPunCallbacks
    {
        public delegate void CharacterManagersCharacterGenerated(int playerPhotonViewID);

        [SerializeField] private GameObject characterPrefab;
        [SerializeField] public PhotonView characterManagerPhotonView;

        private CharacterBase _characterBase;

        private List<int> _allCharacterViewID = new List<int>();


        public override void OnEnable()
        {
            base.OnEnable();

            RoomController.RoomFulled += OnRoomFulled;

            CharacterBase.CharacterGenerated += OnCharacterGenerated;
        }
        public override void OnDisable()
        {
            base.OnDisable();

            RoomController.RoomFulled -= OnRoomFulled;

            CharacterBase.CharacterGenerated -= OnCharacterGenerated;
        }


        private void OnRoomFulled()
        {
            if (characterManagerPhotonView == null)
            {
                return;
            }

            characterManagerPhotonView.RPC("PunRPC_CharacterGenerate", RpcTarget.All);
        }


        private void OnCharacterGenerated(int playerPhotonViewID)
        {
            if (characterManagerPhotonView == null)
            {
                return;
            }

            characterManagerPhotonView.RPC("PunRPC_CharacterGenerated", RpcTarget.All, playerPhotonViewID);
        }


        [PunRPC]
        private void PunRPC_CharacterGenerate()
        {
            if (_characterBase != null)
            {
                return;
            }

            if (characterPrefab == null)
            {
                return;
            }

            GameObject createdCharacter = PhotonNetwork.Instantiate(characterPrefab.name, Vector3.zero, Quaternion.identity);

            if (createdCharacter == null)
            {
                return;
            }

            _characterBase = createdCharacter.GetComponent<CharacterBase>();

            if (_characterBase != null)
            {
                // _characterBase.CharacterSpriteRendererEnable(false);

                _characterBase.CharacterInitialize();
            }
        }


        [PunRPC]
        private void PunRPC_CharacterGenerated(int playerPhotonViewID)
        {
            if (_allCharacterViewID == null)
            {
                _allCharacterViewID = new List<int>();
            }


            if (!_allCharacterViewID.Contains(playerPhotonViewID))
            {
                _allCharacterViewID.Add(playerPhotonViewID);
            }


            if (_allCharacterViewID.Count == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                GameManager.Instance.SetState(GameState.Game_MATCH_FOUND);
            }
        }
    }
}